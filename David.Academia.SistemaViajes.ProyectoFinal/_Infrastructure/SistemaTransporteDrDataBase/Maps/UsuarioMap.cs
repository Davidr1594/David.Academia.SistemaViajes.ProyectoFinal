using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(x => x.UsuarioId);
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.ClaveHash).HasMaxLength(255).IsRequired();
            builder.Property(x => x.RolId).IsRequired();
            builder.Property(x => x.ColaboradorId).IsRequired(false);
            builder.Property(x => x.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.UsuarioCrea).IsRequired();
            builder.Property(x => x.FechaActualizacion).IsRequired(false);
            builder.Property(x => x.UsuarioActualiza).IsRequired(false);
            builder.Property(x => x.Activo).HasDefaultValue(true);

           
            builder.HasOne(u => u.RolNavigation)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            builder.HasOne(u => u.UsuarioCreaNavigation)
                .WithMany(u => u.UsuariosCrea)
                .HasForeignKey(u => u.UsuarioCrea);

            builder.HasOne(u => u.UsuarioActualizaNavigation)
                .WithMany(u => u.UsuariosActualiza)
                .HasForeignKey(u => u.UsuarioActualiza);

            builder.HasOne(u => u.ColaboradorNavigation)
                .WithMany(c => c.UsuariosViajes)
                .HasForeignKey(u => u.ColaboradorId);



        }
    }
}
