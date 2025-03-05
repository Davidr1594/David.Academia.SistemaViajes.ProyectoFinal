using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using FluentAssertions;


namespace David.Academia.SistemaViajes.UnitTests.Sucursales
{
    public class SucursalDomainTest
    {
        private SucursalDomain _sucursalDomain = new SucursalDomain();
        [Theory]
        [ClassData(typeof(SucursalDomainTestData))]
        public void Dado_UnCampo_Cuando_SeValida_EntoncesDebeRetornarResultado(SucursalDto sucursal, bool esperado)
        {
            
            //Act
            var respuesta = _sucursalDomain.ValidarCreacionSucursal(sucursal);
            //Assert
            respuesta.Valido.Should().Be(esperado);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(0, 1, false)]
        [InlineData(1, 0, false)]
        [InlineData(0, 0, false)]
        public void Dado_Un_Colaborador_Una_Sucursal_ValidarRelacion(int colaboradorId, int sucursalId, bool esperado)
        {
            var respuesta = _sucursalDomain.ValidarDatosDeEntradaRelacion(sucursalId, colaboradorId);

            respuesta.Valido.Should().Be(esperado);
        }


        [Theory]
        [ClassData(typeof(SucursalDomainValidarBdTestData))]
        public void ValidarDatosBDRelacion_DeberiaRetornarEsperado(Colaborador? colaborador, Sucursal? sucursal, bool resultadoEsperado)
        {
            // Act
            var resultado = _sucursalDomain.ValidarDatosBDRelacion(colaborador, sucursal);

            // Assert
            Assert.Equal(resultadoEsperado, resultado.Valido);
        }

    }
}
