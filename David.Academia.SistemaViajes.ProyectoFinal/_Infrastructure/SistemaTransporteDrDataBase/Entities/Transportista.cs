namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Transportista
    {
        public int TransportistaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public decimal TarifaPorKm { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime? FechaActualiza { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }

        public Usuario UsuarioCreaNavigation { get; set; }
        public Usuario UsuarioActualizaNavigation { get; set; }


        public ICollection<Viaje> Viajes { get; set; } = new List<Viaje>(); 
        public ICollection<ValoracionViaje> ValoracionesViajes { get; set; } = new List<ValoracionViaje>();
        public ICollection<SolicitudViaje> SolicitudeViajes { get; set; } = new List<SolicitudViaje>();   
        public ICollection<PagoContratista> PagosContratistas { get; set; } = new List<PagoContratista>();

        public Transportista()
        { 
            Nombre = string.Empty;
            Apellido = string.Empty;
            Dni = string.Empty;
            Telefono = string.Empty;
            Email = string.Empty;
        }
    }
}
