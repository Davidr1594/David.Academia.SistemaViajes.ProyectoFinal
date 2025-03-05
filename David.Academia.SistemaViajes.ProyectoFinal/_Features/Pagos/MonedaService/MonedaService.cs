using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService
{
    public class MonedaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Moneda> _monedaRepository;
        private readonly MonedaDomain _monedaDomain;
        private readonly IMapper _mapper;

        public MonedaService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, MonedaDomain monedaDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _monedaRepository = _unitOfWork.Repository<Moneda>();
            _mapper = mapper;
            _monedaDomain = monedaDomain;
        }

        public async Task<Respuesta<MonedaDto>> CrearMoneda(MonedaDto monedaDto)
        {
            var respuesta = new Respuesta<MonedaDto>();

            var respuestaValidarEntrada = _monedaDomain.ValidarDatosDeEntrada(monedaDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _monedaRepository.AsQueryable()
                                                        .AnyAsync(m => m.Nombre.ToLower() == monedaDto.Nombre.ToLower());

            var respuestaValidarBD = _monedaDomain.ValidarRespuestaDeBD(yaExisteNombre);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var moneda = _mapper.Map<Moneda>(monedaDto);

                await _monedaRepository.AddAsync(moneda);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<MonedaDto>(moneda);
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


        public async Task<Respuesta<List<MonedaDto>>> ObtenerMonedas()
        {
            var respuesta = new Respuesta<List<MonedaDto>>();
            try
            {
                var monedas = await _monedaRepository.AsQueryable().ToListAsync();

                if (monedas.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var monedasDto = new List<MonedaDto>();

                foreach (var moneda in monedas)
                {
                    monedasDto.Add(_mapper.Map<MonedaDto>(moneda));
                }

                respuesta.Datos = monedasDto;
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

        public async Task<Respuesta<MonedaDto>> ObtenerMonedaPorId(int monedaId)
        {
            var respuesta = new Respuesta<MonedaDto>();
            try
            {
                var moneda = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == monedaId);

                if (moneda == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var monedaDto = _mapper.Map<MonedaDto>(moneda);

                respuesta.Datos = monedaDto;
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

        public async Task<Respuesta<MonedaDto>> ActualizarMoneda(MonedaDto moneda)
        {
            var respuesta = new Respuesta<MonedaDto>();
            try
            {
                var monedaEncontrada = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == moneda.MonedaId);

                if (monedaEncontrada == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Moneda no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(moneda, monedaEncontrada);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<MonedaDto>(monedaEncontrada);
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

        public async Task<Respuesta<bool>> EstadoMoneda(int monedaId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var monedaEncontrada = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == monedaId);

                if (monedaEncontrada == null)
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

                monedaEncontrada.Activo = estado;
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
