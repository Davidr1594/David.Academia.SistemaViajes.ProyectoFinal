using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Estado_Departamento
{
    public class EstadoDepartamentoDomain
    {
        public Respuesta<EstadoDepartamentoDto> ValidarDatosDeEntrada(EstadoDepartamentoDto estadoDepartamentoDto)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();

            if (estadoDepartamentoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(estadoDepartamentoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            if (estadoDepartamentoDto.PaisId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "país");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = estadoDepartamentoDto;
            return respuesta;
        }

        public Respuesta<bool> ValidarRespuestaDeBD(bool yaExisteNombre, bool existePais)
        {
            var respuesta = new Respuesta<bool>();

            if (yaExisteNombre)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            if (!existePais)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "El país");
                return respuesta;
            }

            respuesta.Valido = true;
            return respuesta;
        }
    }
}
