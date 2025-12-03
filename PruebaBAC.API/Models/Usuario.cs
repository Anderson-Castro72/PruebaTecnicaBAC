using System;
using System.Collections.Generic;

namespace PruebaBAC.API.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string? Rol { get; set; }
}
