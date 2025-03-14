namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Sucursal
    {
        public int SucursalId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }


        public Usuario UsuarioCreaNavigation { get; set; }
        public Usuario UsuarioActualizadorNavigation { get; set; }

        public ICollection<SucursalColaborador> SucursalColaboradores { get; set; } = new List<SucursalColaborador>();
        public ICollection<Viaje> Viajes { get; set; } = new List<Viaje>();
        public ICollection<SolicitudViaje> SolicitudeViajes { get; set; } = new List<SolicitudViaje>();


        public Sucursal()
        { 
            Nombre = string.Empty;
            Direccion = string.Empty;
            Telefono  = string.Empty;
        }
    }
}
