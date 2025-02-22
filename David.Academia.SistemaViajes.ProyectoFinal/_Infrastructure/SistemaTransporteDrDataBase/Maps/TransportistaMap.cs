using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class TransportistaMap : IEntityTypeConfiguration<Transportista>
    {
        public void Configure(EntityTypeBuilder<Transportista> builder)
        {
            builder.ToTable("Transportistas");
            builder.HasKey(t => t.TransportistaId);
            builder.Property(t => t.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Apellido).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Dni).HasMaxLength(30).IsRequired();
            builder.HasIndex(t => t.Dni).IsUnique();
            builder.Property(t => t.Telefono).HasMaxLength(15).IsRequired(false);
            builder.Property(t => t.Email).HasMaxLength(50).IsRequired(false);
            builder.HasIndex(t => t.Email).IsUnique();
            builder.Property(t => t.TarifaPorKm).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(t => t.FechaCreacion).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(t => t.UsuarioCrea).IsRequired();
            builder.Property(t => t.FechaActualiza).IsRequired(false);
            builder.Property(t => t.UsuarioActualiza).IsRequired(false);
            builder.Property(t => t.Activo).HasDefaultValue(true).IsRequired();


            builder.HasOne(t => t.UsuarioCreaNavigation)
                .WithMany(u => u.TransportistasCrea)
                .HasForeignKey(t => t.UsuarioCrea);

            builder.HasOne(t => t.UsuarioActualizaNavigation)
                .WithMany(u => u.TransportistasActualiza)
                .HasForeignKey(t => t.UsuarioActualiza);

        }
    }
}
