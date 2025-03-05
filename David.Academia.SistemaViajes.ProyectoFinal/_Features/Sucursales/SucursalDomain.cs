using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
        public class SucursalDomain
        {
            public Respuesta<SucursalDto> ValidarCreacionSucursal(SucursalDto sucursalDto)
            {
                var respuesta = new Respuesta<SucursalDto>();

                if (sucursalDto == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalido);
                    return respuesta;
                }

                if (string.IsNullOrWhiteSpace(sucursalDto.Nombre))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "Nombre");
                    return respuesta;
                }

                if (sucursalDto.Latitud < -90 || sucursalDto.Latitud > 90)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.LatitudNoValida;
                    return respuesta;
                }

                if (sucursalDto.Longitud < -180 || sucursalDto.Longitud > 180)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.LongitudNoValida;
                    return respuesta;
                }

                if (string.IsNullOrWhiteSpace(sucursalDto.Telefono))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "Telefono");
                    return respuesta;
                }

                respuesta.Valido = true;
                respuesta.Datos = sucursalDto;
                return respuesta;
            }

            public Respuesta<bool> ValidarDatosDeEntradaRelacion(int sucursalId, int colaboradorId)
            {
                var respuesta = new Respuesta<bool>();

                if (colaboradorId <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "Colaborador");
                    return respuesta;
                }
                if (sucursalId <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.ElCampoEsRequerido, "Sucursal");
                    return respuesta;
                }

                respuesta.Valido = true;
                respuesta.Datos = true;
                return respuesta;
            }

            public Respuesta<bool> ValidarDatosBDRelacion(Colaborador? colaborador, Sucursal? sucursal)
            {
                var respuesta = new Respuesta<bool>();

                if (colaborador == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Colaborador");
                    return respuesta;
                }
                if (sucursal == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Colaborador");
                    return respuesta;
                }

                respuesta.Valido = true;
                respuesta.Datos = true;
                return respuesta;
            }


        }
}
