using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaBAC.API.Models;
using PruebaBAC.API.Utilidades;

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
                string passwordEncriptada = Encriptador.ConvertirSHA256(login.Password);

                var resultado = await _context.UsuariosSesion
                    .FromSqlRaw("EXEC sp_ValidarUsuario {0}, {1}", login.Usuario, passwordEncriptada)
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
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioDTO modelo)
        {
            try
            {
                string passwordEncriptada = Encriptador.ConvertirSHA256(modelo.Password);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_RegistrarUsuario {0}, {1}, {2}, {3}",
                    modelo.NombreUsuario,
                    passwordEncriptada, 
                    modelo.NombreCompleto,
                    modelo.Rol
                );

                return Ok(new { mensaje = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {

                return BadRequest("Error al registrar: " + ex.Message);
            }
        }   
    }
}