using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBAC.API.Models;

namespace PruebaBAC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly PruebaTecnicaBacContext _context;

        public UsuariosController(PruebaTecnicaBacContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            try
            {
                var resultado = await _context.UsuariosSesion
                    .FromSqlRaw("EXEC sp_ValidarUsuario {0}, {1}", login.Usuario, login.Password)
                    .ToListAsync();

                var usuario = resultado.FirstOrDefault();

                if (usuario == null)
                {
                    return Unauthorized("Usuario o contraseña incorrectos");
                }

                return Ok(new
                {
                    Id = usuario.IdUsuario,
                    Nombre = usuario.NombreCompleto,
                    Rol = usuario.Rol
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Error en el servidor: " + ex.Message);
            }
        }
    }
}