using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class PagoContratista
    {
        public int PagoId { get; set; }
        public int TransportistaId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoTotal { get; set; }
        public int EstadoPagoId { get; set; }
        public int? MetodoPagoId { get; set; }
        public string Comentario { get; set; }
        public int UsuarioRegistraId { get; set; }

        public  Transportista TransportistaNavigation { get; set; }
        public  EstadoPago? EstadoPagoNavigation { get; set; }
        public  MetodoPago? MetodoPagoNavigation { get; set; }
        public  Usuario UsuarioRegistraNavigation { get; set; }

        public ICollection<PagoViaje> PagosViajes { get; set; } = new List<PagoViaje>();

        public PagoContratista()
        { 
            Comentario = string.Empty;


        }
    }
}
