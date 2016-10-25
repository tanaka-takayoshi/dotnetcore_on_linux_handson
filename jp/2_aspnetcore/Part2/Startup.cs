using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCorehandson
{
    public class Startup
    {
        public static string App_Data { get; set; }

        public Startup(IHostingEnvironment env)
        {
            App_Data = Path.Combine(env.ContentRootPath, "App_Data");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
            app.Run(async (context) =>
            {
                using (PubsEntities pubs = new PubsEntities())
                {
                    var query = pubs.Authors.Where(a => a.State == "CA");
                    await context.Response.WriteAsync(query.Count().ToString());
                }
            });
        }
    }
}
