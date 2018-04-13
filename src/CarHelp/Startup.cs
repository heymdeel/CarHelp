using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Services;
using CarHelp.DAL;
using CarHelp.DAL.Repositories;
using CarHelp.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
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
            services.Configure<ConnectionOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AuthOptions>(Configuration.GetSection("Authentication").GetSection("JWTBearer"));

            // Repositories
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IWorkersRepository, WorkersRepository>();
            services.AddTransient<IOrdersRepository, OrdersRepository>();

            // Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IWorkersService, WorkersService>();
            services.AddTransient<IOrdersService, OrdersService>();

            // Authhorization
            services.AddTokenAuthorization(Configuration);

            services.AddMvc();
            services.AddAutoMapper();
            
            // Swagger
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Info { Title = "CarHelp API", Version = "v1" });
                swagger.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "CarHelp.xml"));

                swagger.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
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

                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
