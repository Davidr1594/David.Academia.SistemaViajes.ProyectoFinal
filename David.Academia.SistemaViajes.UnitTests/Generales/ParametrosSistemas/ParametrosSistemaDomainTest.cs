using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace David.Academia.SistemaViajes.UnitTests.Generales.ParametrosSistemas
{
    public class ParametroSistemaDomainTest
    {
        private readonly ParametroSistemaDomain _parametroSistemaDomain = new();

        [Theory]
        [ClassData(typeof(ParametroSistemaTestData))]
        public void Dado_UnParametro_Cuando_SeValida_EntoncesDebeRetornarResultado(ParametroSistemaDto parametroDto, bool esperado)
        {
            // Act
            var respuesta = _parametroSistemaDomain.ValidarDatosDeEntrada(parametroDto);

            // Assert
            respuesta.Valido.Should().Be(esperado);
        }

        [Theory]
        [InlineData(true, false)]  // ❌ Ya existe la descripción
        [InlineData(false, true)]  // ✅ No existe la descripción
        public void ValidarRespuestaDeBD_DeberiaRetornarEsperado(bool yaExisteDescripcion, bool resultadoEsperado)
        {
            // Act
            var resultado = _parametroSistemaDomain.ValidarRespuestaDeBD(yaExisteDescripcion);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Fact]
        public void Dado_UnObjetoNulo_Cuando_SeValida_EntoncesDebeRetornarResultadoFalso()
        {
            // Act
            var respuesta = _parametroSistemaDomain.ValidarDatosDeEntrada(null);

            // Assert
            respuesta.Valido.Should().Be(false);
        }
    }
}