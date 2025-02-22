using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class EstadoViajeMap : IEntityTypeConfiguration<EstadoViaje>
    {
        public void Configure(EntityTypeBuilder<EstadoViaje> builder)
        {
            builder.ToTable("EstadosViajes");
            builder.HasKey(e => e.EstadoViajeId);
            builder.Property(e => e.EstadoViajeId).IsRequired();
            builder.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Activo).HasDefaultValue(true).IsRequired();
        }
    }
}
