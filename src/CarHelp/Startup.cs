using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer;
using CarHelp.AppLayer.Models;
using CarHelp.AppLayer.Services;
using CarHelp.DAL;
using CarHelp.DAL.Repositories;
using CarHelp.Middlewares;
using CarHelp.Options;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CarHelp
{   
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddOptions();
           
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AuthOptions>(Configuration.GetSection("Authentication").GetSection("JWTBearer"));

            // Repositories
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IOrdersRepository, OrdersRepository>();

            // Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddScoped<ISmsService, SmsService>();

            // Authhorization
            services.AddTokenAuthorization(Configuration);

            services.AddMvc();
            
            // TODO: replace this with instance API
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(typeof(MappingProfileVM));
                cfg.AddProfile(typeof(MappingProfileAppLayer));
            });

            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "CarHelp API", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CarHelp.xml"));

                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarHelp API V1");
                });
            }

            app.UseAuthentication();

            app.UseMiddleware<AppErrorsMiddleware>();

            app.UseMvc();
        }
    }
}
