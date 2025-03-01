using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes
{
    public class ReporteDomain
    {
        public Respuesta<bool> validarDatosEntrada(int transportistaId, DateTime fechaInicio, DateTime fechaFin)
        {
            var respuesta = new Respuesta<bool>();

            if (transportistaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El ID del transportista no es válido.";
                return respuesta;
            }

            if (fechaInicio.Date == DateTime.MinValue || fechaFin.Date == DateTime.MinValue)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Las fechas no pueden estar vacías.";
                return respuesta;
            }

            if (fechaInicio.Date > fechaFin.Date)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                return respuesta;
            }
            return respuesta;
        }
    }
}
