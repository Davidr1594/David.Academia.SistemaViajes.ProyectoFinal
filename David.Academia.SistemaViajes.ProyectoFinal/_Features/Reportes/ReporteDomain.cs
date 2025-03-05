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
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "Transportista");
                return respuesta;
            }

            if (fechaInicio.Date == DateTime.MinValue || fechaFin.Date == DateTime.MinValue)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.FechaNoValida;
                return respuesta;
            }

            if (fechaInicio.Date > fechaFin.Date)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.FechaNoValida;
                return respuesta;
            }
            return respuesta;
        }
    }
}
