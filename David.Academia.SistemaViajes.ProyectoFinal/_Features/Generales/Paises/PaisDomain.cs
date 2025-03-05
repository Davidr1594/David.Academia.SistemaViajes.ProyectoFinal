using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises
{
    public class PaisDomain
    {
        public Respuesta<PaisDto> ValidarDatosDeEntrada(PaisDto paisDto)
        {
            var respuesta = new Respuesta<PaisDto>();

            if (paisDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(paisDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(paisDto.Prefijo))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "prefijo");
                return respuesta;
            }

            if (paisDto.MonedaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "moneda");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = paisDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteNombre, bool existeMoneda)
        {
            var respuesta = new Respuesta<bool>();

            if (yaExisteNombre)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            if (!existeMoneda)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "La moneda");
                return respuesta;
            }

            respuesta.Valido = true;
            return respuesta;
        }
    }
}
