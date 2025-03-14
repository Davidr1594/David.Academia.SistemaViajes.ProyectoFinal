using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps.Entities;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Extensions.Configuration;
using Farsiman.Infraestructure.Core.Entity.Standard;
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



//builder.Services.AddDbContext<SistemaTransporteDrContext>(o => o.UseSqlServer(
//                builder.Configuration.GetConnectionStringFromENV("SISTEMATRANSPORTE")
//            ));


var isTesting = builder.Environment.IsEnvironment("Test");

if (!isTesting)
{
    builder.Services.AddDbContext<SistemaTransporteDrContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionStringFromENV("SISTEMATRANSPORTE"));
    });
    builder.Services.AddScoped<IUnitOfWork>(ServiceProvider =>
    {
        var dbContext = ServiceProvider.GetRequiredService<SistemaTransporteDrContext>();
        return new UnitOfWork(dbContext);
    });
}


// Servicios de Aplicación
builder.Services.AddScoped<UnitOfWorkBuilder>();

#region clases de servicios
builder.Services.AddTransient<RolService>();
builder.Services.AddTransient<UsuarioService>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<ColaboradorService>();
builder.Services.AddTransient<SucursalService>();
builder.Services.AddTransient<TransportistaService>();
builder.Services.AddTransient<ViajeService>();
builder.Services.AddTransient<ParametroSistemaService>();
builder.Services.AddTransient<ReportesService>();

builder.Services.AddScoped<IManejoDistanciasService, ManejoDistanciasService>();
#endregion

#region Domains
builder.Services.AddScoped<UsuarioDomain>();
builder.Services.AddScoped<AuthDomain>();
builder.Services.AddScoped<ViajeDomain>();
builder.Services.AddScoped<SucursalDomain>();
builder.Services.AddScoped<ReporteDomain>();
builder.Services.AddScoped<ColaboradorDomain>();
builder.Services.AddScoped<ParametroSistemaDomain>();
builder.Services.AddScoped<RolDomain>();
builder.Services.AddScoped<TransportistaDomain>();
builder.Services.AddScoped<SolicitudViajeDomain>();







#endregion






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

public partial class Program { }