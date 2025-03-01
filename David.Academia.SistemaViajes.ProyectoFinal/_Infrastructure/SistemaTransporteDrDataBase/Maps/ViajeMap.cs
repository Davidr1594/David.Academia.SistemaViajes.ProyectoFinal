using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class ViajeMap : IEntityTypeConfiguration<Viaje>
    {
        public void Configure(EntityTypeBuilder<Viaje> builder)
        {
            builder.ToTable("Viajes");
            builder.HasKey(v => v.ViajeId);
            builder.Property(v => v.FechaViaje).IsRequired();
            builder.Property(v => v.SucursalId).IsRequired();
            builder.Property(v => v.TransportistaId).IsRequired();
            builder.Property(v => v.TotalKms).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(v => v.MontoTotal).HasColumnType("money").IsRequired();
            builder.Property(v => v.MonedaId).IsRequired();
            builder.Property(v => v.HoraSalida).HasColumnType("time").IsRequired();
            builder.Property(v => v.EstadoId).IsRequired();
            builder.Property(v => v.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(v => v.UsuarioCrea).IsRequired();
            builder.Property(v => v.FechaActualiza).IsRequired(false);
            builder.Property(v => v.UsuarioActualiza).IsRequired(false);
            builder.Property(v => v.Activo).HasDefaultValue(true).IsRequired();

            builder.HasOne(v => v.SucursalNavigation)
                .WithMany(s => s.Viajes)
                .HasForeignKey(v => v.SucursalId);

            builder.HasOne(v => v.TransportistaNavigation)
                .WithMany(t => t.Viajes)
                .HasForeignKey(v => v.TransportistaId);

            builder.HasOne(v => v.UsuarioCreadorNavigation)
                .WithMany(u => u.ViajesCrea)
                .HasForeignKey(v => v.UsuarioCrea);

            builder.HasOne(v => v.UsuarioActualizadorNavigation)
                .WithMany(u => u.ViajesActualiza)
                .HasForeignKey(v => v.UsuarioActualiza);

            builder.HasOne(v => v.EstadoNavigation)
                .WithMany(e => e.Viajes)
                .HasForeignKey(v => v.EstadoId);

            builder.HasOne(v => v.MonedaNavigation)
                .WithMany(m => m.Viajes)
                .HasForeignKey(v => v.MonedaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
