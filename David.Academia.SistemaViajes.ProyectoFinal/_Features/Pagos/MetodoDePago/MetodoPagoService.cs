using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago
{
    public class MetodoPagoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MetodoPago> _metodoPagoRepository;
        private readonly MetodoPagoDomain _metodoPagoDomain;
        private readonly IMapper _mapper;

        public MetodoPagoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, MetodoPagoDomain metodoPagoDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _metodoPagoRepository = _unitOfWork.Repository<MetodoPago>();
            _mapper = mapper;
            _metodoPagoDomain = metodoPagoDomain;
        }
        public async Task<Respuesta<MetodoPagoDto>> CrearMetodoPago(MetodoPagoDto metodoPagoDto)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();

            var respuestaValidarEntrada = _metodoPagoDomain.ValidarDatosDeEntrada(metodoPagoDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _metodoPagoRepository.AsQueryable()
                                                             .AnyAsync(mp => mp.Nombre.ToLower() == metodoPagoDto.Nombre.ToLower());

            var respuestaValidarBD = _metodoPagoDomain.ValidarRespuestaDeBD(yaExisteNombre);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var metodoPago = _mapper.Map<MetodoPago>(metodoPagoDto);

                await _metodoPagoRepository.AddAsync(metodoPago);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<MetodoPagoDto>(metodoPago);
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

        public async Task<Respuesta<List<MetodoPagoDto>>> ObtenerMetodosPagos()
        {
            var respuesta = new Respuesta<List<MetodoPagoDto>>();
            try
            {
                var metodosPagos = await _metodoPagoRepository.AsQueryable().ToListAsync();

                if (metodosPagos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
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

        public async Task<Respuesta<MetodoPagoDto>> ObtenerMetodoPagoPorId(int metodoPagoId)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();
            try
            {
                var metodoPago = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPagoId);

                if (metodoPago == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var metodoPagoDto = _mapper.Map<MetodoPagoDto>(metodoPago);

                respuesta.Datos = metodoPagoDto;
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

        public async Task<Respuesta<MetodoPagoDto>> ActualizarMetodoPago(MetodoPagoDto metodoPago)
        {
            var respuesta = new Respuesta<MetodoPagoDto>();
            try
            {
                var metodoPagoEncontrado = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPago.MetodoPagoId);

                if (metodoPagoEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Método de pago no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(metodoPago, metodoPagoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<MetodoPagoDto>(metodoPagoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoMetodoPago(int metodoPagoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var metodoPagoEncontrado = await _metodoPagoRepository.AsQueryable().FirstOrDefaultAsync(mp => mp.MetodoPagoId == metodoPagoId);

                if (metodoPagoEncontrado == null)
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

                metodoPagoEncontrado.Activo = estado;
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
