using CarHelp.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarHelp
{
    internal static class Extensions
    {
        public static IServiceCollection AddTokenAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = configuration.GetSection("Authentication").GetSection("JWTBearer").Get<AuthOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authOptions.Issuer,

                            ValidateAudience = true,
                            ValidAudience = authOptions.AccessAudience,

                            ValidateLifetime = true,

                            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            return services;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
        }
    }
}
