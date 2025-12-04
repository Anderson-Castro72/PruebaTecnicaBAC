using System;

namespace PruebaBAC.Web.Models
{
    public class ReporteVentaViewModel
    {

        public int IdVenta { get; set; }

        public DateTime Fecha { get; set; }

        public string Vendedor { get; set; }

        public decimal Total { get; set; }

        public int CantidadItems { get; set; }
    }
}