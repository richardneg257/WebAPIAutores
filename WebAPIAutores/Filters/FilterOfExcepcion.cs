using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filters
{
    public class FilterOfExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FilterOfExcepcion> logger;

        public FilterOfExcepcion(ILogger<FilterOfExcepcion> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}
