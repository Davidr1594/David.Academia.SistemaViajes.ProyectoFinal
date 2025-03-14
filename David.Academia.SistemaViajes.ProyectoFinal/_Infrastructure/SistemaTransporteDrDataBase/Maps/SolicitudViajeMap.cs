using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class SolicitudViajeMap : IEntityTypeConfiguration<SolicitudViaje>
    {
        public void Configure(EntityTypeBuilder<SolicitudViaje> builder)
        {
            builder.ToTable("SolicitudesViajes");
            builder.HasKey(sv => sv.SolicitudId);
            builder.Property(sv => sv.FechaSolicitud).IsRequired();
            builder.Property(sv => sv.EstadoId).IsRequired();
            builder.Property(sv => sv.Comentario).HasMaxLength(100).IsRequired(false);
            builder.Property(sv => sv.FechaCreacion).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(sv => sv.FechaActualiza).IsRequired(false);
            builder.Property(sv => sv.UsuarioActualiza).IsRequired(false);
            builder.Property(sv => sv.Activo).HasDefaultValue(true).IsRequired();

            builder.HasOne(sv => sv.ColaboradorNavigation)
                .WithMany(c => c.SolicitudeViajes)
                .HasForeignKey(sv => sv.ColaboradorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sv => sv.SucursalNavigation)
                .WithMany(c => c.SolicitudeViajes)
                .HasForeignKey(sv => sv.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sv => sv.UsuarioAprobadorNavigation)
                .WithMany(u => u.SolicitudeViajesCrea)
                .HasForeignKey(sv => sv.UsuarioAprueba)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sv => sv.UsuarioActualizadorNavigation)
                .WithMany(u => u.SolicitudeViajesActualiza)
                .HasForeignKey(sv => sv.UsuarioActualiza)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(sv => new { sv.ColaboradorId, sv.FechaSolicitud })
                .IsUnique();
        }
    }
}
