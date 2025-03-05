using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios
{
    public class UsuarioDomain
    {

        public Respuesta<UsuarioDto> ValidarCreacionUsuario(UsuarioDto usuarioDto)
        {
            var respuesta = new Respuesta<UsuarioDto>();


            if (usuarioDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }
            if (string.IsNullOrWhiteSpace(usuarioDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido,"nombre");
                return respuesta;
            }
            if (string.IsNullOrWhiteSpace(usuarioDto.Clave))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "clave");
                return respuesta;
            }
            if (usuarioDto.RolId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "rol");
                return respuesta;
            }
            if (usuarioDto.ColaboradorId <= 0)
            {
                usuarioDto.ColaboradorId = null;
            }


            respuesta.Valido = true;
            respuesta.Datos = usuarioDto;

            return respuesta;
        }
    }
}
