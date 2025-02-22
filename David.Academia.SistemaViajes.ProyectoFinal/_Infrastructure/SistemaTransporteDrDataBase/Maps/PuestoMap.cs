using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PuestoMap : IEntityTypeConfiguration<Puesto>

    {
        public void Configure(EntityTypeBuilder<Puesto> builder)
        {
            builder.ToTable("Puestos");
            builder.HasKey(x => x.PuestoId);
            builder.Property(x => x.PuestoId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Descripcion).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

        }
    }
}
