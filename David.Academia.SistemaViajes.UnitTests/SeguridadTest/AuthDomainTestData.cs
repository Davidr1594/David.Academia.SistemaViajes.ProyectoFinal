using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.SeguridadTest
{
    internal class AutoDomainTestData : TheoryData<UsuarioAuthDto, bool>
    {
        public AutoDomainTestData()
        {
            Add(new UsuarioAuthDto { nombre = "", clave = "password123" }, false); // ❌ Nombre vacío
            Add(new UsuarioAuthDto { nombre = "UsuarioTest", clave = "" }, false); // ❌ Clave vacía
            Add(new UsuarioAuthDto { nombre = "", clave = "" }, false); // ❌ Ambos vacíos
            Add(new UsuarioAuthDto { nombre = "UsuarioTest", clave = "password123" }, true); // ✅ Datos válidos
        }
    }
}