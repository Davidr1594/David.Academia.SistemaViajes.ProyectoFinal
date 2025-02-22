namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }


        public ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public Rol()
        {
            Nombre = string.Empty;
        }
    }

}
