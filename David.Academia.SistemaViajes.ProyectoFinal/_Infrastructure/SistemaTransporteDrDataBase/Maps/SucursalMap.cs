using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class SucursalMap : IEntityTypeConfiguration<Sucursal>
    {
        public void Configure(EntityTypeBuilder<Sucursal> builder)
        {
            builder.ToTable("Sucursales");
            builder.HasKey(s => s.SucursalId);
            builder.Property(s => s.SucursalId).IsRequired();
            builder.HasIndex(s => s.Nombre).IsUnique();
            builder.Property(s => s.Nombre).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Direccion).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Latitud).HasColumnType("decimal(9,6)").IsRequired();
            builder.Property(s => s.Longitud).HasColumnType("decimal(9,6)").IsRequired();
            builder.Property(s => s.Telefono).HasMaxLength(15).IsRequired(false);
            builder.Property(s => s.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(s => s.UsuarioCrea).IsRequired();
            builder.Property(s => s.FechaActualizacion).HasColumnType("datetime").IsRequired(false);
            builder.Property(s => s.UsuarioActualiza).IsRequired(false);
            builder.Property(s => s.Activo).HasDefaultValue(true).IsRequired();

            builder.HasOne(s => s.UsuarioCreaNavigation)
                .WithMany(u => u.SucursalesCrea)
                .HasForeignKey(s => s.UsuarioCrea);

            builder.HasOne(s => s.UsuarioActualizadorNavigation)
                .WithMany(u => u.SucursalesActualiza)
                .HasForeignKey(s => s.UsuarioActualiza);

        }
    }
}
