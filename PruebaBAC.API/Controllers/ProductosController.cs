using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBAC.API.Models;

namespace PruebaBAC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly PruebaTecnicaBacContext _context;

        public ProductosController(PruebaTecnicaBacContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos
                .FromSqlRaw("EXEC sp_ListarProductos")
                .ToListAsync();
        }

        // 2. BUSCAR POR CODIGO 
        [HttpGet("buscar/{codigo}")]
        public async Task<ActionResult<Producto>> GetProductoPorCodigo(string codigo)
        {
            var resultado = await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductoPorCodigo {0}", codigo)
                .ToListAsync();

            var producto = resultado.FirstOrDefault();

            if (producto == null)
            {
                return NotFound("Producto no encontrado");
            }

            return Ok(producto);
        }

        // 3. GUARDAR
        [HttpPost]
        public async Task<ActionResult> PostProducto(Producto producto)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_GuardarProducto {0}, {1}, {2}",
                producto.Codigo,
                producto.Producto1,
                producto.Precio
            );
            return Ok(new { mensaje = "Guardado correctamente" });
        }

        // 4. EDITAR
        [HttpPut]
        public async Task<IActionResult> PutProducto(Producto producto)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_EditarProducto {0}, {1}, {2}, {3}",
                producto.IdPro,
                producto.Codigo,
                producto.Producto1,
                producto.Precio
            );
            return Ok(new { mensaje = "Actualizado correctamente" });
        }

        // 5. ELIMINAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_EliminarProducto {0}", id);
            return Ok(new { mensaje = "Eliminado correctamente" });
        }
    }
}