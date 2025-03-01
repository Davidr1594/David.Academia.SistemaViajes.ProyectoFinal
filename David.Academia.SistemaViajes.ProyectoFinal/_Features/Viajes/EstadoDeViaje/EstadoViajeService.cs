using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.EstadoDeViaje
{
    public class EstadoViajeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoViaje> _estadoViajeRepository;
        private readonly IMapper _mapper;

        public EstadoViajeService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoViajeRepository = _unitOfWork.Repository<EstadoViaje>();
            _mapper = mapper;
        }

        public async Task<Respuesta<EstadoViajeDto>> CrearEstadoViaje(EstadoViajeDto estadoViajeDto)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();

            if (estadoViajeDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un estado de viaje valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(estadoViajeDto.Nombre) || string.IsNullOrWhiteSpace(estadoViajeDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del estado de viaje es requerido.";
                return respuesta;
            }

            try
            {
                if (await _estadoViajeRepository.AsQueryable().AnyAsync(ev => ev.Nombre.ToLower() == estadoViajeDto.Nombre.ToLower()))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ya existe un estado de viaje con este nombre.";
                    return respuesta;
                }

                var estadoViaje = _mapper.Map<EstadoViaje>(estadoViajeDto);

                await _estadoViajeRepository.AddAsync(estadoViaje);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoViajeDto>(estadoViaje);
                respuesta.Mensaje = "Estado de viaje creado con éxito.";
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
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
                    respuesta.Mensaje = "No hay estados de viaje";
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
                respuesta.Mensaje = "Error al conectar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<Respuesta<EstadoViajeDto>> ObtenerEstadoViajePorId(int estadoViaje)
        {
            var respuesta = new Respuesta<EstadoViajeDto>();
            try
            {
                var estadoViajEncontrado = await _estadoViajeRepository.AsQueryable().FirstOrDefaultAsync(ev => ev.EstadoViajeId == estadoViaje);

                if (estadoViajEncontrado == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Estado de viaje no encontrado.";
                }
                var estadoViajeDto = _mapper.Map<EstadoViajeDto>(estadoViajEncontrado);

                respuesta.Datos = estadoViajeDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al guardar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
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
                    respuesta.Mensaje = "Estado de viaje no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoViaje, estadoViajeEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Estado de viaje actualizado con exito";
                respuesta.Datos = _mapper.Map<EstadoViajeDto>(estadoViajeEncontrado);
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al actualizar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
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
                    respuesta.Mensaje = "Estado de viaje no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Estado de viaje ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Estado de viaje ha sido inactivado.";
                }

                estadoViajeEncontrado.Activo = estado;
                respuesta.Datos = true;

                await _unitOfWork.SaveChangesAsync();


            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al actualizar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }
    }
}
