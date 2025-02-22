using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class ViajeDetalleMap : IEntityTypeConfiguration<ViajeDetalle>
    {
        public void Configure(EntityTypeBuilder<ViajeDetalle> builder)
        {
            builder.ToTable("ViajesDetalles");
            builder.HasKey(vd => vd.ViajeDetalleId);
            builder.Property(vd => vd.DireccionDestino).HasMaxLength(100).IsRequired(false);
            builder.Property(vd => vd.Kms).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(vd => vd.Costo).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasOne(vd => vd.Viaje)
                .WithMany(v => v.ViajeDetalles)
                .HasForeignKey(vd => vd.ViajeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vd => vd.Colaborador)
                .WithMany(c => c.ViajeDetalles)
                .HasForeignKey(vd => vd.ColaboradorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(vd => new { vd.ViajeId, vd.ColaboradorId })
                .IsUnique();
        }
    }
}
