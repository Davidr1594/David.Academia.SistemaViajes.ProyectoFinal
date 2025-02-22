namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class ParametroSistema
    {
        public int RegistroId { get; set; }
        public string Descripcion { get; set; }
        public int? Valor { get; set; }
        public string ValorString { get; set; }
        public int UsuarioCrea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualiza { get; set; }
        public int? UsuarioActualiza { get; set; }
        public bool Activo { get; set; }

        public Usuario? UsuarioCreaNavigation { get; set; }
        public Usuario? UsuarioActualizaNavigation { get; set; }

        public ParametroSistema()
        {
            Descripcion = string.Empty;
            ValorString = string.Empty;

        }
    }
}
