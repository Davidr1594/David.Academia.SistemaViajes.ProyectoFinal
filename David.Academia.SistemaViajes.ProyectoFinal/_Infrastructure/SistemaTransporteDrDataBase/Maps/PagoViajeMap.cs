using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PagoViajeMap : IEntityTypeConfiguration<PagoViaje>
    {
        public void Configure(EntityTypeBuilder<PagoViaje> builder)
        {
            builder.ToTable("PagosViajes");
            builder.HasKey(pv => new { pv.PagoId, pv.ViajeId });
            builder.Property(pv => pv.MontoPagado).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasOne(pv => pv.PagoContratista)
                .WithMany(pc => pc.PagosViajes)
                .HasForeignKey(pv => pv.PagoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Viaje)
                .WithMany(v => v.PagosViajes)
                .HasForeignKey(pv => pv.ViajeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
