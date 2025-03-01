namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes.Dto
{
    public class ReporteViajeDto
    {
        public int ViajeId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string SucursalNombre { get; set; }
        public string TransportistaNombre { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public decimal TotalKms { get; set; }
        public decimal MontoTotal { get; set; }
        public string Moneda { get; set; }
    }
}
