using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos
{
    public class PuestoDomain
    {
        public Respuesta<PuestoDto> ValidarDatosDeEntrada(PuestoDto puestoDto)
        {
            var respuesta = new Respuesta<PuestoDto>();

            if (puestoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(puestoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }


            if (string.IsNullOrWhiteSpace(puestoDto.Descripcion))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "descripción");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = puestoDto;
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

