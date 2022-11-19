namespace CourseWork.Web.Controllers
{
    using CourseWork.Web.Attributes;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    [ExceptionFilter]
    public class HomeController : Controller
    {
        /// <summary>
        /// Начальная страница.
        /// </summary>
        /// <returns>Представление с главной страницей.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}