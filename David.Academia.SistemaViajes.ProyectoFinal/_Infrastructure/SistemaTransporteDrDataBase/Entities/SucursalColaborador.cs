namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class SucursalColaborador
    {
        public int SucursalId { get; set; }
        public int ColaboradorId { get; set; }
        public decimal DistanciaKm { get; set; }

        public Sucursal SucursalNavigation { get; set; }
        public Colaborador ColaboradorNavigation { get; set; }

        public SucursalColaborador()
        {

        }
    }
}
