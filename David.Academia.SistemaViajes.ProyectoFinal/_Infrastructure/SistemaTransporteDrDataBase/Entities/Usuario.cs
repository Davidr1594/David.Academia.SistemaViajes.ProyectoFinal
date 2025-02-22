namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public byte[] ClaveHash { get; set; }
        public int RolId { get; set; }
        public int? ColaboradorId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }



        public Rol RolNavigation { get; set; }
        public Usuario? UsuarioCreaNavigation { get; set; }
        public Usuario? UsuarioActualizaNavigation { get; set; }
        public Colaborador? ColaboradorNavigation { get; set; }



        public ICollection<Colaborador> ColaboradoresCrear { get; set; } = new List<Colaborador>();
        public ICollection<Colaborador> ColaboradoresActualizar { get; set; } = new List<Colaborador>();
     
        public ICollection<Sucursal> SucursalesCrea { get; set; } = new List<Sucursal>(); 
        public ICollection<Sucursal> SucursalesActualiza { get; set; } = new List<Sucursal>();  

        public ICollection<Viaje> ViajesCrea { get; set; } = new List<Viaje>(); 
        public ICollection<Viaje> ViajesActualiza { get; set; } = new List<Viaje>(); 

        public ICollection<SolicitudViaje> SolicitudeViajesCrea { get; set; } = new List<SolicitudViaje>(); 
        public ICollection<SolicitudViaje> SolicitudeViajesActualiza { get; set; } = new List<SolicitudViaje>(); 

        public ICollection<ParametroSistema> ParametrosSistemasCrea { get; set; } = new List<ParametroSistema>(); 
        public ICollection<ParametroSistema> ParametrosSistemasActualiza { get; set; } = new List<ParametroSistema>(); 

        public ICollection<PagoContratista> PagosContratistas{ get; set; } = new List<PagoContratista>(); 

        public ICollection<Usuario> UsuariosCrea { get; set; } = new List<Usuario>();
        public ICollection<Usuario> UsuariosActualiza { get; set; } = new List<Usuario>();

        public ICollection<Transportista> TransportistasCrea { get; set; } = new List<Transportista>();
        public ICollection<Transportista> TransportistasActualiza { get; set; } = new List<Transportista>();


        public Usuario()
        { 
            Nombre = string.Empty;
            Email = string.Empty;
            ClaveHash = new byte[16];
        }
    }

}
