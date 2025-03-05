using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago
{
    public class EstadoPagoDomain
    {
        public Respuesta<EstadoPagoDto> ValidarDatosDeEntrada(EstadoPagoDto estadoPagoDto)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();

            if (estadoPagoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(estadoPagoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = estadoPagoDto;
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
