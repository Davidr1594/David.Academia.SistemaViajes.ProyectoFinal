using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago
{
    public class MetodoPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MetodoPago> _metodoPagoRepository;
        private readonly IMapper _mapper;

        public MetodoPagoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _metodoPagoRepository = _unitOfWork.Repository<MetodoPago>();
            _mapper = mapper;
        }

        public async Task<Respuesta<MetodoPagoDto>> CrearMetodoPago(MetodoPagoDto metodoPagoDto)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();

            if (metodoPagoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un Metodo de pago valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(metodoPagoDto.Nombre) || string.IsNullOrWhiteSpace(metodoPagoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del Metodo de pago es requerido.";
                return respuesta;
            }

            try
            {
                if (await _metodoPagoRepository.AsQueryable().AnyAsync(r => r.Nombre.ToLower() == metodoPagoDto.Nombre.ToLower()))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ya existe un Metodo de pago con este nombre.";
                    return respuesta;
                }

                var metodoPago = _mapper.Map<MetodoPago>(metodoPagoDto);

                await _metodoPagoRepository.AddAsync(metodoPago);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<MetodoPagoDto>(metodoPago);
                respuesta.Mensaje = "Metodo de pago creado con éxito.";
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

        public async Task<Respuesta<List<MetodoPagoDto>>> ObtenerMetodosPagos()
        {
            var respuesta = new Respuesta<List<MetodoPagoDto>>();
            try
            {
                var metodosPagos = await _metodoPagoRepository.AsQueryable().ToListAsync();

                if (metodosPagos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Metodos de pagos";
                    return respuesta;
                }

                var metodosPagosDto = new List<MetodoPagoDto>();

                foreach (var metodoPago in metodosPagos)
                {
                    metodosPagosDto.Add(_mapper.Map<MetodoPagoDto>(metodoPago));
                }

                respuesta.Datos = metodosPagosDto;

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

        public async Task<Respuesta<MetodoPagoDto>> ObtenerMetodoPagoPorId(int metodoPagoId)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();
            try
            {
                var metodoPago = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPagoId);

                if (metodoPago == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Metodo de pago no encontrado.";
                }
                var metodoPagoDto = _mapper.Map<MetodoPagoDto>(metodoPago);

                respuesta.Datos = metodoPagoDto;
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

        public async Task<Respuesta<MetodoPagoDto>> ActualizarMetodoPago(MetodoPagoDto metodoPago)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();
            try
            {
                var metodoPagoEncontrado = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPago.MetodoPagoId);

                if (metodoPagoEncontrado == null)
                {
                    respuesta.Mensaje = "Metodo de pago no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(metodoPago, metodoPagoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Metodo de pago actualizado con exito";
                respuesta.Datos = _mapper.Map<MetodoPagoDto>(metodoPagoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoMetodoPago(int metodoPagoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var metodoPagoEncontrado = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPagoId);

                if (metodoPagoEncontrado == null)
                {
                    respuesta.Mensaje = "Metodo de pago no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Metodo de pago ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Metodo de pago ha sido inactivado.";
                }

                metodoPagoEncontrado.Activo = estado;
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
