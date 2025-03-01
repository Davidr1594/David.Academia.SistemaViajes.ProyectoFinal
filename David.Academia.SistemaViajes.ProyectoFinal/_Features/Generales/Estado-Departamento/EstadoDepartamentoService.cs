using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class EstadoDepartamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoDepartamento> _estadoDepartamentoRepository;
        private readonly IMapper _mapper;


        public EstadoDepartamentoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoDepartamentoRepository = _unitOfWork.Repository<EstadoDepartamento>();
            _mapper = mapper;
        }

        public async Task<Respuesta<EstadoDepartamentoDto>> CrearEstadoDepartamento(EstadoDepartamentoDto estadoDepartamentoDto)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();

            if (estadoDepartamentoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió una EstadoDepartamento valida.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(estadoDepartamentoDto.Nombre) || string.IsNullOrWhiteSpace(estadoDepartamentoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre de la EstadoDepartamento es requerido.";
                return respuesta;
            }
            if (await _estadoDepartamentoRepository.AsQueryable().AnyAsync(ed => ed.Nombre.ToLower() == estadoDepartamentoDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un EstadoDepartamento con este nombre.";
                return respuesta;
            }

            try
            {
                var estadoDepartamento = _mapper.Map<EstadoDepartamento>(estadoDepartamentoDto);

                await _estadoDepartamentoRepository.AddAsync(estadoDepartamento);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamento);
                respuesta.Mensaje = "EstadoDepartamento creado con éxito.";
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

        public async Task<Respuesta<List<EstadoDepartamentoDto>>> ObtenerEstadosDepartamentos()
        {
            var respuesta = new Respuesta<List<EstadoDepartamentoDto>>();
            try
            {
                var estadosDepartamentos = await _estadoDepartamentoRepository.AsQueryable().ToListAsync();

                if (estadosDepartamentos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay EstadosDepartamentos";
                    return respuesta;
                }

                var estadosDepartamentosDto = new List<EstadoDepartamentoDto>();

                foreach (var estadoDepartamento in estadosDepartamentos)
                {
                    estadosDepartamentosDto.Add(_mapper.Map<EstadoDepartamentoDto>(estadoDepartamento));
                }

                respuesta.Datos = estadosDepartamentosDto;

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

        public async Task<Respuesta<EstadoDepartamentoDto>> ObtenerEstadoDepartamentoPorId(int estadoDepartamentoId)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();
            try
            {
                var estadoDepartamento = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamentoId);

                if (estadoDepartamento == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "EstadoDepartamento no encontrado.";
                }
                var ciudadDto = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamento);

                respuesta.Datos = ciudadDto;
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

        public async Task<Respuesta<EstadoDepartamentoDto>> ActualizarEstadoDepartamento(EstadoDepartamentoDto estadoDepartamento)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();
            try
            {
                var estadoDepartamentoEncontrado = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamento.EstadoId);

                if (estadoDepartamentoEncontrado == null)
                {
                    respuesta.Mensaje = "ÈstadoDepartamento no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoDepartamento, estadoDepartamentoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "EstadoDepartamento actualizado con exito";
                respuesta.Datos = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamentoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoDeEstadoDepartamento(int estadoDepartamentoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var estadoDepartamentoEncontrado = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamentoId);

                if (estadoDepartamentoEncontrado == null)
                {
                    respuesta.Mensaje = "EstadoDepartamento no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "EstadoDepartamento ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "EstadoDepartamento ha sido inactivado.";
                }

                estadoDepartamentoEncontrado.Activo = estado;
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
