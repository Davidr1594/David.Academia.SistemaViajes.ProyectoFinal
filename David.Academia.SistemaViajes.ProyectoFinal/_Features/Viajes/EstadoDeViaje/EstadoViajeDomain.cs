using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje
{
    public class EstadoViajeDomain
    {
        public Respuesta<EstadoViajeDto> ValidarDatosDeEntrada(EstadoViajeDto estadoViajeDto)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();

            if (estadoViajeDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(estadoViajeDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = estadoViajeDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteNombre)
        {
            var respuesta = new Respuesta<bool>();

            if (yaExisteNombre)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            respuesta.Valido = true;
            return respuesta;
        }
    }
}
