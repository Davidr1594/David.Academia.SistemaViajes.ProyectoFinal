namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class EstadoPago
    {
        public int EstadoPagoId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public ICollection<PagoContratista> PagosContratistas { get; set; } = new List<PagoContratista>();


        public EstadoPago()
        { 
            Nombre = string.Empty;
        }
    }
}
