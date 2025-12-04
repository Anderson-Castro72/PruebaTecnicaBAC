namespace PruebaBAC.Web.Models
{
    public class VentaViewModel
    {
        public string Vendedor { get; set; }
        public decimal Total { get; set; }
        public List<DetalleViewModel> Detalles { get; set; }
    }

    public class DetalleViewModel
    {
        public int IdPro { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Iva { get; set; }
        public decimal TotalLinea { get; set; }
    }
}