namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class MetodoPago
    {
        public int MetodoPagoId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public ICollection<PagoContratista> PagosContratistas { get; set; } = new List<PagoContratista>();

        public MetodoPago()
        { 
            Nombre = string.Empty;
        }
    }
}
