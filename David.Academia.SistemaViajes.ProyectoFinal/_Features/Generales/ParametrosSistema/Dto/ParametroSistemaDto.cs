namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto
{
    public class ParametroSistemaDto
    {
        public int RegistroId { get; set; }
        public string Descripcion { get; set; }
        public int? Valor { get; set; }
        public string ValorString { get; set; }
        public bool Activo { get; set; }
    }
}
