using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps
{
    public class ParametroSistemaMap : IEntityTypeConfiguration<ParametroSistema>
    {
        public void Configure(EntityTypeBuilder<ParametroSistema> builder)
        {
            builder.ToTable("ParametrosSistema");
            builder.HasKey(p => p.RegistroId);
            builder.HasIndex(p => p.Descripcion).IsUnique();
            builder.Property(p => p.Descripcion).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Valor).IsRequired(false);
            builder.Property(p => p.ValorString).HasMaxLength(100).IsRequired(false);
            builder.Property(p => p.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(p => p.FechaActualiza).HasColumnType("datetime").IsRequired(false);
            builder.Property(p => p.Activo).HasDefaultValue(true).IsRequired();

            builder.HasOne(p => p.UsuarioCreaNavigation)
                   .WithMany(u => u.ParametrosSistemasCrea)
                   .HasForeignKey(p => p.UsuarioCrea)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UsuarioActualizaNavigation)
                   .WithMany(u => u.ParametrosSistemasActualiza)
                   .HasForeignKey(p => p.UsuarioActualiza)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
