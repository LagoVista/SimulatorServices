using LagoVista.Core.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Simulator.Runtime.Portal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LagoVista.IoT.Simulator.Runtime.Portal
{
    public class Startup
    {
        

        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

            Configuration = builder.Build();

            var orgSection = Configuration.GetSection("org");
            var orgId = orgSection.GetValue<string>("id");
            var orgName = orgSection.GetValue<string>("name");

            var userSection = Configuration.GetSection("user");
            var userId = userSection.GetValue<string>("id");
            var userName = userSection.GetValue<string>("name");

            _org = EntityHeader.Create(orgId, orgName);
            _user = EntityHeader.Create(userId, userName);

            _simulatorId = Configuration.GetValue<string>("simulatorId");
            _accessKey = Configuration.GetValue<string>("accessKey");

            switch (env.EnvironmentName.ToLower())
            {
                case "development": _environemnt = Core.Interfaces.Environments.Development; break;
                case "test": _environemnt = Core.Interfaces.Environments.Testing; break;
                case "staging": _environemnt = Core.Interfaces.Environments.Testing; break;
                default: _environemnt = Core.Interfaces.Environments.Production; break;
            }

            Console.WriteLine("Simulator Startup");
            Console.WriteLine("====================================");
            Console.WriteLine($"Environment    : {_environemnt}");
            Console.WriteLine($"Org            : {orgId} - {orgName}");
            Console.WriteLine($"User           : {userId} - {userName}");
            Console.WriteLine($"Simulator      : {_simulatorId} - {_accessKey.Substring(0,10)}...");
        }

        EntityHeader _org;
        EntityHeader _user;
        string _simulatorId;
        string _accessKey;

        SimulatorRuntimeManager _simRuntimeManager;
        Core.Interfaces.Environments _environemnt;


        public IConfiguration Configuration { get; }

        public IServiceCollection _serviceCollection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _serviceCollection = services;
            _simRuntimeManager = new SimulatorRuntimeManager(new SimulatorRuntimeServicesFactory(), new AdminLogger(new LogWriter()));
            _serviceCollection.AddSingleton(_simRuntimeManager);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private async void StartSimManager()
        {
            await _simRuntimeManager.InitAsync(_simulatorId, _accessKey, _org, _user, _environemnt);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/realtime");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
            
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            _simRuntimeManager.Publisher = new NotificationPublisher(app.ApplicationServices);

            StartSimManager();
        }
    }
}
