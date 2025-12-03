using System;
using System.Collections.Generic;

namespace PruebaBAC.API.Models;

public partial class Producto
{
    public int IdPro { get; set; }

    public string Codigo { get; set; } = null!;

    public string Producto1 { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
