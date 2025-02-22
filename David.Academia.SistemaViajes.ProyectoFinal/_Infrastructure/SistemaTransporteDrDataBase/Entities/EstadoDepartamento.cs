namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class EstadoDepartamento
    {
        public int EstadoId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public int PaisId { get; set; }

        public Pais? PaisNavigation { get; set; }

        public ICollection<Ciudad> Ciudades { get; set; } = new List<Ciudad>();

        public EstadoDepartamento()
        {
            Nombre = string.Empty;
            Ciudades = new List<Ciudad>();
        }
    }
}
