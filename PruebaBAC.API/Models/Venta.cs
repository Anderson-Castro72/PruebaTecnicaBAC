using System;
using System.Collections.Generic;

namespace PruebaBAC.API.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateTime? Fecha { get; set; }

    public string Vendedor { get; set; } = null!;

    public decimal Total { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();
}
