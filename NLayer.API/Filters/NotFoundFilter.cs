using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    public class NotFoundFilter<T,Dto> : IAsyncActionFilter where T : BaseEntity where Dto : class
    {
        private readonly IService<T,Dto> _service;

        public NotFoundFilter(IService<T,Dto> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if (idValue is null)
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue;
            //check whether there is an id
            var anyEntity = await _service.AnyAsync(x => x.Id == id);

            if (anyEntity.Data)
            {
                await next.Invoke();
                return;
            }

            context.Result =
                new NotFoundObjectResult(
                    CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));
        }
    }
}