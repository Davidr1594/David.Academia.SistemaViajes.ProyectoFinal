using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using System.Text.RegularExpressions;

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
                respuesta.Mensaje = "No se recibió una sucursal válida.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(sucursalDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre de la sucursal es requerido.";
                return respuesta;

            }

            if (sucursalDto.Latitud < -90 || sucursalDto.Latitud > 90)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La latitud debe estar entre -90 y 90 grados.";
                return respuesta;
            }

            if (sucursalDto.Longitud < -180 || sucursalDto.Longitud > 180)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "La longitud debe estar entre -180 y 180 grados.";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(sucursalDto.Telefono))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El campo numero telefono es requerido";
                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = sucursalDto;
            return respuesta;
        }
    }
}
