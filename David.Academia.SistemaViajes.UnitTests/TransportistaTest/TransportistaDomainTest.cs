using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace David.Academia.SistemaViajes.UnitTests.TransportistaTest
{
    public class TransportistaDomainTests
    {
        private readonly TransportistaDomain _transportistaDomain = new();

        [Theory]
        [ClassData(typeof(TransportistaDtoTestData))]
        public void ValidarDatosDeEntrada_DeberiaRetornarEsperado(TransportistaDto transportistaDto, bool esperado)
        {
            // Act
            var respuesta = _transportistaDomain.ValidarDatosDeEntrada(transportistaDto);

            // Assert
            respuesta.Valido.Should().Be(esperado);
        }
    }
}
