using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class EstadoDepartamentoMap : IEntityTypeConfiguration<EstadoDepartamento>
    {
        public void Configure(EntityTypeBuilder<EstadoDepartamento> builder)
        {
            builder.ToTable("EstadoDepartamento");
            builder.HasKey(x => x.EstadoId);
            builder.Property(x => x.EstadoId).IsRequired();
            builder.HasIndex(x => x.Nombre).IsUnique();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.PaisId).IsRequired();
            builder.Property(x => x.Activo).HasDefaultValue(true);

            builder.HasOne(e => e.PaisNavigation)
                    .WithMany(p => p.EstadosDepartamentos)
                    .HasForeignKey(e => e.PaisId);

        }
    }
}
