using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Usuarios.UsuarioTest
{
    public class UsuarioDomainTest
    {
        private readonly UsuarioDomain _usuarioDomain = new();

        [Theory]
        [ClassData(typeof(UsuarioDoaminTestData))]
        public void ValidarCreacionUsuario_DeberiaRetornarEsperado(UsuarioDto usuarioDto, bool resultadoEsperado)
        {
            // Act
            var resultado = _usuarioDomain.ValidarCreacionUsuario(usuarioDto);

            // Assert
            Assert.Equal(resultadoEsperado, resultado.Valido);
        }
    }
}
