using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema
{
    public class ParametroSistemaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ParametroSistema> _parametroSistemaRepository;
        private readonly ParametroSistemaDomain _parametroSistemaDomain;
        private readonly IMapper _mapper;


        public ParametroSistemaService(UnitOfWorkBuilder unitOfWork, IMapper mapper, ParametroSistemaDomain parametroSistemaDomain)
        {
            _unitOfWork = unitOfWork.BuildSistemaDeTransporte();
            _parametroSistemaRepository = _unitOfWork.Repository<ParametroSistema>();
            _mapper = mapper;
            _parametroSistemaDomain = parametroSistemaDomain;
        }

        public async Task<Respuesta<ParametroSistemaDto>> CrearParametroSistema(int usuarioCreaId, ParametroSistemaDto parametroDto)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();

            var respuestaValidarEntrada = _parametroSistemaDomain.ValidarDatosDeEntrada(parametroDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteDescripcion = await _parametroSistemaRepository.AsQueryable()
                .AnyAsync(p => p.Descripcion.ToLower() == parametroDto.Descripcion.ToLower());

            var respuestaValidarBD = _parametroSistemaDomain.ValidarRespuestaDeBD(yaExisteDescripcion);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            var parametroACrear = _mapper.Map<ParametroSistema>(parametroDto);
            parametroACrear.UsuarioCrea = usuarioCreaId;

            try
            {
                await _parametroSistemaRepository.AddAsync(parametroACrear);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ParametroSistemaDto>(parametroACrear);
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


        public async Task<Respuesta<List<ParametroSistemaDto>>> ObtenerParametrosSistema()
        {
            var respuesta = new Respuesta<List<ParametroSistemaDto>>();
            try
            {
                var parametros = await _parametroSistemaRepository.AsQueryable().ToListAsync();

                if (parametros.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var parametrosDto = new List<ParametroSistemaDto>();

                foreach (var parametro in parametros)
                {
                    parametrosDto.Add(_mapper.Map<ParametroSistemaDto>(parametro));
                }

                respuesta.Datos = parametrosDto;
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

        public async Task<Respuesta<ParametroSistemaDto>> ObtenerParametroSistemaPorId(int parametroId)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();
            try
            {
                var parametro = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametroId);

                if (parametro == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var parametroDto = _mapper.Map<ParametroSistemaDto>(parametro);

                respuesta.Datos = parametroDto;
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

        public async Task<Respuesta<ParametroSistemaDto>> ActualizarParametroSistema(int usuarioActualizaId, ParametroSistemaDto parametro)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();
            try
            {
                var parametroEncontrado = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametro.RegistroId);

                if (parametroEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Parámetro no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(parametro, parametroEncontrado);
                parametroEncontrado.UsuarioActualiza = usuarioActualizaId;
                parametroEncontrado.FechaActualiza = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ParametroSistemaDto>(parametroEncontrado);
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

        public async Task<Respuesta<bool>> EstadoParametroSistema(int parametroId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var parametroEncontrado = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametroId);

                if (parametroEncontrado == null)
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

                parametroEncontrado.Activo = estado;
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
