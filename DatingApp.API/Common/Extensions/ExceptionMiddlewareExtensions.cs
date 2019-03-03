using System.Net;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Common.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
                // app.UseExceptionHandler(builder =>
                // {
                //     builder.Run(async context =>
                //     {
                //         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //         var error = context.Features.Get<IExceptionHandlerFeature>();
                //         if (error != null)
                //         {
                //             context.Response.AddApplicationError(error.Error.Message);
                //             await context.Response.WriteAsync(error.Error.Message);
                //         }
                //     });
                // });

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please call Admin NOW!!!!",
                            InnerMessage = contextFeature.Error.InnerException.ToString()
                        }.ToString());
                    }
                });
            });
        }
    }
}