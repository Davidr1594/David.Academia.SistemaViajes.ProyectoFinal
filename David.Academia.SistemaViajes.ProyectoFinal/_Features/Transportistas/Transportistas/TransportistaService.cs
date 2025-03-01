using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
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
        private readonly IMapper _mapper;

        public TransportistaService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _transportistaRepository = _unitOfWork.Repository<Transportista>();

            _mapper = mapper;
        }

        public async Task<Respuesta<TransportistaDto>> CrearTransportista(int usuarioCrea, TransportistaDto transportistaDto)
        {

            var respuesta = new Respuesta<TransportistaDto>();

            if (transportistaDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un usuario valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(transportistaDto.Nombre) || string.IsNullOrWhiteSpace(transportistaDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del usuario es requerido.";
                return respuesta;
            }
            if (await _transportistaRepository.AsQueryable().AnyAsync(t => t.Nombre.ToLower() == transportistaDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un usuario con este nombre.";
                return respuesta;
            }

            var transportistaACrear = _mapper.Map<Transportista>(transportistaDto);
            transportistaACrear.UsuarioCrea = usuarioCrea;

            try
            {

                await _transportistaRepository.AddAsync(transportistaACrear);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<TransportistaDto>(transportistaACrear);
                respuesta.Mensaje = "Transportista creado con éxito.";
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

        public async Task<Respuesta<List<TransportistaDto>>> ObtenerTransportistas()
        {
            var respuesta = new Respuesta<List<TransportistaDto>>();
            try
            {
                var transportistas = await _transportistaRepository.AsQueryable().AsQueryable().AsNoTracking().ToListAsync();

                if (transportistas.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay transportistas";
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

        public async Task<Respuesta<TransportistaDto>> ObtenerTransportistaPorId(int transportistaId)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportista = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportista == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Transportista no encontrado.";
                }
                var transportistaDto = _mapper.Map<TransportistaDto>(transportista);

                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<TransportistaDto>> ActualizarTransportista(int usuarioActualizaId, TransportistaDto transportistaDto)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportistaEncontrado = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaDto.TransportistaId);

                if (transportistaEncontrado == null)
                {
                    respuesta.Mensaje = "Usuario no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(transportistaDto, transportistaEncontrado);
                transportistaEncontrado.UsuarioActualiza = usuarioActualizaId;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<bool>> EstadoTransportista(int transportistaId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var transportistaEncontrado = await _transportistaRepository.AsQueryable().FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportistaEncontrado == null)
                {
                    respuesta.Mensaje = "Transportista no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Transportista ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Transportista ha sido inactivado.";
                }

                transportistaEncontrado.Activo = estado;
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
