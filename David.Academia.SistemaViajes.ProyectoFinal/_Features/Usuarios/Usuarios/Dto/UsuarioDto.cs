namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public int? ColaboradorId { get; set; }
        public bool Activo { get; set; }



        public UsuarioDto()
        {
            Nombre = string.Empty;
            Email = string.Empty;
            Clave = string.Empty;
        }
    }

}
