using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaBAC.Web.Models;
using System.Text;

namespace PruebaBAC.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly HttpClient _httpClient;
        // Confirma que este sea tu puerto correcto (el mismo que usas en Productos)
        private readonly string _urlApi = "http://localhost:5058/api";

        public VentasController()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);
        }

        // 1. PANTALLA PRINCIPAL DE VENTAS (El POS)
        public IActionResult Index()
        {
            return View();
        }

        // 2. BUSCAR PRODUCTO (Para el JavaScript de la vista)
        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string codigo)
        {
            try
            {
                var respuesta = await _httpClient.GetAsync($"{_urlApi}/Productos/buscar/{codigo}");
                if (respuesta.IsSuccessStatusCode)
                {
                    var json = await respuesta.Content.ReadAsStringAsync();
                    var producto = JsonConvert.DeserializeObject<ProductoViewModel>(json);
                    return Json(producto);
                }
            }
            catch { }
            return Json(null);
        }

        // 3. REGISTRAR VENTA (Envía los datos a la API)
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] VentaViewModel venta)
        {
            try
            {
    
                venta.Vendedor = "Juan Perez";

                var json = JsonConvert.SerializeObject(venta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var respuesta = await _httpClient.PostAsync($"{_urlApi}/Ventas/registrar", content);

                if (respuesta.IsSuccessStatusCode)
                {
                    return Json(new { exito = true, mensaje = "Venta registrada con éxito" });
                }
                else
                {
                    return Json(new { exito = false, mensaje = "Error en la API: " + respuesta.StatusCode });
                }
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error de conexión: " + ex.Message });
            }
        }
    }
}