using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.SolicitudDeViaje
{
    public class SolicitudViajeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SolicitudViajeDomain _solicitudViajeDomain;


        public SolicitudViajeService(IUnitOfWork unitOfWork, SolicitudViajeDomain solicitudViajeDomain, IMapper mapper)
        { 
            _unitOfWork = unitOfWork;
            _solicitudViajeDomain = solicitudViajeDomain;
            _mapper = mapper;
        }



        public async Task<Respuesta<SolicitudViaje>> CrearSolicitudViaje(int colaboradorId, int sucursalId, string comentario)
        { 
            var respuesta = new Respuesta<SolicitudViaje>();

            var respuestaValidarDatosEntrada = _solicitudViajeDomain.ValidarDatosDeEntrada(colaboradorId, sucursalId, comentario);
            if (!respuestaValidarDatosEntrada.Valido)
            { 
                respuesta.Valido = respuestaValidarDatosEntrada.Valido;
                respuesta.Mensaje = respuestaValidarDatosEntrada.Mensaje;
                return respuesta;
            }

            var respuestaProcesarDatosSolictudViaje = _solicitudViajeDomain.ProcesarDatosViajeSolicitud(colaboradorId, sucursalId, comentario);
            var solicitudViajeAGuardar = respuestaProcesarDatosSolictudViaje.Datos;
            if (solicitudViajeAGuardar == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                return respuesta;
            }
            try
            {

               await _unitOfWork.Repository<SolicitudViaje>().AddAsync(solicitudViajeAGuardar);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<SolicitudViajeDto>>> ObtenerSolicitudesViaje()
        {
            var respuesta = new Respuesta<List<SolicitudViajeDto>>();
            try
            {
                var SolicitudViajes = await _unitOfWork.Repository<SolicitudViaje>().AsQueryable().AsNoTracking().ToListAsync();

                if (SolicitudViajes.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var solicitudViajesDto = new List<SolicitudViajeDto>();

                foreach (var viaje in SolicitudViajes)
                {
                    solicitudViajesDto.Add(_mapper.Map<SolicitudViajeDto>(viaje));
                }

                respuesta.Datos = solicitudViajesDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }
    }
}
