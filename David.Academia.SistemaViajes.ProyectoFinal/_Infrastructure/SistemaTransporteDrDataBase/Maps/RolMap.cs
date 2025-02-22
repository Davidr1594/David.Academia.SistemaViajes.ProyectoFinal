using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.RolId);
            builder.Property(x => x.RolId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(60).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

        }
    }
}
