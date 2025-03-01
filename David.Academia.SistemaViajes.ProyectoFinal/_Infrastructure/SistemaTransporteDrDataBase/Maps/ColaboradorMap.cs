using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class ColaboradorMap : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.ToTable("Colaboradores");
            builder.HasKey(x => x.ColaboradorId);
            builder.Property(x => x.ColaboradorId).IsRequired();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Apellido).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Telefono).HasMaxLength(15).IsRequired();
            builder.Property(x => x.FechaNacimiento).IsRequired();
            builder.Property(x => x.Dni).HasMaxLength(30).IsRequired();
            builder.HasIndex(x => x.Dni).IsUnique();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Direccion).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Latitud).HasColumnType("DECIMAL(19,15)").IsRequired();
            builder.Property(x => x.Longitud).HasColumnType("DECIMAL(19,15)").IsRequired();
            builder.Property(x => x.CiudadId).IsRequired(false);
            builder.Property(x => x.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.UsuarioCrea).IsRequired(false);
            builder.Property(x => x.FechaActualizacion).IsRequired(false);
            builder.Property(x => x.UsuarioActualiza).IsRequired(false);
            builder.Property(x => x.PuestoId).IsRequired(false);
            builder.Property(x => x.Activo).HasDefaultValue(true);

            builder.HasOne(co => co.CiudadNavigation)
                    .WithMany(ci => ci.Colaboradores)
                    .HasForeignKey(co => co.CiudadId);

            builder.HasOne(co => co.PuestoNavigation)
                    .WithMany(p => p.Colaboradores)
                    .HasForeignKey(co => co.PuestoId);


            builder.HasOne(c => c.UsuarioCreaNavigation)
                    .WithMany(u => u.ColaboradoresCrear)
                    .HasForeignKey(c => c.UsuarioCrea);

            builder.HasOne(c => c.UsuarioActualizaNavigation)
                    .WithMany(u => u.ColaboradoresActualizar)
                    .HasForeignKey(c => c.UsuarioActualiza);

        }
    }
}
