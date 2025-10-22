using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;

namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheServie = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var casheKey = GenerateCasheKey(context.HttpContext.Request);

            var result = await cacheServie.GetCacheValueAsync(casheKey);

            if (!string.IsNullOrEmpty(result))
            {
                context.Result = new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var contextResult = await next.Invoke();

            if (contextResult.Result is OkObjectResult okObject)
            {
                await cacheServie.SetCacheValueAsync(casheKey, okObject.Value, TimeSpan.FromSeconds(durationInSec));
            }
        }

        private string GenerateCasheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append($"{request.Path}");

            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }

            return key.ToString();
        }
    }
}
