using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Ciudades.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Ciudades
{
    public class CiudadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Ciudad> _ciudadRepository;
        private readonly CiudadDomain _ciudadDomain;
        private readonly IMapper _mapper;

        public CiudadService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, CiudadDomain ciudadDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _ciudadRepository = _unitOfWork.Repository<Ciudad>();
            _mapper = mapper;
            _ciudadDomain = ciudadDomain;
        }

        public async Task<Respuesta<CiudadDto>> CrearCiudad(CiudadDto ciudadDto)
        {
            var respuesta = new Respuesta<CiudadDto>();

            var respuestaValidarEntrada = _ciudadDomain.ValidarDatosDeEntrada(ciudadDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _ciudadRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == ciudadDto.Nombre.ToLower());
            var existeEstado = await _unitOfWork.Repository<EstadoDepartamento>().AsQueryable().AnyAsync(e => e.EstadoId == ciudadDto.EstadoId);

            var respuestaValidarBD = _ciudadDomain.ValidarRespuestaDeBD(yaExisteNombre, existeEstado);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var ciudad = _mapper.Map<Ciudad>(ciudadDto);

                await _ciudadRepository.AddAsync(ciudad);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<CiudadDto>(ciudad);
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

        public async Task<Respuesta<List<CiudadDto>>> ObtenerCiudades()
        {
            var respuesta = new Respuesta<List<CiudadDto>>();
            try
            {
                var ciudades = await _ciudadRepository.AsQueryable().ToListAsync();

                if (ciudades.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var ciudadesDto = ciudades.Select(ciudad => _mapper.Map<CiudadDto>(ciudad)).ToList();
                respuesta.Datos = ciudadesDto;
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

        public async Task<Respuesta<CiudadDto>> ObtenerCiudadPorId(int ciudadId)
        {
            var respuesta = new Respuesta<CiudadDto>();
            try
            {
                var ciudad = await _ciudadRepository.AsQueryable().FirstOrDefaultAsync(c => c.CiudadId == ciudadId);

                if (ciudad == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Ciudad");
                    return respuesta;
                }

                respuesta.Datos = _mapper.Map<CiudadDto>(ciudad);
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

        public async Task<Respuesta<CiudadDto>> ActualizarCiudad(CiudadDto ciudad)
        {
            var respuesta = new Respuesta<CiudadDto>();
            try
            {
                var ciudadEncontrada = await _ciudadRepository.AsQueryable().FirstOrDefaultAsync(c => c.CiudadId == ciudad.CiudadId);

                if (ciudadEncontrada == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Ciudad");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(ciudad, ciudadEncontrada);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<CiudadDto>(ciudadEncontrada);
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

        public async Task<Respuesta<bool>> EstadoCiudad(int ciudadId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var ciudadEncontrada = await _ciudadRepository.AsQueryable().FirstOrDefaultAsync(c => c.CiudadId == ciudadId);

                if (ciudadEncontrada == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Ciudad");
                    respuesta.Datos = false;
                    return respuesta;
                }

                respuesta.Mensaje = estado ? Mensajes.EntidadActivada : Mensajes.EntidadInactivada;
                ciudadEncontrada.Activo = estado;
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
