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
        [Theory]
        [ClassData(typeof(ColaboradorDtoTestData))]
        public void Dado_UnCampo_Cuando_SeValida_EntoncesDebeRetornarResultado(ColaboradorDto colaborador, bool esperado)
        { 
            var colaboradorDomain = new ColaboradorDomain();

            var respuesta = colaboradorDomain.ValidarDatosDeEntrada(colaborador);

            respuesta.Valido.Should().Be(esperado);
        }


    }
}
