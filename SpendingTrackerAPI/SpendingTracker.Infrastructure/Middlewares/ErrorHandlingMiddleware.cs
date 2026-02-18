using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SpendingTracker.Domain.Exeptions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Infrastructure.Middlewares
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
            catch (RegisterExeption rex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(rex.Message);
            }
            catch(GetDataExeption gex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(gex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Coś poszło nie tak.");
            }
        }
    }
}
