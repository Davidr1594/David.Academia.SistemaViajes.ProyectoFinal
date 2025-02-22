using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class CiudadMap : IEntityTypeConfiguration<Ciudad>
    {
        public void Configure(EntityTypeBuilder<Ciudad> builder)
        {
            builder.ToTable("Ciudades");
            builder.HasKey(x => x.CiudadId);
            builder.Property(x => x.CiudadId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EstadoId).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

            builder.HasOne(c => c.EstadoDepartamentoNavigation)
                    .WithMany(e => e.Ciudades)
                    .HasForeignKey(c => c.EstadoId);
        }
    }
}
