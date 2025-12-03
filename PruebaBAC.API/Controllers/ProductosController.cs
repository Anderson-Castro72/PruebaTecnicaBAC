using Microsoft.AspNetCore.Mvc;

namespace PruebaBAC.API.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
