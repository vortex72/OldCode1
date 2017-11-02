using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace EPWI.ShipExecInterface
{
    

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Application startup method
        /// </summary>
        /// <param name="env"></param>
        //============================================================
        //Revision History
        //Date        Author          Description
        //02/27/2017  TB               Created
        //============================================================
        public Startup(IHostingEnvironment env)
        {
            //load configuration settings
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();

            //build app settings object
            services.Configure<AppSettings>(x => {
                x.ShipExecShipURL = Configuration.GetSection("AppSettings")["ShipExecShipURL"];
                x.ConnectionString = Configuration.GetConnectionString("AS400");
                x.SQLConnectionString = Configuration.GetConnectionString("logging");
                x.LogAllRequests = Convert.ToBoolean(Configuration.GetSection("AppSettings")["LogAllRequests"]);
            });

            services.AddTransient<ISQLLogger, SQLLogger>();

            //add filter to capture exceptions
            services.AddScoped<AppExceptionFilterAttribute>();

            //add transient interfaces
            services.AddTransient<IDataInterface, DataInterface>();
            services.AddTransient<IShipExecInterface,ShipExecInterface>();
            services.AddTransient<IDataFactory, DataFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug();
            }

            app.UseMvc();
        }
    }
}
