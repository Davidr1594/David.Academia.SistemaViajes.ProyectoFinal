
using AcademiaIntegrationTestAndMock.IntegrationTest.Mocks;

using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;

namespace David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {

                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SistemaTransporteDrContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }
                var unitOfWorkDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
                if (unitOfWorkDescriptor != null)
                {
                    services.Remove(unitOfWorkDescriptor);
                }

                services.RemoveService<IManejoDistanciasService>();
  

                services.AddDefaultManejoDistanciasServiceMock();


                services.AddDbContext<SistemaTransporteDrContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb")
                               .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning));
                    ;
                });
                services.AddScoped<IUnitOfWork>(serviceProvider =>
                {
                    var dbContext = serviceProvider.GetRequiredService<SistemaTransporteDrContext>();
                    return new UnitOfWork(dbContext);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SistemaTransporteDrContext>();
                context.Database.EnsureCreated();
                SeedTestData(context);

            });

            builder.UseEnvironment("Test");



        }


        private static void SeedTestData(SistemaTransporteDrContext context)
        {
            if (!context.Colaboradores.Any())
            {
                context.Colaboradores.AddRange(new[]
                {
                    new Colaborador
                    {
                        Nombre = "Juan",
                        Apellido = "Pérez",
                        Dni = "12345678",
                        Email = "juan.perez@example.com",
                        Telefono = "987654321",
                        Direccion = "San Pedro Sula",
                        Latitud = 10.0m,
                        Longitud = -85.5m,
                        PuestoId = 1,
                        CiudadId = 1,
                        UsuarioCrea = 1
                    },
                    new Colaborador
                    {
                        Nombre = "María",
                        Apellido = "González",
                        Dni = "87654321",
                        Email = "maria.gonzalez@example.com",
                        Telefono = "123456789",
                        Direccion = "San Pedro Sula",
                        Latitud = 15.0m,
                        Longitud = -75.0m,
                        PuestoId = 2,
                        CiudadId = 2,
                        UsuarioCrea = 1
                    },
                    new Colaborador
                    {
                        Nombre = "Deras",
                        Apellido = "González",
                        Dni = "87654321",
                        Email = "Deras@example.com",
                        Telefono = "123456789",
                        Direccion = "San Pedro Sula",
                        Latitud = 15.0m,
                        Longitud = -75.0m,
                        PuestoId = 2,
                        CiudadId = 2,
                        UsuarioCrea = 1
                    }

                });

                context.SaveChanges();
            }

            if (!context.Sucursales.Any())
            {
                context.Sucursales.AddRange(new[]
                {
                    new Sucursal
                    {
                        SucursalId = 1,
                        Nombre = "Sucursal Central",
                        Latitud = 10.5m,
                        Longitud = -80.3m,
                        Telefono = "999888777",
                        Activo = true
                    },
                    new Sucursal
                    {
                        SucursalId = 2,
                        Nombre = "Sucursal Norte",
                        Latitud = 12.8m,
                        Longitud = -81.6m,
                        Telefono = "777888999",
                        Activo = true
                    }
                });

                context.SaveChanges();
            }

            if (!context.Transportistas.Any())
            {
                context.Transportistas.Add(new Transportista
                {
                    Nombre = "Carlos",
                    Apellido = "Rodriguez",
                    Dni = "54321987",
                    Email = "carlos.rodriguez@example.com",
                    Telefono = "1122334455",
                    TarifaPorKm = 10.5m,
                    Activo = true
                });

                context.SaveChanges();
            }
            if (!context.parametroSistemas.Any())
            {
                context.parametroSistemas.Add(new ParametroSistema
                {
                    Descripcion = "Total kms maximo",
                    Valor = 100,
                    ValorString = "",
                    Activo = true
                });

                context.SaveChanges();
            }
            if (!context.SucursalesColaboradores.Any())
            {
                context.SucursalesColaboradores.AddRange(new[]
                {
                    new SucursalColaborador
                    {
                        SucursalId = 1,
                        ColaboradorId = 1,
                        DistanciaKm = 8.10m
                    },
                    new SucursalColaborador
                    {
                        SucursalId = 1,
                        ColaboradorId = 2,
                        DistanciaKm = 13.4m
                    }, 
                    new SucursalColaborador
                    {
                        SucursalId = 1,
                        ColaboradorId = 3,
                        DistanciaKm = 10.4m
                    }
                });
            }
                context.SaveChanges();
         
            if (!context.Ciudades.Any())
            {
                context.Ciudades.AddRange(new[]
                {
                    new Ciudad
                    {
                        CiudadId = 1,
                        Nombre = "Tegucigalpa",
                        Activo = true,
                        EstadoId = 1
                    },
                    new Ciudad
                    {
                        CiudadId = 2,
                        Nombre = "San Pedro Sula",
                        Activo = true,
                        EstadoId = 2
                    }
                });

                context.SaveChanges();
            }

            if (!context.Puestos.Any())
            {
                context.Puestos.AddRange(new[]
                {
                    new Puesto
                    {
                        PuestoId = 1,
                        Nombre = "Desarrollador",
                        Descripcion = "Encargado de desarrollar software",
                        Activo = true
                    },
                    new Puesto
                    {
                        PuestoId = 2,
                        Nombre = "Analista",
                        Descripcion = "Encargado de analizar requerimientos",
                        Activo = true
                    }
                 });

                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new[]
                {
                    new Rol
                    {
                        RolId = 1,
                        Nombre = "Administrador",
                        Activo = true
                    },
                    new Rol
                    {
                        RolId = 2,
                        Nombre = "Gerente",
                        Activo = true
                    }
                });

                context.SaveChanges();
            }

            if (!context.Usuarios.Any())
            {
                context.Usuarios.AddRange(new[]
                {
                    new Usuario
                    {
                        UsuarioId = 1,
                        ClaveHash = EncriptarClave("admin123"),
                        Nombre = "Davidr",
                        Email = "admin@example.com",
                        RolId = 1,
                        ColaboradorId = null,
                        Activo = true
                    },
                    new Usuario
                    {
                        UsuarioId = 2,
                        ClaveHash = EncriptarClave("user123"),
                        Nombre = "Josue",
                        Email = "user@example.com",
                        RolId = 2,
                        ColaboradorId = null,
                        Activo = true
                    }
                });

                context.SaveChanges();
            }
            if (!context.Monedas.Any())
            {
                context.Monedas.AddRange(new[]
                {
                    new Moneda
                    {
                        MonedaId = 1,
                        Nombre = "Dólar Estadounidense",
                        CodigoIso = "USD",
                        Simbolo = "$",
                        Activo = true
                    },
                    new Moneda
                    {
                        MonedaId = 2,
                        Nombre = "Lempira",
                        CodigoIso = "HNL",
                        Simbolo = "L",
                        Activo = true
                    }
                });

                context.SaveChanges();
            }
            if (!context.Viajes.Any())
            {
                context.Viajes.AddRange(new[]
                {
                    new Viaje
                    {
                        ViajeId = 1,
                        FechaCreacion = DateTime.Now.Date,
                        SucursalId = 1, 
                        TransportistaId = 1,
                        TotalKms = 150.5m,
                        MontoTotal = 1575.5m,
                        MonedaId = 1,
                        HoraSalida = new TimeSpan(8, 30, 0),
                        EstadoId = 1,
                        Activo = true
                    },
                    new Viaje
                    {
                            ViajeId = 2,
                            FechaCreacion = DateTime.UtcNow.AddDays(-1),
                            SucursalId = 2,
                            TransportistaId = 1,
                            TotalKms = 75.3m,
                            MontoTotal = 780.5m,
                            MonedaId = 1,
                            HoraSalida = new TimeSpan(14, 0, 0),
                            EstadoId = 2,
                            Activo = true
                     } 
                });

                context.SaveChanges();
                if (!context.ViajesDetalles.Any())
                {
                    context.ViajesDetalles.Add(new ViajeDetalle
                    {
                        ColaboradorId = 3,
                        DireccionDestino = "San Pedro Sula",
                        ViajeId = 1,
                    });

                    context.SaveChanges();
                }
            }
        }

        private static byte[] EncriptarClave(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }
        }



    }


}
