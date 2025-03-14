using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using AcademiaIntegrationTestAndMock.IntegrationTest.Mocks;


namespace David.Academia.SistemaViajes.IntregrationTests.Features.Viajes
{
    public class ViajeTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly IManejoDistanciasService _manejoDistanciasService;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public ViajeTest(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            _manejoDistanciasService = scope.ServiceProvider.GetRequiredService<IManejoDistanciasService>();
            _factory = factory;

        }

        [Fact]
        public async Task ObtenerViajes_DeberiaRetornar200YListaDeColaboradores()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/viaje/ObtenerViajes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Deserializar correctamente en Respuesta<IEnumerable<Colaborador>>
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<IEnumerable<Viaje>>>();

            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeTrue();
            apiResponse.Datos.Should().NotBeNullOrEmpty();
            apiResponse.Datos.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task CrearViaje_DeberiaRetornarSuccess_CuandoDatosSonValidos()
        {
            ManejoDistanciasServiceMock.SetupExitosoCalcularDistanciaTotalAjustadaAsync(_manejoDistanciasService);
            // Arrange
            var nuevoViaje = new
            {
                FechaViaje = DateTime.UtcNow,
                SucursalId = 1,
                TransportistaId = 1,
                TotalKms = 120.5m,
                MontoTotal = 2000.0m,
                MonedaId = 1,
                HoraSalida = new TimeSpan(9, 0, 0),
                EstadoId = 1,
                Activo = true,
                ColaboradoresId = new List<int> { 1, 2 }
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/viaje/CrearViaje/?usuarioCreaId=1", nuevoViaje);
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ViajeDto>>();


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse!.Valido.Should().BeTrue();
            apiResponse.Mensaje.Should().Be("Se guardó con éxito.");
        }

        [Fact]
        public async Task CrearViaje_DeberiaRetornarBadRequest_CuandoSucursalNoExiste()
        {
            ManejoDistanciasServiceMock.SetupExitosoCalcularDistanciaTotalAjustadaAsync(_manejoDistanciasService);
            // Arrange
            var nuevoViaje = new
            {
                FechaViaje = DateTime.UtcNow,
                SucursalId = 999, // Sucursal inexistente
                TransportistaId = 1,
                TotalKms = 120.5m,
                MontoTotal = 2000.0m,
                MonedaId = 1,
                HoraSalida = new TimeSpan(9, 0, 0),
                EstadoId = 1,
                Activo = true,
                ColaboradoresId = new List<int> { 1, 2 }
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/viaje/CrearViaje/?usuarioCreaId=1", nuevoViaje);
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ViajeDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse!.Valido.Should().BeFalse();
            apiResponse.Mensaje.Should().Contain("Sucursal no existe en la base de datos");
        }

        [Fact]
        public async Task CrearViaje_DeberiaRetornarBadRequest_CuandoTransportistaNoExiste()
        {
            ManejoDistanciasServiceMock.SetupExitosoCalcularDistanciaTotalAjustadaAsync(_manejoDistanciasService);
            // Arrange
            var nuevoViaje = new
            {
                FechaViaje = DateTime.UtcNow,
                SucursalId = 1,
                TransportistaId = 999, // Transportista inexistente
                TotalKms = 120.5m,
                MontoTotal = 2000.0m,
                MonedaId = 1,
                HoraSalida = new TimeSpan(9, 0, 0),
                EstadoId = 1,
                Activo = true,
                ColaboradoresId = new List<int> { 1, 2 }
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/viaje/CrearViaje/?usuarioCreaId=1", nuevoViaje);
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ViajeDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse!.Valido.Should().BeFalse();
            apiResponse.Mensaje.Should().Contain("Transportista no existe en la base de datos");
        }

        [Fact]
        public async Task CrearViaje_DeberiaRetornarBadRequest_CuandoColaboradorYaTieneViajeAsignado()
        {
            ManejoDistanciasServiceMock.SetupExitosoCalcularDistanciaTotalAjustadaAsync(_manejoDistanciasService);
            // Arrange
            var nuevoViaje = new
            {
                FechaViaje = DateTime.UtcNow,
                SucursalId = 1,
                TransportistaId = 1,
                TotalKms = 25.1m,
                MontoTotal = 2000.0m,
                MonedaId = 1,
                HoraSalida = new TimeSpan(6, 0, 0),
                EstadoId = 1,
                Activo = true,
                ColaboradoresId = new List<int> { 3 } // Colaborador ya tiene un viaje
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/viaje/CrearViaje/?usuarioCreaId=1", nuevoViaje);
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<ViajeDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse!.Valido.Should().BeFalse();
            apiResponse.Mensaje.Should().Contain("Algunos Colaboradores ya tiene un viaje asignado el dia de hoy.");
        }
    }
}

