namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje.Dto
{
    public class SolicitudViajeDto
    {
        public int SolicitudId { get; set; }
        public int ColaboradorId { get; set; }
        public int SucursalId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Comentario { get; set; }
    }
}
