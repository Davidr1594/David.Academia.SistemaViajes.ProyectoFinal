using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;


namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises
{
    public class PaisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Pais> _paisRepository;
        private readonly PaisDomain _paisDomain;
        private readonly IMapper _mapper;

        public PaisService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, PaisDomain paisDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _paisRepository = _unitOfWork.Repository<Pais>();
            _mapper = mapper;
            _paisDomain = paisDomain;
        }

        public async Task<Respuesta<PaisDto>> CrearPais(PaisDto paisDto)
        {
            var respuesta = new Respuesta<PaisDto>();

            var respuestaValidarEntrada = _paisDomain.ValidarDatosDeEntrada(paisDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _paisRepository.AsQueryable()
                                                     .AnyAsync(p => p.Nombre.ToLower() == paisDto.Nombre.ToLower());
            var existeMoneda = await _unitOfWork.Repository<Moneda>().AsQueryable()
                                                     .AnyAsync(m => m.MonedaId == paisDto.MonedaId);

            var respuestaValidarBD = _paisDomain.ValidarRespuestaDeBD(yaExisteNombre, existeMoneda);
            if (!respuestaValidarBD.Valido)
            {
                respuesta.Valido = respuestaValidarBD.Valido;
                respuesta.Mensaje = respuestaValidarBD.Mensaje;
                return respuesta;
            }

            try
            {
                var pais = _mapper.Map<Pais>(paisDto);

                await _paisRepository.AddAsync(pais);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<PaisDto>(pais);
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

        public async Task<Respuesta<List<PaisDto>>> ObtenerPaises()
        {
            var respuesta = new Respuesta<List<PaisDto>>();
            try
            {
                var paises = await _paisRepository.AsQueryable().ToListAsync();

                if (paises.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var paisesDto = new List<PaisDto>();

                foreach (var pais in paises)
                {
                    paisesDto.Add(_mapper.Map<PaisDto>(pais));
                }

                respuesta.Datos = paisesDto;

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

        public async Task<Respuesta<PaisDto>> ObtenerPaisPorId(int paisId)
        {
            var respuesta = new Respuesta<PaisDto>();
            try
            {
                var pais = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (pais == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var paisDto = _mapper.Map<PaisDto>(pais);

                respuesta.Datos = paisDto;
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

        public async Task<Respuesta<PaisDto>> ActualizarPais(int paisId, PaisDto paisDto)
        {
            var respuesta = new Respuesta<PaisDto>();
            try
            {
                var paisEncontrado = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (paisEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Pais");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(paisDto, paisEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = paisDto;
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

        public async Task<Respuesta<bool>> EstadoPais(int paisId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var paisEncontrado = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (paisEncontrado == null)
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

                paisEncontrado.Activo = estado;
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
