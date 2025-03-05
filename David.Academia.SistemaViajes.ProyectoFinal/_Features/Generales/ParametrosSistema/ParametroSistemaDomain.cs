using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema
{
    public class ParametroSistemaDomain
    {
        public Respuesta<ParametroSistemaDto> ValidarDatosDeEntrada(ParametroSistemaDto parametroDto)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();

            if (parametroDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(parametroDto.Descripcion))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "descripción");
                return respuesta;
            }

            if (parametroDto.Valor == null && string.IsNullOrWhiteSpace(parametroDto.ValorString))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "valor 2");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = parametroDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteDescripcion)
        {
            var respuesta = new Respuesta<bool>();

            if (yaExisteDescripcion)
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
