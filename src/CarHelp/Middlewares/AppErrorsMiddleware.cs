using CarHelp.AppLayer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CarHelp.Middlewares
{
    public class AppErrorsMiddleware
    {
        private readonly RequestDelegate _next;

        public AppErrorsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (AppException ex)
            {
                if (ex is BadInputException)
                {
                    // TODO: add custom codes for different types of errrors. For example, x for "user doesn't exist", y for "validation errors"
                    context.Response.ContentType = "text/plain";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    await context.Response.WriteAsync(ex.Message);
                }
            }
        }
    }
}
