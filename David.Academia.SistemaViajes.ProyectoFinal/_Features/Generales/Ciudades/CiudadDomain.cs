using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Ciudades.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Ciudades
{
    public class CiudadDomain
    {
        public Respuesta<CiudadDto> ValidarDatosDeEntrada(CiudadDto ciudadDto)
        {
            var respuesta = new Respuesta<CiudadDto>();

            if (ciudadDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(ciudadDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "ciudad");
                return respuesta;
            }

            if (ciudadDto.EstadoId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "Estado");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = ciudadDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteNombre, bool existeEstado)
        {
            var respuesta = new Respuesta<bool>();

            if (yaExisteNombre)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            if (!existeEstado)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "El estado");
                return respuesta;
            }

            respuesta.Valido = true;
            return respuesta;
        }
    }
}
