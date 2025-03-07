using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores
{
    public class ColaboradorDomain
    {
        public Respuesta<ColaboradorDto> ValidarDatosDeEntrada(ColaboradorDto colaboradorDto)
        {
            var respuesta = new Respuesta<ColaboradorDto>();

            if (colaboradorDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Apellido))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "apellido");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Dni))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "dni");
                return respuesta;
            }

            if (colaboradorDto.FechaNacimiento == default)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "Fecha nacimiento");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Telefono))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "teléfono");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Email))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "correo");
                return respuesta;
            }
            if (!colaboradorDto.Email.Contains("@"))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.CorreoNoValido;
                return respuesta;
            }
            if (colaboradorDto.Latitud < -90 || colaboradorDto.Latitud > 90)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.LatitudNoValida;
                return respuesta;
            }

            if (colaboradorDto.Longitud < -180 || colaboradorDto.Longitud > 180)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.LongitudNoValida;
                return respuesta;
            }

            if (colaboradorDto.PuestoId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "puesto");
                return respuesta;
            }

            if (colaboradorDto.CiudadId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "ciudad");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = colaboradorDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteCorreo, bool yaExisteNombre, bool existePuesto, bool existeCiudad )
        { 
            var respuesta = new Respuesta<bool>();

            if (yaExisteCorreo)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteCorreo;
                return respuesta;
            }
            if (!existePuesto)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "El puesto");
                return respuesta;
            }
            if (!existeCiudad)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "La ciudad");
                return respuesta;
            }

            return respuesta;
        }
    }
}
