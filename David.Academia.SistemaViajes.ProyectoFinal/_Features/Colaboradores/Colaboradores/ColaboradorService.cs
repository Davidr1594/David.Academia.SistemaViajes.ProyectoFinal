using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common0;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_
{
    public class ColaboradorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Colaborador> _colaboradorRepository;
        private readonly IMapper _mapper;
        private readonly ManejoDistanciasService _manejoDistanciasService;
        private readonly ColaboradorDomain _colaboradorDomain;

        public ColaboradorService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ManejoDistanciasService manejoDistanciasService, ColaboradorDomain colaboradorDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _colaboradorRepository = _unitOfWork.Repository<Colaborador>();
            _mapper = mapper;
            _manejoDistanciasService = manejoDistanciasService;
            _colaboradorDomain = colaboradorDomain;
        }

        public async Task<Respuesta<ColaboradorDto>> CrearColaborador(int usuarioCreaId, ColaboradorDto colaboradorDto)
        {
            var respuesta = new Respuesta<ColaboradorDto>();

            var respuestaValidarEntradaDatos = _colaboradorDomain.ValidarCreacionColaborador(colaboradorDto);
            if (!respuestaValidarEntradaDatos.Valido)
            {
                respuesta.Valido = respuestaValidarEntradaDatos.Valido;
                respuesta.Mensaje = respuestaValidarEntradaDatos.Mensaje;
                return respuesta;
            }

            if (await _colaboradorRepository.AsQueryable().AnyAsync(c => c.Email.ToLower() == colaboradorDto.Email.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un colaborador con el mismo correo.";
                return respuesta;
            }
            if (await _colaboradorRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == colaboradorDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un colaborador con este nombre.";
                return respuesta;
            }
            if (!await _unitOfWork.Repository<Puesto>().AsQueryable().AnyAsync(p => p.PuestoId == colaboradorDto.PuestoId))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Puesto no existe o esta inactivo.";
                return respuesta;
            }
            if (!await _unitOfWork.Repository<Ciudad>().AsQueryable().AnyAsync(c => c.CiudadId == colaboradorDto.CiudadId))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ciudad no existe o esta inactivo.";
                return respuesta;
            }
            var colaborador = _mapper.Map<Colaborador>(colaboradorDto);
            colaborador.UsuarioCrea = usuarioCreaId;
            var convertirCordenadasADireccion = await _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(colaboradorDto.Latitud, colaboradorDto.Longitud);
            colaborador.Direccion = convertirCordenadasADireccion;

            try
            {

                await _colaboradorRepository.AddAsync(colaborador);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ColaboradorDto>(colaborador);
                respuesta.Mensaje = "Colaborador creado con éxito.";
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

        public async Task<Respuesta<List<ColaboradorDto>>> ObtenerColaboradores()
        {
            var respuesta = new Respuesta<List<ColaboradorDto>>();
            try
            {
                var colaboradores = await _colaboradorRepository.AsQueryable().ToListAsync();

                if (colaboradores.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var colaboradoresDto = new List<ColaboradorDto>();

                foreach (var colaborador in colaboradores)
                {
                    var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                    colaboradoresDto.Add(colaboradorDto);
                }

                respuesta.Datos = colaboradoresDto;
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

        public async Task<Respuesta<ColaboradorDto>> ObtenerColaboradorPorId(int colaboradorId)
        {
            var respuesta = new Respuesta<ColaboradorDto>();
            try
            {
                var colaborador = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

                if (colaborador == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Colaborador");
                }
                var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                respuesta.Datos = colaboradorDto;
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

        public async Task<Respuesta<List<ColaboradorDto>>> ObtenerColaboradorPorSucursalID(int sucursalId)
        {
            var respuesta = new Respuesta<List<ColaboradorDto>>();

            if (!await _unitOfWork.Repository<Sucursal>().AsQueryable().AsNoTracking().AnyAsync(s => s.SucursalId == sucursalId && s.Activo == true))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Sucursal");
                return respuesta;
            }

            try
            {
                var colaboradores = await (from c in _colaboradorRepository.AsQueryable().AsNoTracking()
                                           join sc in _unitOfWork.Repository<SucursalColaborador>().AsQueryable().AsNoTracking() on c.ColaboradorId equals sc.ColaboradorId
                                           where sc.SucursalId == sucursalId && c.Activo == true
                                           select c).ToListAsync();

                if (colaboradores.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var colaboradoresDto = new List<ColaboradorDto>();

                foreach (var colaborador in colaboradores)
                {
                    var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                    colaboradoresDto.Add(colaboradorDto);
                }

                respuesta.Datos = colaboradoresDto;
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

        public async Task<Respuesta<ColaboradorDto>> ActualizarColaborador(int usuarioActualizaId, ColaboradorDto colaborador)
        {
            var respuesta = new Respuesta<ColaboradorDto>();
            try
            {
                var colaboradorEncontrado = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaborador.ColaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Colaborador");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(colaborador, colaboradorEncontrado);
                colaboradorEncontrado.UsuarioActualiza = usuarioActualizaId;
                colaboradorEncontrado.FechaActualizacion = DateTime.Now;

                var convertirCordenadasADireccion = await _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(colaborador.Latitud, colaborador.Longitud);
                colaboradorEncontrado.Direccion = convertirCordenadasADireccion;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ColaboradorDto>(colaboradorEncontrado);
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

        public async Task<Respuesta<bool>> EstadoColaborador(int colaboradorId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var colaboradorEncontrado = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = Mensajes.EntidadNoExiste;
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

                colaboradorEncontrado.Activo = estado;
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
