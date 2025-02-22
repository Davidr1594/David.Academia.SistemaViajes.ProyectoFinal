using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class PagoContratistaMap : IEntityTypeConfiguration<PagoContratista>
    {
        public void Configure(EntityTypeBuilder<PagoContratista> builder)
        {
            builder.ToTable("PagosContratistas");
            builder.HasKey(p => p.PagoId);
            builder.Property(p => p.FechaPago).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(p => p.MontoTotal).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(p => p.Comentario).HasMaxLength(100).IsRequired(false);

            builder.HasOne(p => p.TransportistaNavigation)
                .WithMany(t => t.PagosContratistas)
                .HasForeignKey(p => p.TransportistaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.EstadoPagoNavigation)
                .WithMany(e => e.PagosContratistas)
                .HasForeignKey(p => p.EstadoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.MetodoPagoNavigation)
                .WithMany(m => m.PagosContratistas)
                .HasForeignKey(p => p.MetodoPagoId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.UsuarioRegistraNavigation)
                .WithMany(u => u.PagosContratistas)
                .HasForeignKey(p => p.UsuarioRegistraId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
