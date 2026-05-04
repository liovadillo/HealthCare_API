using HealthCare_API.Exceptions;

namespace HealthCare_API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next) { 
        
            _next = next;
        
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (BaseException ex) {

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = ex.StatusCode,
                    message = ex.Message
                });

            }
            catch(Exception ex) { 
            
            }


        }
    }
}
