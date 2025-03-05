using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Reportes
{
    internal class ReporteDomainTestData : TheoryData<int, DateTime, DateTime, bool>
    {
        public ReporteDomainTestData()
        {
            Add(0, DateTime.Today, DateTime.Today.AddDays(1), false); //transportistaId inválido
            Add(1, DateTime.MinValue, DateTime.Today.AddDays(1), false); //FechaInicio no válida
            Add(1, DateTime.Today, DateTime.MinValue, false); //fechaFin no válida
            Add(1, DateTime.Today.AddDays(1), DateTime.Today, false); //fechaInicio después de fechaFin
            Add(1, DateTime.Today, DateTime.Today.AddDays(1), true); //Datos válidos
        }
    }
}

