using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.ColaboradorTest
{
  
    public class ColaboradorDomainTest
    {
        private ColaboradorDomain _colaboradorDomain = new ColaboradorDomain();

        [Theory]
        [ClassData(typeof(ColaboradorDtoTestData))]
        public void Dado_UnCampo_Cuando_SeValida_EntoncesDebeRetornarResultado(ColaboradorDto colaborador, bool esperado)
        { 
            //Act
            var respuesta = _colaboradorDomain.ValidarDatosDeEntrada(colaborador);

            //Assert
            respuesta.Valido.Should().Be(esperado);
        }


        [Theory]
        [InlineData(false, false, true, true, true)]  //Todos los valores correctos
        [InlineData(true, false, true, true, false)] //Ya existe el correo
        [InlineData(false, false, false, true, false)] //No existe el puesto
        [InlineData(false, false, true, false, false)] //No existe la ciudad
        [InlineData(true, true, true, true, false)] //Correo y nombre ya existen
        [InlineData(false, false, false, false, false)] //Puesto y ciudad no existen
        [InlineData(false, true, false, false, false)] // Ya existe el nombre pero los demás son falsos
        public void ValidarRespuestaDeBD_DeberiaRetornarEsperado(bool yaExisteCorreo, bool yaExisteNombre, bool existePuesto, bool existeCiudad, bool resultadoEsperado)
        {
            // Act
            var resultado = _colaboradorDomain.ValidarRespuestaDeBD(yaExisteCorreo, yaExisteNombre, existePuesto, existeCiudad);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }


    }
}
