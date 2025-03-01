using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores
{
    public class ColaboradorDomain
    {
        public Respuesta<ColaboradorDto> ValidarCreacionColaborador(ColaboradorDto colaboradorDto)
        {
            var respuesta = new Respuesta<ColaboradorDto>();

            if (colaboradorDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un colaborador válido.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del colaborador es requerido.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Apellido))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El apellido del colaborador es requerido.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Dni))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El DNI del colaborador es requerido.";
                return respuesta;
            }

            if (colaboradorDto.FechaNacimiento == default)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La fecha de nacimiento es requerida.";
                return respuesta;
            }

            if (colaboradorDto.FechaNacimiento > DateTime.Now.AddYears(-18))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El colaborador debe ser mayor de edad.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Telefono))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El número de teléfono es requerido.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Email))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El correo electrónico es requerido.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(colaboradorDto.Direccion))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La dirección es requerida.";
                return respuesta;
            }

            if (colaboradorDto.Latitud < -90 || colaboradorDto.Latitud > 90)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La latitud debe estar entre -90 y 90 grados.";
                return respuesta;
            }

            if (colaboradorDto.Longitud < -180 || colaboradorDto.Longitud > 180)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La longitud debe estar entre -180 y 180 grados.";
                return respuesta;
            }

            if (colaboradorDto.PuestoId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El puesto del colaborador es requerido.";
                return respuesta;
            }

            if (colaboradorDto.CiudadId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La ciudad del colaborador es requerida.";
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = colaboradorDto;
            return respuesta;
        }
    }
}
