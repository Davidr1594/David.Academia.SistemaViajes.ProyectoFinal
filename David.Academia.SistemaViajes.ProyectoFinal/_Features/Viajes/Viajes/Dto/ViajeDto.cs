namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto
{
    public class ViajeDto
    {
        public int ViajeId { get; set; }
        public DateTime FechaViaje { get; set; }
        public int SucursalId { get; set; }
        public string NombreSucursal { get; set; }
        public int TransportistaId { get; set; }
        public string NombreTransportista { get; set; }

        public decimal TotalKms { get; set; }
        public decimal MontoTotal { get; set; }
        public int MonedaId { get; set; }
        public string NombreMoneda { get; set; }

        public TimeSpan HoraSalida { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }

        public bool Activo { get; set; }
        public List<int> ColaboradoresId { get; set; }

    }
}
