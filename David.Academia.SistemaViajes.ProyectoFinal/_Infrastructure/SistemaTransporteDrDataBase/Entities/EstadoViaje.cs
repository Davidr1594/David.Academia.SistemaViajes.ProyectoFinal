namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class EstadoViaje
    {
        public int EstadoViajeId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();

        public EstadoViaje()
        { 
            Nombre = string.Empty;
        }

    }
}
