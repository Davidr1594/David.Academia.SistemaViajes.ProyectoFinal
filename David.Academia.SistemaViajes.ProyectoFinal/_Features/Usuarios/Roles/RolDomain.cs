using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles
{
    public class RolDomain
    {
        public Respuesta<RolDto> ValidarDatosDeEntrada(RolDto rolDto)
        {
            var respuesta = new Respuesta<RolDto>();

            if (rolDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(rolDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = rolDto;
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
