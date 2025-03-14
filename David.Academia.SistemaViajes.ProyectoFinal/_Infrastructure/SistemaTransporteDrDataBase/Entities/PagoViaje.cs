using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class PagoViaje
    {
        public int PagoId { get; set; }
        public int ViajeId { get; set; }
        public decimal MontoPagado { get; set; }

        public  PagoContratista PagoContratista { get; set; }
        public  Viaje Viaje { get; set; }

        public PagoViaje()
        {

        }
    }
}
