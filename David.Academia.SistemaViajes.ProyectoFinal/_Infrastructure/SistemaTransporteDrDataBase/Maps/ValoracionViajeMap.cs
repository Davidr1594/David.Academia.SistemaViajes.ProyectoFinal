using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class ValoracionViajeMap : IEntityTypeConfiguration<ValoracionViaje>
    {
        public void Configure(EntityTypeBuilder<ValoracionViaje> builder)
        {
            builder.ToTable("ValoracionesViajes");
            builder.HasKey(vv => vv.ValoracionId);
            builder.Property(vv => vv.Calificacion).IsRequired();
            builder.Property(vv => vv.Comentario).HasMaxLength(100).IsRequired(false);
            builder.Property(vv => vv.FechaCreacion).HasDefaultValueSql("GETDATE()").IsRequired();

            builder.HasOne(vv => vv.TransportistaNavigation)
                .WithMany(t => t.ValoracionesViajes)
                .HasForeignKey(vv => vv.TransportistaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(vv => vv.ColaboradorNavigation)
                .WithMany(c => c.ValoracionViajes)
                .HasForeignKey(vv => vv.ColaboradorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(vv => new { vv.TransportistaId, vv.ColaboradorId })
                .IsUnique();
        }
    }
}
