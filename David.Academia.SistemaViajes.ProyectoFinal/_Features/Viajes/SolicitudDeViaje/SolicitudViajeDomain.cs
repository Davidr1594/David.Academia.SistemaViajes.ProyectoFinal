
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje
{
    public class SolicitudViajeDomain
    {
        public Respuesta<bool>ValidarDatosDeEntrada(int colaboradorId, int sucursalId, string comentario)
        {
            var respuesta = new Respuesta<bool>();
            if (colaboradorId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "colaborador");
                return respuesta;
            }
            if (sucursalId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "sucursal");
                return respuesta;
            }
            if (string.IsNullOrWhiteSpace(comentario))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "comentario");
                return respuesta;
            }
            if (comentario.Length > 100)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.TextoExcedeLimite, "100");
                return respuesta;
            }
            respuesta.Valido = true;
            respuesta.Datos = true;
            return respuesta;
        }

        public Respuesta<SolicitudViaje> ProcesarDatosViajeSolicitud(int colaboradorId, int sucursalId, string comentario)
        {
            var respuesta = new Respuesta<SolicitudViaje>();

            var solicitudViaje = new SolicitudViaje()
            {
                ColaboradorId = colaboradorId,
                SucursalId = sucursalId,
                FechaSolicitud = DateTime.Now,
                Comentario = comentario,
            };


            respuesta.Valido = true;
            respuesta.Datos = solicitudViaje;
            return respuesta;
        }
    }
}
