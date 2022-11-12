namespace CourseWork.DistributionAPI.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Фильтр исключений.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        /// <inheritdoc/>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is OutOfMemoryException)
            {
                context.Result = new BadRequestObjectResult(context.Exception)
                {
                    Value = "Недостаточно оперативной памяти на распределительном сервере для таких данных!",
                    StatusCode = 500,
                };
            }
            else
            {
                string exceptionMessage = context.Exception.Message;
                context.Result = new BadRequestObjectResult(context.Exception)
                {
                    Value = exceptionMessage,
                    StatusCode = 500,
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
