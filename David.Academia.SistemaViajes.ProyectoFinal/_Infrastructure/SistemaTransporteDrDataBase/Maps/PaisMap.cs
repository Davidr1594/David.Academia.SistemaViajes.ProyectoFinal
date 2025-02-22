using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PaisMap : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ToTable("Paises");
            builder.HasKey(x => x.PaisId);
            builder.Property(x => x.PaisId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Prefijo).HasMaxLength(5).IsRequired();
            builder.Property(x => x.MonedaId).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

            builder.HasOne(p => p.MonedaNavigation)
                    .WithOne()
                    .HasForeignKey<Pais>(p => p.MonedaId);


        }
    }
}
