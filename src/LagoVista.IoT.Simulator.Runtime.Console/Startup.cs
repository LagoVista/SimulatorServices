using LagoVista.IoT.Simulator.Runtime.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LagoVista.IoT.Simulator.Runtime
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddMvc((options) =>
            {

            })

               .AddApplicationPart(typeof(SimulatorController).GetTypeInfo().Assembly);
        }

//        ILoggerFactory _loggerFactory;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

        }
    }
}
