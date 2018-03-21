using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHelp.BLL.Services;
using CarHelp.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace CarHelp
{
    
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(L2DBRepository<>));

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();

                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                //{
                //    In = "header",
                //    Description = "Please insert JWT with Bearer into field",
                //    Name = "Authorization",
                //    Type = "apiKey"
                //});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseMvc();
        }
    }
}
