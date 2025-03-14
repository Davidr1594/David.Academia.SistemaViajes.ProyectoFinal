using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using AcademiaIntegrationTestAndMock.IntegrationTest.Mocks;
using Microsoft.EntityFrameworkCore;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using David.Academia.SistemaViajes.IntregrationTests.Mocks.Infranstructure;
using NSubstitute.ExceptionExtensions;

namespace David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests.Features.Colaboradores
{
    public class ColaboradoresTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly IManejoDistanciasService _manejoDistanciasService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public ColaboradoresTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            _manejoDistanciasService = scope.ServiceProvider.GetRequiredService<IManejoDistanciasService>();
            _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _factory = factory;

        }

        [Fact]
        public async Task ObtenerColaboradores_DeberiaRetornar200YListaDeColaboradores()
        {
            //Arrange
            var response = await _httpClient.GetAsync("/api/colaborador/ObtenerColaboradores");

            // Act
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Deserializar correctamente en Respuesta<IEnumerable<Colaborador>>
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<IEnumerable<Colaborador>>>();

            // Assert
            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeTrue();
            apiResponse.Datos.Should().NotBeNullOrEmpty();
            apiResponse.Datos.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task CrearColaborador_DeberiaRetornar200YColaboradorCreado()
        {

            ManejoDistanciasServiceMock.SetupExitosoObtenerDireccionDesdeCordenadas(_manejoDistanciasService);

            // Arrange
            var nuevoColaborador = new ColaboradorDto
            {
                ColaboradorId = 0, // Se asignará automáticamente en la BD
                Nombre = "Carlos",
                Apellido = "Rodriguez",
                Dni = "54321987",
                Email = "carlos.rodriguez@example.com",
                Telefono = "1122334455",
                Direccion = "",
                FechaNacimiento = new DateTime(1990, 05, 20),
                Latitud = 10.0m,
                Longitud = -85.5m,
                PuestoId = 1,
                CiudadId = 1,
                Activo = true
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync($"/api/colaborador/CrearColaborador/?usuarioCreaId=1", nuevoColaborador);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ColaboradorDto>>();

            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeTrue();
            apiResponse.Datos.Should().NotBeNull();
            apiResponse.Datos.Nombre.Should().Be(nuevoColaborador.Nombre);
            apiResponse.Datos.Email.Should().Be(nuevoColaborador.Email);
            apiResponse.Datos.FechaNacimiento.Should().Be(nuevoColaborador.FechaNacimiento);
            apiResponse.Datos.Direccion.Should().Be("San Pedro Sula"); // Verificar que la dirección sea la esperada
        }
        [Fact]
        public async Task CrearColaborador_DeberiaRetornarBadRequest_FallaApiGoogle()
        {

            ManejoDistanciasServiceMock.SetupErrorObtenerDireccionDesdeCordenadas(_manejoDistanciasService);

            // Arrange
            var nuevoColaborador = new ColaboradorDto
            {
                ColaboradorId = 0,
                Nombre = "Carlos",
                Apellido = "Rodriguez",
                Dni = "54329999",
                Email = "carlos.ro@example.com",
                Telefono = "1122334455",
                Direccion = "",
                FechaNacimiento = new DateTime(1990, 05, 20),
                Latitud = 0.0m, // coordenada inválida para forzar excepción claramente
                Longitud = 0.0m,
                PuestoId = 1,
                CiudadId = 1,
                Activo = true
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/colaborador/CrearColaborador/?usuarioCreaId=1", nuevoColaborador);



            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ColaboradorDto>>();

            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeFalse();
            apiResponse.Mensaje.Should().NotBeNullOrEmpty();
            apiResponse.Mensaje.Should().Contain("No se pudo obtener la dirección.");

        }

        [Fact]
        public async Task CrearColaborador_DeberiaFallar_CuandoElServicioDeDistanciasLanzaExcepcion()
        {
            ManejoDistanciasServiceMock.SetupThrowExceptionObtenerDireccionDesdeCordenadas(_manejoDistanciasService);

            var nuevoColaborador = new ColaboradorDto
            {
                ColaboradorId = 0,
                Nombre = "Carlos",
                Apellido = "Rodriguez",
                Dni = "54329999",
                Email = "carlos.rodriguez@example.com",
                Telefono = "1122334455",
                Direccion = "",
                FechaNacimiento = new DateTime(1990, 05, 20),
                Latitud = 10.0m,
                Longitud = -85.5m,
                PuestoId = 1,
                CiudadId = 1,
                Activo = true
            };

            var response = await _httpClient.PostAsJsonAsync("/api/colaborador/CrearColaborador?usuarioCreaId=1", nuevoColaborador);

            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ColaboradorDto>>();

            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeFalse();
            apiResponse.DetalleError.Should().Contain("Error en el servicio de distancias.");
        }

    }

}
