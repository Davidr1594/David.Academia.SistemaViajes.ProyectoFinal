using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps.Entities;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Ciudades;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Controllers;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder =>
        {
            if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Staging"))
            {
                corsBuilder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
            else
            {
                corsBuilder
                .WithOrigins("https://*.grupofarsiman.com", "https://*.grupofarsiman.io")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.GetConnectionStringFromENV("SISTEMATRANSPORTE");

builder.Services.Configure<GoogleMapsSetting>(builder.Configuration.GetSection("GoogleMapsSetting"));
builder.Services.AddHttpClient<ManejoDistanciasService>();



builder.Services.AddDbContext<SistemaTransporteDrContext>(o => o.UseSqlServer(
                builder.Configuration.GetConnectionStringFromENV("SISTEMATRANSPORTE")
            ));


// Servicios de Aplicación
builder.Services.AddScoped<UnitOfWorkBuilder>();

builder.Services.AddTransient<RolService>();
builder.Services.AddTransient<UsuarioService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<ColaboradorService>();
builder.Services.AddTransient<SucursalService>();
builder.Services.AddTransient<PaisService>();
builder.Services.AddTransient<TransportistaService>();
builder.Services.AddTransient<CiudadService>();
builder.Services.AddTransient<EstadoDepartamentoService>();
builder.Services.AddTransient<MetodoPagoService>();
builder.Services.AddTransient<EstadoPagoService>();
builder.Services.AddTransient<MonedaService>();
builder.Services.AddTransient<EstadoViajeService>();
builder.Services.AddTransient<PuestoService>();
builder.Services.AddTransient<ViajeService>();
builder.Services.AddTransient<ParametroSistemaService>();
builder.Services.AddTransient<ReportesService>();



builder.Services.AddScoped<UsuarioDomain>();
builder.Services.AddScoped<AuthDomain>();
builder.Services.AddScoped<ViajeDomain>();
builder.Services.AddScoped<SucursalDomain>();
builder.Services.AddScoped<ReporteDomain>();
builder.Services.AddScoped<ColaboradorDomain>();







builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.UseAuthentication();

//app.UseFsAuthService();

app.MapControllers();

app.Run();