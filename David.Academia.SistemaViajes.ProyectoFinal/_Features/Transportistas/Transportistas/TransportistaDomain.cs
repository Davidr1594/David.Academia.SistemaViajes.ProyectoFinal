using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas
{
    public class TransportistaDomain
    {
        public Respuesta<TransportistaDto> ValidarDatosDeEntrada(TransportistaDto transportistaDto)
        {
            var respuesta = new Respuesta<TransportistaDto>();

            if (transportistaDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(transportistaDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "nombre");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(transportistaDto.Apellido))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "apellido");
                return respuesta;
            }


            if (string.IsNullOrWhiteSpace(transportistaDto.Dni))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "DNI");
                return respuesta;
            }

            if (transportistaDto.Dni.Length > 20)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.CampoExcedeCaracteres, "dni", "20");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(transportistaDto.Telefono))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "teléfono");
                return respuesta;
            }

            if (transportistaDto.Telefono.Length > 20)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.CampoExcedeCaracteres, "telefono", "20");
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(transportistaDto.Email))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "correo electrónico");
                return respuesta;
            }

            if (!transportistaDto.Email.Contains("@"))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.CorreoNoValido;
                return respuesta;
            }

            if (transportistaDto.TarifaPorKm <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.ElCampoDebeSerMayorCero, "tarifa");
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = transportistaDto;
            return respuesta;
        }

    }
}
