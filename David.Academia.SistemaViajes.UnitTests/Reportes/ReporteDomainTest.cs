using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas;
using David.Academia.SistemaViajes.UnitTests.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.NewFolder
{
    public class ReporteDomainTest
    {
        private  ReporteDomain _reporteDomain = new ReporteDomain();
        [Theory]
        [ClassData(typeof(ReporteDomainTestData))]
        public void ValidarDatosEntrada_DeberiaRetornarEsperado(int transportistaId, DateTime fechaInicio, DateTime fechaFin, bool resultadoEsperado)
        {
            // Act
            var resultado = _reporteDomain.validarDatosEntrada(transportistaId, fechaInicio, fechaFin);

            // Assert
            Assert.Equal(resultadoEsperado, resultado.Valido);
        }
    }
}
