using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles.Dto;
using FluentAssertions;


namespace David.Academia.SistemaViajes.UnitTests.Usuarios.RolTest
{
    public class RolDomainTest
    {

        [Theory]
        [InlineData("Nombre", true)]
        [InlineData("", false)]
        public void Dado_UnNombre_Cuando_SeValida_EntoncesDebeRetornarResultado(string nombre, bool esperado)
        {
            //Arrange
            var rolDto = new RolDto();
            rolDto.Nombre = nombre;

            var rolDomain = new RolDomain();

            //Act
            var respuesta = rolDomain.ValidarDatosDeEntrada(rolDto);

            //Asssert

            respuesta.Valido.Should().Be(esperado);
        }


    }
}
