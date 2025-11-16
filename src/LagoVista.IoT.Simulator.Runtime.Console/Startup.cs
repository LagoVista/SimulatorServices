// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ef5f68e6db9168afbe5921aa7d525fafd00762a95285460c2c91312974821517
// IndexVersion: 2
// --- END CODE INDEX META ---
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
