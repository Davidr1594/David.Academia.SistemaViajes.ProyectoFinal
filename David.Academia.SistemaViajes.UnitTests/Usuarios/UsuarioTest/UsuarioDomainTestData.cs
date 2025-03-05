using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Usuarios.UsuarioTest
{
    internal class UsuarioDoaminTestData : TheoryData<UsuarioDto, bool>
    {
        public UsuarioDoaminTestData()
        {
            Add(UsuarioNulo(), false);
            Add(UsuarioNombreVacio(), false);
            Add(UsuarioClaveVacia(), false);
            Add(UsuarioRolInvalido(), false);
            Add(UsuarioColaboradorInvalido(), true);
            Add(UsuarioTodosLosCamposValidos(), true);
        }

        private static UsuarioDto UsuarioNulo() => null;

        private static UsuarioDto UsuarioNombreVacio() => new UsuarioDto
        {
            Nombre = "",
            Clave = "password123",
            RolId = 1,
            ColaboradorId = 1
        };

        private static UsuarioDto UsuarioClaveVacia() => new UsuarioDto
        {
            Nombre = "UsuarioTest",
            Clave = "",
            RolId = 1,
            ColaboradorId = 1
        };

        private static UsuarioDto UsuarioRolInvalido() => new UsuarioDto
        {
            Nombre = "UsuarioTest",
            Clave = "password123",
            RolId = 0, 
            ColaboradorId = 1
        };

        private static UsuarioDto UsuarioColaboradorInvalido() => new UsuarioDto
        {
            Nombre = "UsuarioTest",
            Clave = "password123",
            RolId = 1,
            ColaboradorId = 0 
        };

        private static UsuarioDto UsuarioTodosLosCamposValidos() => new UsuarioDto
        {
            Nombre = "UsuarioTest",
            Clave = "password123",
            RolId = 1,
            ColaboradorId = 1
        };
    }
}
