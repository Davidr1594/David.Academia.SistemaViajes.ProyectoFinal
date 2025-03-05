using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas
{
    public class TransportistaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Transportista> _transportistaRepository;
        private readonly TransportistaDomain _transportistaDomain;
        private readonly IMapper _mapper;

        public TransportistaService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, TransportistaDomain transportistaDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _transportistaRepository = _unitOfWork.Repository<Transportista>();
            _transportistaDomain = transportistaDomain;

            _mapper = mapper;
        }

        public async Task<Respuesta<TransportistaDto>> CrearTransportista(int usuarioCrea, TransportistaDto transportistaDto)
        {

            var respuesta = new Respuesta<TransportistaDto>();

            var respuestaValidarDatosEntrada = _transportistaDomain.ValidarDatosDeEntrada(transportistaDto);
            if (!respuestaValidarDatosEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarDatosEntrada.Valido;
                respuesta.Mensaje = respuestaValidarDatosEntrada.Mensaje;
                return respuesta;
            }

            if (await _transportistaRepository.AsQueryable().AnyAsync(t => t.Nombre.ToLower() == transportistaDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            var transportistaACrear = _mapper.Map<Transportista>(transportistaDto);
            transportistaACrear.UsuarioCrea = usuarioCrea;

            try
            {

                await _transportistaRepository.AddAsync(transportistaACrear);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<TransportistaDto>(transportistaACrear);
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

        public async Task<Respuesta<List<TransportistaDto>>> ObtenerTransportistas()
        {
            var respuesta = new Respuesta<List<TransportistaDto>>();
            try
            {
                var transportistas = await _transportistaRepository.AsQueryable().AsNoTracking().ToListAsync();

                if (transportistas.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var transportistasDto = new List<TransportistaDto>();

                foreach (var transportista in transportistas)
                {
                    transportistasDto.Add(_mapper.Map<TransportistaDto>(transportista));
                }

                respuesta.Datos = transportistasDto;
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

        public async Task<Respuesta<TransportistaDto>> ObtenerTransportistaPorId(int transportistaId)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportista = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportista == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var transportistaDto = _mapper.Map<TransportistaDto>(transportista);

                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<TransportistaDto>> ActualizarTransportista(int usuarioActualizaId, TransportistaDto transportistaDto)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportistaEncontrado = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaDto.TransportistaId);

                if (transportistaEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Transportista no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(transportistaDto, transportistaEncontrado);
                transportistaEncontrado.UsuarioActualiza = usuarioActualizaId;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<bool>> EstadoTransportista(int transportistaId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var transportistaEncontrado = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportistaEncontrado == null)
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

                transportistaEncontrado.Activo = estado;
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
