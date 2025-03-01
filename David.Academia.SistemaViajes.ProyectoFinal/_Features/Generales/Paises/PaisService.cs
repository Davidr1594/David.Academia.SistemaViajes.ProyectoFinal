using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;


namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.Paises
{
    public class PaisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Pais> _paisRepository;
        private readonly IMapper _mapper;

        public PaisService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _paisRepository = _unitOfWork.Repository<Pais>();
            _mapper = mapper;
        }

        public async Task<Respuesta<PaisDto>> CrearPais(PaisDto paisDto)
        {

            var respuesta = new Respuesta<PaisDto>();


            if (paisDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un rol valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(paisDto.Nombre) || string.IsNullOrWhiteSpace(paisDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del rol es requerido.";
                return respuesta;
            }
            if (await _paisRepository.AsQueryable().AnyAsync(p => p.Nombre.ToLower() == paisDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un rol con este nombre.";
                return respuesta;
            }

            try
            {

                var pais = _mapper.Map<Pais>(paisDto);

                await _paisRepository.AddAsync(pais);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<PaisDto>(pais);
                respuesta.Mensaje = "Pais creado con éxito.";
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

        public async Task<Respuesta<List<PaisDto>>> ObtenerPaises()
        {
            var respuesta = new Respuesta<List<PaisDto>>();
            try
            {
                var paises = await _paisRepository.AsQueryable().ToListAsync();

                if (paises.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Paises";
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

        public async Task<Respuesta<PaisDto>> ObtenerPaisPorId(int paisId)
        {
            var respuesta = new Respuesta<PaisDto>();
            try
            {
                var pais = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (pais == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Pais no encontrado.";
                }
                var paisDto = _mapper.Map<PaisDto>(pais);

                respuesta.Datos = paisDto;
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

        public async Task<Respuesta<PaisDto>> ActualizarPais(int paisId, PaisDto paisDto)
        {
            var respuesta = new Respuesta<PaisDto>();
            try
            {
                var paisEncontrado = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (paisEncontrado == null)
                {
                    respuesta.Mensaje = "Rol no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(paisDto, paisEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Pais actualizado con exito";
                respuesta.Datos = paisDto;
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

        public async Task<Respuesta<bool>> EstadoPais(int paisId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var paisEncontrado = await _paisRepository.AsQueryable().FirstOrDefaultAsync(p => p.PaisId == paisId);

                if (paisEncontrado == null)
                {
                    respuesta.Mensaje = "Pais no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Pais ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Pais ha sido inactivado.";
                }

                paisEncontrado.Activo = estado;
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
