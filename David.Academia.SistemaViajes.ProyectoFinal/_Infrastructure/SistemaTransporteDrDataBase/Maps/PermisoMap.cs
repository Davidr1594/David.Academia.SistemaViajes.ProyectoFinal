using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PermisoMap : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("Permisos");
            builder.HasKey(x => x.PermisoId);
            builder.Property(x => x.PermisoId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

        }
    }
}
