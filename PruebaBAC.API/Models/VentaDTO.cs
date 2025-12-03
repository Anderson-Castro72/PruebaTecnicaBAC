namespace PruebaBAC.API.Models
{
    public class VentaDTO
    {
        public string Vendedor { get; set; }
        public decimal Total { get; set; }

        public List<DetalleDTO> Detalles { get; set; }
    }

    public class DetalleDTO
    {
        public int IdPro { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Iva { get; set; }
        public decimal TotalLinea { get; set; }
    }
}