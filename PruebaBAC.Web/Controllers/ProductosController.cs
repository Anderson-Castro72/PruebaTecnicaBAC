using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaBAC.Web.Models;
using System.Text;
using PruebaBAC.Web.Filtros;

namespace PruebaBAC.Web.Controllers
{
    [ValidarRol("Administrador")]
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var json = JsonConvert.SerializeObject(modelo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respuesta = await _httpClient.PostAsync(_baseApi, content);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error al crear producto";
                return View(modelo);
            }
        }

        // 3. EDITAR - VISTA (GET)
        // Buscamos por código para llenar el formulario
        public async Task<IActionResult> Edit(string codigo)
        {
            var respuesta = await _httpClient.GetAsync($"{_baseApi}/buscar/{codigo}");

            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(json);
                return View(producto);
            }

            return RedirectToAction("Index");
        }

        // 3. EDITAR - ACCIÓN (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(ProductoViewModel modelo)
        {
            var json = JsonConvert.SerializeObject(modelo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respuesta = await _httpClient.PutAsync(_baseApi, content);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Error al editar";
                return View(modelo);
            }
        }

        // 4. ELIMINAR (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var respuesta = await _httpClient.DeleteAsync($"{_baseApi}/{id}");
            return RedirectToAction("Index");
        }

    }
}