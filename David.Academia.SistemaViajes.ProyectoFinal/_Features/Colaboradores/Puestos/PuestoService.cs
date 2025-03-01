using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common0;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos
{
    public class PuestoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Puesto> _puestoRepository;
        private readonly IMapper _mapper;

        public PuestoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _puestoRepository = _unitOfWork.Repository<Puesto>();
            _mapper = mapper;
        }

        public async Task<Respuesta<PuestoDto>> CrearPuesto(PuestoDto puestoDto)
        {
            var respuesta = new Respuesta<PuestoDto>();

            if (puestoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un puesto válido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(puestoDto.Nombre) || string.IsNullOrWhiteSpace(puestoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del puesto es requerido.";
                return respuesta;
            }
            if (await _puestoRepository.AsQueryable().AnyAsync(p => p.Nombre.ToLower() == puestoDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un puesto con este nombre.";
                return respuesta;
            }

            try
            {
                var puesto = _mapper.Map<Puesto>(puestoDto);

                await _puestoRepository.AddAsync(puesto);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<PuestoDto>(puesto);
                respuesta.Mensaje = "Puesto creado con éxito.";
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

        public async Task<Respuesta<List<PuestoDto>>> ObtenerPuestos()
        {
            var respuesta = new Respuesta<List<PuestoDto>>();
            try
            {
                var puestos = await _puestoRepository.AsQueryable().ToListAsync();

                if (puestos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var puestosDto = puestos.Select(puesto => _mapper.Map<PuestoDto>(puesto)).ToList();
                respuesta.Datos = puestosDto;
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

        public async Task<Respuesta<PuestoDto>> ObtenerPuestoPorId(int puestoId)
        {
            var respuesta = new Respuesta<PuestoDto>();
            try
            {
                var puesto = await _puestoRepository.AsQueryable().FirstOrDefaultAsync(p => p.PuestoId == puestoId);

                if (puesto == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Puesto");
                    return respuesta;
                }

                respuesta.Datos = _mapper.Map<PuestoDto>(puesto);
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

        public async Task<Respuesta<PuestoDto>> ActualizarPuesto(PuestoDto puesto)
        {
            var respuesta = new Respuesta<PuestoDto>();
            try
            {
                var puestoEncontrado = await _puestoRepository.AsQueryable().FirstOrDefaultAsync(p => p.PuestoId == puesto.PuestoId);

                if (puestoEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Puesto");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(puesto, puestoEncontrado);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<PuestoDto>(puestoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoPuesto(int puestoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var puestoEncontrado = await _puestoRepository.AsQueryable().FirstOrDefaultAsync(p => p.PuestoId == puestoId);

                if (puestoEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Puesto");
                    respuesta.Datos = false;
                    return respuesta;
                }

                respuesta.Mensaje = estado ? Mensajes.EntidadActivada : Mensajes.EntidadInactivada;
                puestoEncontrado.Activo = estado;
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
