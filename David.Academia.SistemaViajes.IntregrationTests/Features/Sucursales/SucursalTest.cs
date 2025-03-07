using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests._Common;

namespace David.Academia.SistemaViajes.IntregrationTests.Features.Sucursales
{
    public class SucursalTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public SucursalTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;

        }

        [Fact]
        public async Task ObtenerSucursales_DeberiaRetornar200YListaDeColaboradores()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/Sucursal/ObtenerSucursal");

            // Assert
            var apiResponse = await response.Content.ReadFromJsonAsync<Respuesta<IEnumerable<Sucursal>>>();

            apiResponse.Should().NotBeNull();
            apiResponse.Valido.Should().BeTrue();
            apiResponse.Datos.Should().NotBeNullOrEmpty();
            apiResponse.Datos.Should().HaveCountGreaterThan(0);
        }
    }
}
