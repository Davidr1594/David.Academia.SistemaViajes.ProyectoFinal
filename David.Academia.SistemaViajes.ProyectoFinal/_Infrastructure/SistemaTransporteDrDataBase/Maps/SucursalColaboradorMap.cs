using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class SucursalColaboradorMap : IEntityTypeConfiguration<SucursalColaborador>
    {
        public void Configure(EntityTypeBuilder<SucursalColaborador> builder)
        {
            builder.ToTable("SucursalColaboradores");
            builder.HasKey(sc => new { sc.SucursalId, sc.ColaboradorId });
            builder.Property(sc => sc.DistanciaKm).HasColumnType("decimal(10,2)").IsRequired();

            builder.HasOne(sc => sc.SucursalNavigation)
                .WithMany(s => s.SucursalColaboradores)
                .HasForeignKey(sc => sc.SucursalId);

            builder.HasOne(sc => sc.ColaboradorNavigation)
                .WithMany(c => c.SucursalColaboradores)
                .HasForeignKey(sc => sc.ColaboradorId);

        }
    }
}
