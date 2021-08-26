using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeWorkConstraint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeWorkConstraint", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var routeHandler = new RouteHandler(async context =>
            {
                var routeDataValues = context.GetRouteData().Values;
                var serialize = JsonSerializer.Serialize(routeDataValues);
                await context.Response.WriteAsync(serialize);
            });

            var routeBuilder = new RouteBuilder(app, routeHandler);

            //routeBuilder.MapRoute("DefaultRoute", "{controller}/{action?}/{id?}");

            routeBuilder.MapRoute("DefaultRoute", "{controller}/{action}/{id?}",
                new { controller = "Home", action = "Index" }
            );

            //routebuilder.maproute("defaultroute", "{controller:minlength(7)}/{action}/{id:int?}",
            //    new { controller = "home", action = "index" }
            //);

            var router = routeBuilder.Build();
            app.UseRouter(router);

        }
    }
}
