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
        private readonly IMapper _mapper;


        public ParametroSistemaService(UnitOfWorkBuilder unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork.BuildSistemaDeTransporte();
            _parametroSistemaRepository = _unitOfWork.Repository<ParametroSistema>();
            _mapper = mapper;
        }

        public async Task<Respuesta<ParametroSistemaDto>> CrearParametroSistema(int usuarioCreaId, ParametroSistemaDto parametroDto)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();

            if (parametroDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un parámetro válido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(parametroDto.Descripcion) || string.IsNullOrWhiteSpace(parametroDto.Descripcion))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del parámetro es requerido.";
                return respuesta;
            }
            if (await _parametroSistemaRepository.AsQueryable().AnyAsync(p => p.Descripcion.ToLower() == parametroDto.Descripcion.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un parámetro con este nombre.";
                return respuesta;
            }

            var parametroACrear = _mapper.Map<ParametroSistema>(parametroDto);
            parametroACrear.UsuarioCrea = usuarioCreaId;

            try
            {
                await _parametroSistemaRepository.AddAsync(parametroACrear);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ParametroSistemaDto>(parametroACrear);
                respuesta.Mensaje = "Parámetro creado con éxito.";
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

        public async Task<Respuesta<List<ParametroSistemaDto>>> ObtenerParametrosSistema()
        {
            var respuesta = new Respuesta<List<ParametroSistemaDto>>();
            try
            {
                var parametros = await _parametroSistemaRepository.AsQueryable().ToListAsync();

                if (parametros.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay parámetros";
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

        public async Task<Respuesta<ParametroSistemaDto>> ObtenerParametroSistemaPorId(int parametroId)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();
            try
            {
                var parametro = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametroId);

                if (parametro == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Parámetro no encontrado.";
                }
                var parametroDto = _mapper.Map<ParametroSistemaDto>(parametro);

                respuesta.Datos = parametroDto;
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

        public async Task<Respuesta<ParametroSistemaDto>> ActualizarParametroSistema(int usuarioActualizaId, ParametroSistemaDto parametro)
        {
            var respuesta = new Respuesta<ParametroSistemaDto>();
            try
            {
                var parametroEncontrado = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametro.RegistroId);

                if (parametroEncontrado == null)
                {
                    respuesta.Mensaje = "Parámetro no existe";
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

        public async Task<Respuesta<bool>> EstadoParametroSistema(int parametroId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var parametroEncontrado = await _parametroSistemaRepository.AsQueryable().FirstOrDefaultAsync(p => p.RegistroId == parametroId);

                if (parametroEncontrado == null)
                {
                    respuesta.Mensaje = "Parámetro no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Parámetro ha sido activado.";
                }
                else
                {
                    respuesta.Mensaje = "Parámetro ha sido inactivado.";
                }

                parametroEncontrado.Activo = estado;
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
