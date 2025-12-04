using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaBAC.Web.Models;
using System.Text;
using ClosedXML.Excel;

namespace PruebaBAC.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlApi = "http://localhost:5058/api";

        public VentasController()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _httpClient = new HttpClient(handler);
        }

        // 1. PANTALLA PRINCIPAL: LISTADO DE VENTAS 
        public async Task<IActionResult> Index()
        {
            List<ReporteVentaViewModel> lista = new List<ReporteVentaViewModel>();
            try
            {
                var respuesta = await _httpClient.GetAsync($"{_urlApi}/Ventas/reporte");

                if (respuesta.IsSuccessStatusCode)
                {
                    var json = await respuesta.Content.ReadAsStringAsync();
                    var datos = JsonConvert.DeserializeObject<List<ReporteVentaViewModel>>(json);
                    if (datos != null) lista = datos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(lista);
        }

        // 2. PANTALLA DE REGISTRO
        public IActionResult Create()
        {
            return View();
        }

        // 3. ACCIÓN: BUSCAR UN PRODUCTO
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

        // 4. ACCIÓN: REGISTRAR LA VENTA 
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] VentaViewModel venta)
        {
            try
            {
                venta.Vendedor = "Juan Martinez"; 

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

        // 5. ACCIÓN: EXPORTAR EXCEL
        public async Task<IActionResult> ExportarExcel()
        {
            List<ReporteVentaViewModel> lista = new List<ReporteVentaViewModel>();
            var respuesta = await _httpClient.GetAsync($"{_urlApi}/Ventas/reporte");

            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<ReporteVentaViewModel>>(json);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Ventas");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Ticket #";
                worksheet.Cell(1, 2).Value = "Fecha";
                worksheet.Cell(1, 3).Value = "Vendedor";
                worksheet.Cell(1, 4).Value = "Total";

                // Estilos
                var rango = worksheet.Range("A1:D1");
                rango.Style.Fill.BackgroundColor = XLColor.Red;
                rango.Style.Font.FontColor = XLColor.White;
                rango.Style.Font.Bold = true;

                // Datos
                int fila = 2;
                foreach (var v in lista)
                {
                    worksheet.Cell(fila, 1).Value = v.IdVenta;
                    worksheet.Cell(fila, 2).Value = v.Fecha;
                    worksheet.Cell(fila, 3).Value = v.Vendedor;
                    worksheet.Cell(fila, 4).Value = v.Total;
                    worksheet.Cell(fila, 4).Style.NumberFormat.Format = "$ #,##0.00";
                    fila++;
                }
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentas.xlsx");
                }
            }
        }
        public async Task<IActionResult> ReportePDF()
        {
            List<ReporteVentaViewModel> lista = new List<ReporteVentaViewModel>();

            var respuesta = await _httpClient.GetAsync($"{_urlApi}/Ventas/reporte");

            if (respuesta.IsSuccessStatusCode)
            {
                var json = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<ReporteVentaViewModel>>(json);
            }

            return View(lista);
        }
    }
}