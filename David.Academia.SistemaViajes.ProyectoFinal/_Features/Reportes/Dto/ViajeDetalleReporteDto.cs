namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes.Dto
{
    public class ViajeDetalleReporteDto
    {
        public int ViajeDetalleId { get; set; }
        public int ViajeId { get; set; }
        public string Sucursal { get; set; }
        public string NombreColaborador { get; set; }
        public string DireccionDestino { get; set; }
        public decimal Kms { get; set; }
        public DateTime FechaViaje { get; set; }

    }
}
