namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class ViajeDetalle
    {
        public int ViajeDetalleId { get; set; }
        public int ViajeId { get; set; }
        public int ColaboradorId { get; set; }
        public string DireccionDestino { get; set; }
        public decimal Kms { get; set; }
        public decimal Costo { get; set; }

        public Viaje Viaje { get; set; }
        public Colaborador Colaborador { get; set; }

        public ViajeDetalle()
        { 
            DireccionDestino = string.Empty;
            Viaje = new Viaje();
            Colaborador = new Colaborador();
        }
    }
}
