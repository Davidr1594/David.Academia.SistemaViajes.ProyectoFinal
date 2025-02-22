using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Maps;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase
{
    public class SistemaTransporteDrContext : DbContext
    {
        public SistemaTransporteDrContext(DbContextOptions<SistemaTransporteDrContext> options) : base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CiudadMap());
            modelBuilder.ApplyConfiguration(new ColaboradorMap());
            modelBuilder.ApplyConfiguration(new EstadoDepartamentoMap());
            modelBuilder.ApplyConfiguration(new EstadoPagoMap());
            modelBuilder.ApplyConfiguration(new EstadoViajeMap());
            modelBuilder.ApplyConfiguration(new MetodoPagoMap());
            modelBuilder.ApplyConfiguration(new MonedaMap());
            modelBuilder.ApplyConfiguration(new PagoContratistaMap());
            modelBuilder.ApplyConfiguration(new PagoViajeMap());
            modelBuilder.ApplyConfiguration(new PaisMap());
            modelBuilder.ApplyConfiguration(new ParametroSistemaMap());
            modelBuilder.ApplyConfiguration(new PermisoMap());
            modelBuilder.ApplyConfiguration(new PermisosRolesMap());
            modelBuilder.ApplyConfiguration(new PuestoMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new SolicitudViajeMap());
            modelBuilder.ApplyConfiguration(new SucursalColaboradorMap());
            modelBuilder.ApplyConfiguration(new SucursalMap());
            modelBuilder.ApplyConfiguration(new TransportistaMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ValoracionViajeMap());
            modelBuilder.ApplyConfiguration(new ViajeDetalleMap());
            modelBuilder.ApplyConfiguration(new ViajeMap());

        }

        public DbSet<Ciudad> Ciudades => Set<Ciudad>();
        public DbSet<Colaborador> Colaboradores => Set<Colaborador>();
        public DbSet<EstadoDepartamento> EstadosDepartamentos => Set<EstadoDepartamento>();
        public DbSet<EstadoPago> EstadosPagos => Set<EstadoPago>();
        public DbSet<EstadoViaje> EstadosViajes => Set<EstadoViaje>();
        public DbSet<MetodoPago> MetodosPagos => Set<MetodoPago>();
        public DbSet<Moneda> Monedas => Set<Moneda>();
        public DbSet<PagoContratista> PagosConstratistas => Set<PagoContratista>();
        public DbSet<PagoViaje> PagosViajes => Set<PagoViaje>();
        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<ParametroSistema> parametroSistemas => Set<ParametroSistema>();
        public DbSet<Permiso> Permisos => Set<Permiso>();
        public DbSet<PermisoRol> PermisosRoles => Set<PermisoRol>();
        public DbSet<Puesto> Puestos => Set<Puesto>();
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<SolicitudViaje> SolicitudesViajes => Set<SolicitudViaje>();
        public DbSet<Sucursal> Sucursales => Set<Sucursal>();
        public DbSet<SucursalColaborador> SucursalesColaboradores => Set<SucursalColaborador>();
        public DbSet<Transportista> Transportistas => Set<Transportista>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<ValoracionViaje> ValoracionesViajes => Set<ValoracionViaje>();
        public DbSet<Viaje> Viajes => Set<Viaje>();
        public DbSet<ViajeDetalle> ViajesDetalles => Set<ViajeDetalle>();



    }
}