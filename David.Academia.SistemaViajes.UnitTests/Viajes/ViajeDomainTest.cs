using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Enum;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using FluentAssertions;


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


        [Theory]
        [InlineData(5, 40, 50, true)]  //No excede el máximo
        [InlineData(20, 30, 50, true)]  //Límite exacto (30+20=50)
        [InlineData(25, 30, 50, false)] //Excede el máximo permitido (30+25=55)
        [InlineData(5, 45, 50, true)]   //Justo dentro del límite
        [InlineData(10, 50, 50, false)] //Exactamente en el límite no puede agregar más
        public void ValidarKmsDeViajeAlAgregarColaborador_DeberiaRetornarEsperado(decimal totalKmsViaje, decimal viajeTotalKms, decimal maximoKms, bool resultadoEsperado)
        {
            // Arrange
            var viaje = new Viaje { TotalKms = viajeTotalKms };

            // Act
            var resultado = _viajeDomain.ValidarKmsDeViajeAlAgregarColaborador(totalKmsViaje, viaje, maximoKms);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Theory]
        [InlineData(1, true, true, true)]  //Caso válido: Viaje existe, colaboradores asignados y válidos
        [InlineData(0, true, true, false)] //Viaje no existe
        [InlineData(1, false, true, false)] //No hay colaboradores con relacion
        [InlineData(1, true, false, false)] //No hay colaboradores válidos
        [InlineData(0, false, false, false)] //Ninguna condición válida
        public void ValidarRespuestasDeBDAgregarColaboradores_DeberiaRetornarEsperado(int viajeId, bool hayColaboradoresConKms, bool hayColaboradoresValidos, bool resultadoEsperado)
        {
            // Arrange
            var viaje = new Viaje { ViajeId = viajeId };
            var colaboradoresConKms = hayColaboradoresConKms ? new List<ColaboradorConKmsDto> { new ColaboradorConKmsDto() } : new List<ColaboradorConKmsDto>();
            var colaboradoresValidos = hayColaboradoresValidos ? new List<ColaboradorConKmsDto> { new ColaboradorConKmsDto() } : new List<ColaboradorConKmsDto>();

            // Act
            var resultado = _viajeDomain.ValidarRespuestasDeBDAgregarColaboradores(viaje, colaboradoresConKms, colaboradoresValidos);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Theory]
        [ClassData(typeof(ViajeDomainProcesarDatosTestData))]
        public void ProcesarDatosDeViajeAGuardar_DeberiaRetornarEsperado(Viaje viaje, int usuarioCreaId, decimal totalKms, Transportista transportista, bool resultadoEsperado)
        {
            // Act
            var resultado = _viajeDomain.ProcesarDatosDeViajeAGuardar(viaje, usuarioCreaId, totalKms, transportista);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
            resultado.Datos!.UsuarioCrea.Should().Be(usuarioCreaId);
            resultado.Datos.TotalKms.Should().Be(totalKms);
            resultado.Datos.MontoTotal.Should().Be(transportista.TarifaPorKm * totalKms);
            resultado.Datos.EstadoId.Should().Be((int)EstadoViajeEnum.Pendiente);
            resultado.Datos.Activo.Should().BeTrue();
        }

        [Fact]
        public void ProcesarDatosDeViajeAGuardar_DeberiaManejarTransportistaSinTarifa()
        {
            // Arrange
            var viaje = new Viaje { ViajeId = 1, TotalKms = 10 };
            var transportista = new Transportista { TransportistaId = 1, TarifaPorKm = 0 };

            // Act
            var resultado = _viajeDomain.ProcesarDatosDeViajeAGuardar(viaje, 1, 10, transportista);

            // Assert
            resultado.Valido.Should().BeTrue();
            resultado.Datos.MontoTotal.Should().Be(0);
        }

        [Fact]
        public void ProcesarDatosDetalleViajeAGuardar_DeberiaRetornarListaDeDetalles()
        {
            // Arrange
            var colaboradores = new List<ColaboradorConKmsDto>
            {
                new ColaboradorConKmsDto { ColaboradorId = 1, DireccionDestino = "Destino 1", DistanciaKms = 10 },
                new ColaboradorConKmsDto { ColaboradorId = 2, DireccionDestino = "Destino 2", DistanciaKms = 20 }
            };

            // Act
            var resultado = _viajeDomain.ProcesarDatosDetalleViajeAGuardar(colaboradores, 1);

            // Assert
            resultado.Valido.Should().BeTrue();
            resultado.Datos.Should().HaveCount(2);
            resultado.Datos[0].ViajeId.Should().Be(1);
            resultado.Datos[1].ColaboradorId.Should().Be(2);
        }

        [Fact]
        public void ProcesarDatosAgregarColaboradorAViaje_DeberiaAgregarColaboradores()
        {
            // Arrange
            var colaboradores = new List<ColaboradorConKmsDto>
            {
                new ColaboradorConKmsDto { ColaboradorId = 1, DireccionDestino = "Destino 1", DistanciaKms = 15 },
                new ColaboradorConKmsDto { ColaboradorId = 2, DireccionDestino = "Destino 2", DistanciaKms = 25 }
            };

            // Act
            var resultado = _viajeDomain.ProcesarDatosAgregarColaboradorAViaje(colaboradores, 1);

            // Assert
            resultado.Valido.Should().BeTrue();
            resultado.Datos.Should().HaveCount(2);
            resultado.Datos[0].ColaboradorId.Should().Be(1);
            resultado.Datos[1].DireccionDestino.Should().Be("Destino 2");
        }


    }
}
