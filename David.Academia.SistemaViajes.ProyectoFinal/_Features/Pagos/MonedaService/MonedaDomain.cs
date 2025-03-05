using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService
{
    public class MonedaDomain
    {
        public Respuesta<MonedaDto> ValidarDatosDeEntrada(MonedaDto monedaDto)
        {
            var respuesta = new Respuesta<MonedaDto>();

            if (monedaDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(monedaDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(monedaDto.CodigoIso))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "código ISO");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(monedaDto.Simbolo))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "símbolo");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = monedaDto;
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
