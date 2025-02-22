namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Viaje
    {
        public int ViajeId { get; set; }
        public DateTime FechaViaje { get; set; }
        public int SucursalId { get; set; }
        public int TransportistaId { get; set; }
        public decimal TotalKms { get; set; }
        public decimal MontoTotal { get; set; }
        public int MonedaId { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public int EstadoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime? FechaActualiza { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }

        public Sucursal SucursalNavigation { get; set; }
        public Transportista TransportistaNavigation { get; set; }
        public Usuario UsuarioCreadorNavigation { get; set; }
        public Usuario UsuarioActualizadorNavigation { get; set; }
        public EstadoViaje EstadoNavigation { get; set; }
        public Moneda MonedaNavigation { get; set; }


        public ICollection<ViajeDetalle> ViajeDetalles { get; set; } = new List<ViajeDetalle>();
        public ICollection<PagoViaje> PagosViajes { get; set; } = new List<PagoViaje>();

        public Viaje()
        {  
             
        }
    }
}
