using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class MetodoPagoMap : IEntityTypeConfiguration<MetodoPago>
    {
        public void Configure(EntityTypeBuilder<MetodoPago> builder)
        {
            builder.ToTable("MetodosPagos");
            builder.HasKey(m => m.MetodoPagoId);
            builder.Property(m => m.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(m => m.Activo).HasDefaultValue(true).IsRequired();
        }
    }
}
