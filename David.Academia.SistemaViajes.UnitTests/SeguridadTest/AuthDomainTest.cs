using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.SeguridadTest
{
    public class AuthDomainTest
    {
        private AuthDomain _authDomain = new AuthDomain();

        [Theory]
        [ClassData(typeof(AutoDomainTestData))]
        public void ValidarCredencialesValidas_DeberiaRetornarEsperado(UsuarioAuthDto usuario, bool resultadoEsperado)
        {
            // Act
            var resultado = _authDomain.ValidarCredencialesValidas(usuario);

            // Assert
            Assert.Equal(resultadoEsperado, resultado.Datos);
        }

        [Fact]
        public void EncriptarClave_DeberiaRetornarHash()
        {
            // Arrange
            string clave = "password123";

            // Act
            byte[] hash = _authDomain.EncriptarClave(clave);

            // Assert

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.Equal(32, hash.Length); 
        }
    }
}
