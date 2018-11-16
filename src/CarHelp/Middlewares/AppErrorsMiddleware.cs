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
            catch (AppLayerException ex)
            {

                int statusCode = (int)HttpStatusCode.InternalServerError;
                string body = "";

                if (ex is BadInputException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    body = ex.Message;
                }

                if (ex is AccessRefusedException)
                {
                    statusCode = (int)HttpStatusCode.Forbidden;
                    body = ex.Message;
                }

                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = statusCode;

                // TODO: add custom codes for different types of errrors. For example, 900 for "user doesn't exist", 901 for "validation errors"
                await context.Response.WriteAsync(body);
            }
        }
    }
}
