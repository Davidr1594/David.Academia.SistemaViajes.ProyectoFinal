namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Permiso
    {
        public int PermisoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
        public Permiso()
        { 
            Nombre = string.Empty;
            Descripcion = string.Empty;
        }

    }
}
