using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MonedaService
{
    public class MonedaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Moneda> _monedaRepository;
        private readonly IMapper _mapper;

        public MonedaService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _monedaRepository = _unitOfWork.Repository<Moneda>();
            _mapper = mapper;
        }

        public async Task<Respuesta<MonedaDto>> CrearMoneda(MonedaDto monedaDto)
        {
            var respuesta = new Respuesta<MonedaDto>();

            if (monedaDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió una moneda valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(monedaDto.Nombre) || string.IsNullOrWhiteSpace(monedaDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre de la moneda es requerido.";
                return respuesta;
            }

            try
            {
                if (await _monedaRepository.AsQueryable().AnyAsync(m => m.Nombre.ToLower() == monedaDto.Nombre.ToLower()))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ya existe una moneda con este nombre.";
                    return respuesta;
                }

                var moneda = _mapper.Map<Moneda>(monedaDto);

                await _monedaRepository.AddAsync(moneda);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<MonedaDto>(moneda);
                respuesta.Mensaje = "Moneda creada con éxito.";
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

        public async Task<Respuesta<List<MonedaDto>>> ObtenerMonedas()
        {
            var respuesta = new Respuesta<List<MonedaDto>>();
            try
            {
                var monedas = await _monedaRepository.AsQueryable().ToListAsync();

                if (monedas.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Monedas";
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

        public async Task<Respuesta<MonedaDto>> ObtenerMonedaPorId(int monedaId)
        {
            var respuesta = new Respuesta<MonedaDto>();
            try
            {
                var moneda = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == monedaId);

                if (moneda == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Moneda no encontrada.";
                }
                var monedaDto = _mapper.Map<MonedaDto>(moneda);

                respuesta.Datos = monedaDto;
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

        public async Task<Respuesta<MonedaDto>> ActualizarMoneda(MonedaDto moneda)
        {
            var respuesta = new Respuesta<MonedaDto>();
            try
            {
                var monedaEncontrada = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == moneda.MonedaId);

                if (monedaEncontrada == null)
                {
                    respuesta.Mensaje = "Moneda no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(moneda, monedaEncontrada);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Moneda actualizado con exito";
                respuesta.Datos = _mapper.Map<MonedaDto>(monedaEncontrada);
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

        public async Task<Respuesta<bool>> EstadoMoneda(int monedaId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var rolEncontrado = await _monedaRepository.AsQueryable().FirstOrDefaultAsync(m => m.MonedaId == monedaId);

                if (rolEncontrado == null)
                {
                    respuesta.Mensaje = "Moneda no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Moneda ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Moneda ha sido inactivado.";
                }

                rolEncontrado.Activo = estado;
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
