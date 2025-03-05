using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago
{
    public class EstadoPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoPago> _estadoPagoRepository;
        private readonly EstadoPagoDomain _estadoPagoDomain;
        private readonly IMapper _mapper;

        public EstadoPagoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, EstadoPagoDomain estadoPagoDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoPagoRepository = _unitOfWork.Repository<EstadoPago>();
            _mapper = mapper;
            _estadoPagoDomain = estadoPagoDomain;
        }

        public async Task<Respuesta<EstadoPagoDto>> CrearEstadoPago(EstadoPagoDto estadoPagoDto)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();

            var respuestaValidarEntrada = _estadoPagoDomain.ValidarDatosDeEntrada(estadoPagoDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _estadoPagoRepository.AsQueryable()
                                                          .AnyAsync(ep => ep.Nombre.ToLower() == estadoPagoDto.Nombre.ToLower());

            var respuestaValidarBD = _estadoPagoDomain.ValidarRespuestaDeBD(yaExisteNombre);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var estadoPago = _mapper.Map<EstadoPago>(estadoPagoDto);

                await _estadoPagoRepository.AddAsync(estadoPago);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoPagoDto>(estadoPago);
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


        public async Task<Respuesta<List<EstadoPagoDto>>> ObtenerEstadosPagos()
        {
            var respuesta = new Respuesta<List<EstadoPagoDto>>();
            try
            {
                var estadosPagos = await _estadoPagoRepository.AsQueryable().ToListAsync();

                if (estadosPagos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
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

        public async Task<Respuesta<EstadoPagoDto>> ObtenerEstadoPagoPorId(int estadoPagoId)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();
            try
            {
                var estadoPago = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPagoId);

                if (estadoPago == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var rolDto = _mapper.Map<EstadoPagoDto>(estadoPago);

                respuesta.Datos = rolDto;
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

        public async Task<Respuesta<EstadoPagoDto>> ActualizarRol(EstadoPagoDto estadoPago)
        {
            var respuesta = new Respuesta<EstadoPagoDto>();
            try
            {
                var estadoPagoEncontrado = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPago.EstadoPagoId);

                if (estadoPagoEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Estado de pago no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoPago, estadoPagoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<EstadoPagoDto>(estadoPagoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoDeEstadoPago(int estadoPagoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var estadoPagoEncontrado = await _estadoPagoRepository.AsQueryable().FirstOrDefaultAsync(ep => ep.EstadoPagoId == estadoPagoId);

                if (estadoPagoEncontrado == null)
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

                estadoPagoEncontrado.Activo = estado;
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
