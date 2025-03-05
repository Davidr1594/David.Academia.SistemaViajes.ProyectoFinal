using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Estado_Departamento;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class EstadoDepartamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<EstadoDepartamento> _estadoDepartamentoRepository;
        private readonly EstadoDepartamentoDomain _estadoDepartamentoDomain;
        private readonly IMapper _mapper;


        public EstadoDepartamentoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, EstadoDepartamentoDomain estadoDepartamentoDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _estadoDepartamentoRepository = _unitOfWork.Repository<EstadoDepartamento>();
            _mapper = mapper;
            _estadoDepartamentoDomain = estadoDepartamentoDomain;
        }

        public async Task<Respuesta<EstadoDepartamentoDto>> CrearEstadoDepartamento(EstadoDepartamentoDto estadoDepartamentoDto)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();

            var respuestaValidarEntrada = _estadoDepartamentoDomain.ValidarDatosDeEntrada(estadoDepartamentoDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _estadoDepartamentoRepository.AsQueryable()
                                                                    .AnyAsync(ed => ed.Nombre.ToLower() == estadoDepartamentoDto.Nombre.ToLower());
            var existePais = await _unitOfWork.Repository<Pais>().AsQueryable()
                                                                    .AnyAsync(p => p.PaisId == estadoDepartamentoDto.PaisId);

            var respuestaValidarBD = _estadoDepartamentoDomain.ValidarRespuestaDeBD(yaExisteNombre, existePais);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var estadoDepartamento = _mapper.Map<EstadoDepartamento>(estadoDepartamentoDto);

                await _estadoDepartamentoRepository.AddAsync(estadoDepartamento);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamento);
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


        public async Task<Respuesta<List<EstadoDepartamentoDto>>> ObtenerEstadosDepartamentos()
        {
            var respuesta = new Respuesta<List<EstadoDepartamentoDto>>();
            try
            {
                var estadosDepartamentos = await _estadoDepartamentoRepository.AsQueryable().ToListAsync();

                if (estadosDepartamentos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
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

        public async Task<Respuesta<EstadoDepartamentoDto>> ObtenerEstadoDepartamentoPorId(int estadoDepartamentoId)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();
            try
            {
                var estadoDepartamento = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamentoId);

                if (estadoDepartamento == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var ciudadDto = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamento);

                respuesta.Datos = ciudadDto;
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

        public async Task<Respuesta<EstadoDepartamentoDto>> ActualizarEstadoDepartamento(EstadoDepartamentoDto estadoDepartamento)
        {
            var respuesta = new Respuesta<EstadoDepartamentoDto>();
            try
            {
                var estadoDepartamentoEncontrado = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamento.EstadoId);

                if (estadoDepartamentoEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Estado no esite");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(estadoDepartamento, estadoDepartamentoEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<EstadoDepartamentoDto>(estadoDepartamentoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoDeEstadoDepartamento(int estadoDepartamentoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var estadoDepartamentoEncontrado = await _estadoDepartamentoRepository.AsQueryable().FirstOrDefaultAsync(ed => ed.EstadoId == estadoDepartamentoId);

                if (estadoDepartamentoEncontrado == null)
                {
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadActivada;
                }
                else if (!estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadInactivada;
                }

                estadoDepartamentoEncontrado.Activo = estado;
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
