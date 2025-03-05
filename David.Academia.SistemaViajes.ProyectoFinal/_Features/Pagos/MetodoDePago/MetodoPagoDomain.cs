using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago
{
    public class MetodoPagoDomain
    {
        public Respuesta<MetodoPagoDto> ValidarDatosDeEntrada(MetodoPagoDto metodoPagoDto)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();

            if (metodoPagoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(metodoPagoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = metodoPagoDto;
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
