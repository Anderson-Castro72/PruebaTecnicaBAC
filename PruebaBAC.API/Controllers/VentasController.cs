using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaBAC.API.Models;
using System.Data;

namespace PruebaBAC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly PruebaTecnicaBacContext _context;

        public VentasController(PruebaTecnicaBacContext context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaDTO ventaDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var idVentaParam = new SqlParameter("@IdVenta", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_RegistrarVenta @Vendedor, @Total, @IdVenta OUT",
                        new SqlParameter("@Vendedor", ventaDto.Vendedor),
                        new SqlParameter("@Total", ventaDto.Total),
                        idVentaParam
                    );

                    int idVentaGenerado = (int)idVentaParam.Value;

                    // Registrar cada Detalle
                    foreach (var d in ventaDto.Detalles)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "EXEC sp_RegistrarDetalle {0}, {1}, {2}, {3}, {4}, {5}",
                            idVentaGenerado,
                            d.IdPro,
                            d.Cantidad,
                            d.PrecioUnitario,
                            d.Iva,
                            d.TotalLinea
                        );
                    }

                    transaction.Commit();
                    return Ok(new { mensaje = "Venta registrada con éxito", idVenta = idVentaGenerado });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest("Error al registrar venta: " + ex.Message);
                }
            }
        }
        // GET: api/Ventas/reporte
        [HttpGet("reporte")]
        public async Task<ActionResult<IEnumerable<ReporteVentaDTO>>> GetReporte()
        {
            try
            {
                var reporte = await _context.ReporteVentas
                    .FromSqlRaw("EXEC sp_ObtenerReporteVentas")
                    .ToListAsync();

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al generar reporte: " + ex.Message);
            }
        }
    }
}
