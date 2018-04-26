using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http.Cors;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

//using Microsoft.AspNetCore.Identity;

namespace RagAppGuideApi
{
    //
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("MIGEAuthorize", policy => policy.RequireClaim("jti"));
            //});

            // Add framework services.
            services.AddMvc();

            //var formatterSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
            //formatterSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //JsonOutputFormatter formatter = new JsonOutputFormatter(formatterSettings, ArrayPool<Char>.Shared);

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.OutputFormatters.RemoveType<JsonOutputFormatter>();
            //    options.OutputFormatters.Insert(0, formatter);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //comment out for production (used for debugging)
            app.UseDeveloperExceptionPage();//<-- debugging on server
            //if (env.IsDevelopment()) {
            //    app.UseDeveloperExceptionPage();//<-- debugging on server 
            //}


            // Shows UseCors with named policy.
            //app.UseCors("MyPolicy");

            //app.UseCors(builder =>
            //   builder.WithOrigins("http://dev-net-brn.mig.local:8081")
            //   .AllowAnyHeader()
            //   );


            app.UseMvc();
        }
    }
}
