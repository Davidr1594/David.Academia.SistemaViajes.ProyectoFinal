namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Moneda
    {
        public int MonedaId { get; set; }
        public string Nombre { get; set; }
        public string CodigoIso { get; set; }
        public string Simbolo { get; set; }
        public bool Activo { get; set; }


        public ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();

        public Moneda() 
        {
            Nombre = string.Empty;
            CodigoIso = string.Empty;
            Simbolo = string.Empty;

        }
    }
}
