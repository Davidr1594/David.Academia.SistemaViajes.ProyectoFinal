using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests._Common.GoogleMaps
{
    public class ManejoDistanciaServiceTest
    {
        private  ManejoDistanciasService _manejoDistancia = new ManejoDistanciasService();

        [Theory]
        [InlineData(-91, 0)]
        [InlineData(91, 0)]  
        [InlineData(0, -181)] 
        [InlineData(0, 181)] 
        public async Task ObtenerDireccionDesdeCordenadas_DeberiaLanzarExcepcionSiCoordenadasInvalidas(decimal latitud, decimal longitud)
        {

            //Act
            var respuesta = _manejoDistancia.ObtenerDireccionDesdeCordenadas(latitud, longitud);

            //Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => respuesta);
        }

        [Theory]
        [InlineData(-91, 0, 0, 0)]  // Latitud origen fuera de rango
        [InlineData(0, -181, 0, 0)] // Longitud origen fuera de rango
        [InlineData(0, 0, 91, 0)]   // Latitud destino fuera de rango
        [InlineData(0, 0, 0, 181)]  // Longitud destino fuera de rango
        public async Task ObtenerDistanciaEntreSucursalColaborador_DeberiaLanzarExcepcionSiCoordenadasInvalidas(decimal latOrigen, decimal lonOrigen, decimal latDestino, decimal lonDestino)
        {
            //Act
            var respuesta = _manejoDistancia.ObtenerDistanciaEntreSucursalColaborador(latOrigen,lonOrigen,latDestino,lonDestino);

            //Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => respuesta);
        }
    }
}
