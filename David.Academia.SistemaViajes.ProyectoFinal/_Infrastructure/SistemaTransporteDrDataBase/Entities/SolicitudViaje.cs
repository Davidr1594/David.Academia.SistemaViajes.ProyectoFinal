using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class SolicitudViaje
    {
        public int SolicitudId { get; set; }
        public int ColaboradorId { get; set; }
        public int SucursalId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Comentario { get; set; }
        public int EstadoId { get; set; }
        public int? UsuarioAprueba { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualiza { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }

        public Colaborador ColaboradorNavigation { get; set; }
        public Sucursal SucursalNavigation { get; set; }
        public Usuario? UsuarioAprobadorNavigation { get; set; }
        public Usuario? UsuarioActualizadorNavigation { get; set; }
        public EstadoViaje EstadoViajeNavigation { get; set; }

        public SolicitudViaje()
        { 
            Comentario = string.Empty;

        }
    }
}
