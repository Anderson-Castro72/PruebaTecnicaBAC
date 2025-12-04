using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaBAC.Web.Models;

namespace PruebaBAC.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseApi = "http://localhost:5058/api/Productos";

        public ProductosController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            List<ProductoViewModel> lista = new List<ProductoViewModel>();

            var respuesta = await _httpClient.GetAsync(_baseApi);

            if (respuesta.IsSuccessStatusCode)
            {
                var json_respuesta = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<ProductoViewModel>>(json_respuesta);
            }

            return View(lista);
        }
    }
}