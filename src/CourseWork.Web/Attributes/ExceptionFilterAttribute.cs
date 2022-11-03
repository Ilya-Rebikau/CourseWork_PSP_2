namespace CourseWork.Web.Attributes
{
    using CourseWork.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    /// <summary>
    /// Фильтр исключений.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        /// <inheritdoc/>
        public void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            var content = exceptionType.GetProperty("Content");
            string exceptionMessage;
            if (content != null)
            {
                exceptionMessage = content.GetValue(context.Exception).ToString();
            }
            else
            {
                exceptionMessage = context.Exception.Message;
            }

            if (context.Exception is HttpRequestException)
            {
                exceptionMessage = "Распределительный сервер отключен";
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                Model = new ErrorViewModel { Message = exceptionMessage },
            };
            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = viewData,
            };

            context.ExceptionHandled = true;
        }
    }
}
