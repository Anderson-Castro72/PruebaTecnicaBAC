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
            // Usamos una transacción: Si falla un detalle, se deshace toda la venta.
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Registrar Encabezado y obtener ID
                    // Configuramos el parámetro de salida para capturar el ID
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

                    // 2. Registrar cada Detalle
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
    }
}
