using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpendingsManagementWebAPI.Exeptions;
using SpendingsManagementWebAPI.Exeptions;
using System;
using System.Threading.Tasks;

namespace SpendingsManagementWebAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }

            catch (BadRequestException bRex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(bRex.Message);
            }
            catch (NotFoundException nTex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nTex.Message);
            }
            catch(RegisterExeption rex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(rex.Message);
            }
            catch(EditExeption eex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(eex.Message);
            }
            catch(WrongDataException eDex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(eDex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong.");
            }
        }
    }
}
