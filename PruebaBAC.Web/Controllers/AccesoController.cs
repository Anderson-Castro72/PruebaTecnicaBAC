using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaBAC.Web.Models;
using System.Text;

namespace PruebaBAC.Web.Controllers
{
    public class AccesoController : Controller
    {
        private readonly string _rutaApi = "http://localhost:5058/api/Usuarios/login";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            using (var cliente = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var respuesta = await cliente.PostAsync(_rutaApi, content);

                if (respuesta.IsSuccessStatusCode)
                {
                    var respuestaJson = await respuesta.Content.ReadAsStringAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña incorrectos";
                    return View(modelo);
                }
            }
        }
    }
}