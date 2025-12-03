using System;
using System.Collections.Generic;

namespace PruebaBAC.API.Models;

public partial class DetalleVenta
{
    public int IdDe { get; set; }

    public int IdVenta { get; set; }

    public int IdPro { get; set; }

    public DateTime? Fecha { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public virtual Producto IdProNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
