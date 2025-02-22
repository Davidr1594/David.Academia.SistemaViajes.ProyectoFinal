using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class MonedaMap : IEntityTypeConfiguration<Moneda>
    {
        public void Configure(EntityTypeBuilder<Moneda> builder)
        {
            builder.ToTable("Monedas");
            builder.HasKey(x => x.MonedaId);
            builder.Property(x => x.MonedaId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CodigoIso).HasMaxLength(3).IsRequired();
            builder.Property(x => x.Simbolo).HasMaxLength(5).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

        }
    }
}
