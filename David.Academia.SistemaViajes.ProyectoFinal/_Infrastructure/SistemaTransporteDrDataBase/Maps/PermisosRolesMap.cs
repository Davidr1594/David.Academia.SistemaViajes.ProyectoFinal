using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PermisosRolesMap : IEntityTypeConfiguration<PermisoRol>
    {
        public void Configure(EntityTypeBuilder<PermisoRol> builder)
        {
            builder.ToTable("PermisosRoles");
            builder.HasKey(x => new { x.RolId, x.PermisoId });

            builder.HasOne(pr => pr.Rol)
                .WithMany(r => r.PermisoRoles)
                .HasForeignKey(pr => pr.RolId);

            builder.HasOne(pr => pr.Permiso)
                .WithMany(p => p.PermisoRoles)
                .HasForeignKey(pr => pr.PermisoId);

        }
    }
}
