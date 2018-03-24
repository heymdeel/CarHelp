using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.BLL.Services;
using CarHelp.DAL.Repositories;
using CarHelp.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CarHelp
{   
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISmsService, SmsService>();
            
            services.AddTransient(typeof(IRepository<>), typeof(L2DBRepository<>));

            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IWorkersRepository, WorkersRepository>();

            services.AddTokenAuthorization();

            services.AddMvc();
            services.AddAutoMapper();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CarHelp API", Version = "v1" });

                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
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
