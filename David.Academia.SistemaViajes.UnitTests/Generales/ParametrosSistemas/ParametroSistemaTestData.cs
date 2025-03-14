using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Generales.ParametrosSistemas
{
    internal class ParametroSistemaTestData : TheoryData<ParametroSistemaDto, bool>
    {
        public ParametroSistemaTestData()
        {
            Add(ParametroNulo(), false);
            Add(ParametroDescripcionVacia(), false);
            Add(ParametroValorNulo(), false);
            Add(ParametroValorStringVacio(), false);
            Add(ParametroTodosLosCamposValidos(), true);
        }

        private static ParametroSistemaDto ParametroNulo() => null;

        private static ParametroSistemaDto ParametroDescripcionVacia() => new ParametroSistemaDto
        {
            Descripcion = "",
            Valor = 10,
            ValorString = "Valor"
        };

        private static ParametroSistemaDto ParametroValorNulo() => new ParametroSistemaDto
        {
            Descripcion = "Parámetro válido",
            Valor = null,
            ValorString = null
        };

        private static ParametroSistemaDto ParametroValorStringVacio() => new ParametroSistemaDto
        {
            Descripcion = "Parámetro válido",
            Valor = null,
            ValorString = ""
        };

        private static ParametroSistemaDto ParametroTodosLosCamposValidos() => new ParametroSistemaDto
        {
            Descripcion = "Parámetro válido",
            Valor = 10,
            ValorString = "Texto"
        };
    }
}
