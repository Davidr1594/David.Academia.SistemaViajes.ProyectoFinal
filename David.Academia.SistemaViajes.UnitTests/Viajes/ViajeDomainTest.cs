using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Viajes
{
    public class ViajeDomainTest
    {
        private ViajeDomain _viajeDomain = new ViajeDomain();

        [Theory]
        [ClassData(typeof(ViajeDomainValidarDatosDeEntradaTestData))]
        public void ValidarDatosDeEntradaViaje_DeberiaRetornarEsperado(Viaje viaje, List<int> colaboradoresId, bool resultadoEsperado)
        {
            // Act
            var resultado = _viajeDomain.ValidarDatosDeEntradaViaje(viaje, colaboradoresId);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Theory]
        [ClassData(typeof(ViajeDomainValidarKmsColaborador))]
        public void ValidarKmsDeColaborador_DeberiaRetornarEsperado(List<ColaboradorConKmsDto> colaboradoresDetalle, decimal maximoKms, bool resultadoEsperado)
        {
            // Act
            var resultado = _viajeDomain.ValidarKmsDeColaborador(colaboradoresDetalle, maximoKms);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Theory]
        [ClassData(typeof(ViajeDomainValidarBdTestData))]
        public void ValidarRespuestasDeBD_DeberiaRetornarEsperado(Sucursal sucursal, Usuario usuario, Transportista transportista, List<ColaboradorConKmsDto> colaboradoresDetalle, List<int> colaboradoresEnViaje, bool resultadoEsperado)
        {
            // Act
            var resultado = _viajeDomain.ValidarRespuestasDeBD(sucursal, usuario, transportista, colaboradoresDetalle, colaboradoresEnViaje);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }
    }
}
