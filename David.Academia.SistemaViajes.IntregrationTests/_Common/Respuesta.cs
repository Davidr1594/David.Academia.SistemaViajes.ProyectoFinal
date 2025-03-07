using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.ProyectoFinal.IntegrationTests._Common
{
    public class Respuesta<T>
    {
        public bool Valido { get; set; }
        public string Mensaje { get; set; }
        public string? DetalleError { get; set; }
        public T? Datos { get; set; }

        public Respuesta()
        {
            Valido = true;
            Mensaje = string.Empty;

        }
    }
}
