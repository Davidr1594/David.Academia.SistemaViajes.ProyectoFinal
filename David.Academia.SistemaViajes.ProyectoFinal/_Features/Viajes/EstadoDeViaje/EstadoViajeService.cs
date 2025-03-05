using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje
{
    public class EstadoViajeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoViaje> _estadoViajeRepository;
        private readonly EstadoViajeDomain _estadoViajeDomain;
        private readonly IMapper _mapper;

        public EstadoViajeService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, EstadoViajeDomain estadoViajeDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoViajeRepository = _unitOfWork.Repository<EstadoViaje>();
            _mapper = mapper;
            _estadoViajeDomain = estadoViajeDomain; 
        }

        public async Task<Respuesta<EstadoViajeDto>> CrearEstadoViaje(EstadoViajeDto estadoViajeDto)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();

            var respuestaValidarEntrada = _estadoViajeDomain.ValidarDatosDeEntrada(estadoViajeDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _estadoViajeRepository.AsQueryable()
                                                              .AnyAsync(ev => ev.Nombre.ToLower() == estadoViajeDto.Nombre.ToLower());

            var respuestaValidarBD = _estadoViajeDomain.ValidarRespuestaDeBD(yaExisteNombre);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var estadoViaje = _mapper.Map<EstadoViaje>(estadoViajeDto);

                await _estadoViajeRepository.AddAsync(estadoViaje);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoViajeDto>(estadoViaje);
                respuesta.Mensaje = Mensajes.EntidadGuardada;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }


        public async Task<Respuesta<List<EstadoViajeDto>>> ObtenerEstadosViajes()
        {
            var respuesta = new Respuesta<List<EstadoViajeDto>>();
            try
            {
                var estadoViajes = await _estadoViajeRepository.AsQueryable().ToListAsync();

                if (estadoViajes.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var estadoViajeDto = new List<EstadoViajeDto>();

                foreach (var estadoViaje in estadoViajes)
                {
                    estadoViajeDto.Add(_mapper.Map<EstadoViajeDto>(estadoViaje));
                }

                respuesta.Datos = estadoViajeDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<Respuesta<EstadoViajeDto>> ObtenerEstadoViajePorId(int estadoViaje)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();
            try
            {
                var estadoViajeEncontrado = await _estadoViajeRepository.AsQueryable().FirstOrDefaultAsync(ev => ev.EstadoViajeId == estadoViaje);

                if (estadoViajeEncontrado == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var estadoViajeDto = _mapper.Map<EstadoViajeDto>(estadoViajeEncontrado);

                respuesta.Datos = estadoViajeDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<Respuesta<EstadoViajeDto>> ActualizarEstadoViaje(EstadoViajeDto estadoViaje)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();
            try
            {
                var estadoViajeEncontrado = await _estadoViajeRepository.AsQueryable().FirstOrDefaultAsync(ev => ev.EstadoViajeId == estadoViaje.EstadoViajeId);

                if (estadoViajeEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Estado de viaje no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoViaje, estadoViajeEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<EstadoViajeDto>(estadoViajeEncontrado);
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<Respuesta<bool>> EstadoDeEstadoViaje(int estadoViajeId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var estadoViajeEncontrado = await _estadoViajeRepository.AsQueryable().FirstOrDefaultAsync(ev => ev.EstadoViajeId == estadoViajeId);

                if (estadoViajeEncontrado == null)
                {
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadActivada;
                }
                else
                {
                    respuesta.Mensaje = Mensajes.EntidadInactivada;
                }

                estadoViajeEncontrado.Activo = estado;
                respuesta.Datos = true;

                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}
