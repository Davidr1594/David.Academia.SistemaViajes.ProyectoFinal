using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago
{
    public class EstadoPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoPago> _estadoPagoRepository;
        private readonly IMapper _mapper;

        public EstadoPagoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoPagoRepository = _unitOfWork.Repository<EstadoPago>();
            _mapper = mapper;
        }

        public async Task<Respuesta<EstadoPagoDto>> CrearEstadoPago(EstadoPagoDto estadoPagoDto)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();

            if (estadoPagoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un estado de pago válido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(estadoPagoDto.Nombre) || string.IsNullOrWhiteSpace(estadoPagoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del estado de pago es requerido.";
                return respuesta;
            }


            try
            {
                if (await _estadoPagoRepository.AsQueryable().AnyAsync(ep => ep.Nombre.ToLower() == estadoPagoDto.Nombre.ToLower()))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ya existe un estado de pago con este nombre.";
                    return respuesta;
                }

                var estadoPago = _mapper.Map<EstadoPago>(estadoPagoDto);

                await _estadoPagoRepository.AddAsync(estadoPago);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoPagoDto>(estadoPago);
                respuesta.Mensaje = "Rol creado con éxito.";
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

        public async Task<Respuesta<List<EstadoPagoDto>>> ObtenerEstadosPagos()
        {
            var respuesta = new Respuesta<List<EstadoPagoDto>>();
            try
            {
                var estadosPagos = await _estadoPagoRepository.AsQueryable().ToListAsync();

                if (estadosPagos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Estados de pagos";
                    return respuesta;
                }

                var estadosPagosDto = new List<EstadoPagoDto>();

                foreach (var estadoPago in estadosPagos)
                {
                    estadosPagosDto.Add(_mapper.Map<EstadoPagoDto>(estadoPago));
                }

                respuesta.Datos = estadosPagosDto;

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

        public async Task<Respuesta<EstadoPagoDto>> ObtenerEstadoPagoPorId(int estadoPagoId)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();
            try
            {
                var estadoPago = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPagoId);

                if (estadoPago == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Estado de pago no encontrado.";
                }
                var rolDto = _mapper.Map<EstadoPagoDto>(estadoPago);

                respuesta.Datos = rolDto;
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

        public async Task<Respuesta<EstadoPagoDto>> ActualizarRol(EstadoPagoDto estadoPago)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();
            try
            {
                var estadoPagoEncontrado = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPago.EstadoPagoId);

                if (estadoPagoEncontrado == null)
                {
                    respuesta.Mensaje = "Estado de pago no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoPago, estadoPagoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Estado de pago actualizado con exito";
                respuesta.Datos = _mapper.Map<EstadoPagoDto>(estadoPagoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoDeEstadoPago(int estadoPagoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var estadoPagoEncontrado = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPagoId);

                if (estadoPagoEncontrado == null)
                {
                    respuesta.Mensaje = "Estado de pago no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Estado de pago ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Estado de pago ha sido inactivado.";
                }

                estadoPagoEncontrado.Activo = estado;
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
