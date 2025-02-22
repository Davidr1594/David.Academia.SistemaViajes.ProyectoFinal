using System.ComponentModel.DataAnnotations.Schema;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Colaborador
    {
        public int ColaboradorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int? UsuarioActualiza { get; set; }
        public int? UsuarioCrea { get; set; }
        public int PuestoId { get; set; }
        public int CiudadId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        public bool Activo { get; set; } = true;



        public Usuario? UsuarioCreaNavigation { get; set; }
        public Usuario? UsuarioActualizaNavigation { get; set; }
        public Puesto? PuestoNavigation { get; set; }
        public Ciudad? CiudadNavigation { get; set; }



        public ICollection<SucursalColaborador> SucursalColaboradores { get; set; } = new List<SucursalColaborador>();
        public ICollection<ViajeDetalle> ViajeDetalles { get; set; } = new List<ViajeDetalle>();
        public ICollection<ValoracionViaje> ValoracionViajes { get; set; } = new List<ValoracionViaje>();
        public ICollection<SolicitudViaje> SolicitudeViajes { get; set; } = new List<SolicitudViaje>();
        public ICollection<Usuario> UsuariosViajes { get; set; } = new List<Usuario>();



        public Colaborador()
        {
            Nombre = string.Empty;
            Apellido = string.Empty;
            Telefono = string.Empty;
            Dni = string.Empty;
            Email = string.Empty;
            Direccion = string.Empty;
            FechaCreacion = DateTime.Now;
            Activo = true;
        }
    }
}
