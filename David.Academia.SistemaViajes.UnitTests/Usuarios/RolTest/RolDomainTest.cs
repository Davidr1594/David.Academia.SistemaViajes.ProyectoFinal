using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles.Dto;
using FluentAssertions;


namespace David.Academia.SistemaViajes.UnitTests.Usuarios.RolTest
{
    public class RolDomainTest
    {
        private RolDomain _rolDomain = new RolDomain();

        [Theory]
        [InlineData("Nombre", true)]
        [InlineData("", false)]
        public void Dado_UnNombre_Cuando_SeValida_EntoncesDebeRetornarResultado(string nombre, bool esperado)
        {
            //Arrange
            var rolDto = new RolDto();
            rolDto.Nombre = nombre;

            //Act
            var respuesta = _rolDomain.ValidarDatosDeEntrada(rolDto);

            //Asssert

            respuesta.Valido.Should().Be(esperado);
        }

        [Theory]
        [InlineData(true, false)]  // ❌ Ya existe el nombre
        [InlineData(false, true)]  // ✅ No existe el nombre
        public void ValidarRespuestaDeBD_DeberiaRetornarEsperado(bool yaExisteNombre, bool resultadoEsperado)
        {
            // Act
            var resultado = _rolDomain.ValidarRespuestaDeBD(yaExisteNombre);

            // Assert
            resultado.Valido.Should().Be(resultadoEsperado);
        }

        [Fact]
        public void Dado_UnObjeto_Cuando_SeValida_EntoncesDebeRetornarResultado()
        {

            //Act
            var respuesta = _rolDomain.ValidarDatosDeEntrada(null);

            //Asssert

            respuesta.Valido.Should().Be(false);
        }


    }
}
