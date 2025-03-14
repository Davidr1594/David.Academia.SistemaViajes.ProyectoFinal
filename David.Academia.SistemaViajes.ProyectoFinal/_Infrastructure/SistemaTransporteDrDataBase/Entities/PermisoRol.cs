using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class PermisoRol
    {
        public int RolId { get; set; }
        public int PermisoId { get; set; }

        public Rol Rol { get; set; }
        public Permiso Permiso { get; set; }


        public PermisoRol() 
        {
            Rol = new Rol();
            PermisoId = 0;
            Permiso = new Permiso();
        }
    }
}
