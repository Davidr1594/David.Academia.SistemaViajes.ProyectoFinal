using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class EstadoPagoMap : IEntityTypeConfiguration<EstadoPago>
    {
        public void Configure(EntityTypeBuilder<EstadoPago> builder)
        {
            builder.ToTable("EstadosPagos");
            builder.HasKey(e => e.EstadoPagoId);
            builder.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Activo).HasDefaultValue(true).IsRequired();

        }
    }
}
