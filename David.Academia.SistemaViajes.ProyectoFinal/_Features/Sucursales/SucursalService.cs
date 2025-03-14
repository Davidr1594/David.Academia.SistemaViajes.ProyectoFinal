using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class SucursalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Sucursal> _sucursalRepository;
        private readonly IMapper _mapper;
        private readonly ManejoDistanciasService _manejoDistanciasService;
        private readonly SucursalDomain _sucursalDomain;

        public SucursalService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ManejoDistanciasService manejoDistanciasService, SucursalDomain sucursalDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _sucursalRepository = _unitOfWork.Repository<Sucursal>();
            _mapper = mapper;
            _manejoDistanciasService = manejoDistanciasService;
            _sucursalDomain = sucursalDomain;

        }

        public async Task<Respuesta<bool>> AgregarRelacionEntreSucursalColaborador(int sucursalId, int colaboradorId)
        {
            var respuesta = new Respuesta<bool>();

            var respuestaValidarDatosEntrada = _sucursalDomain.ValidarDatosDeEntradaRelacion(sucursalId, colaboradorId);
            if (!respuestaValidarDatosEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarDatosEntrada.Valido;
                respuesta.Mensaje = respuestaValidarDatosEntrada.Mensaje;
                return respuesta;
            }
            try
            {
                var colaborador = await _unitOfWork.Repository<Colaborador>()
                                                    .AsQueryable().AsNoTracking()
                                                    .FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
                var sucursal = await _unitOfWork.Repository<Sucursal>()
                                                .AsQueryable().AsNoTracking()
                                                .FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                var respuestaValidarDatosBaseDatos = _sucursalDomain.ValidarDatosBDRelacion(colaborador, sucursal);
                if(!respuestaValidarDatosBaseDatos.Valido)
                {
                    respuesta.Valido = respuestaValidarDatosBaseDatos.Valido;
                    respuesta.Mensaje = respuestaValidarDatosBaseDatos.Mensaje;
                    return respuesta;
                }
                var kmsDeDistancia = await _manejoDistanciasService.ObtenerDistanciaEntreSucursalColaborador(sucursal!.Latitud, sucursal.Longitud, colaborador!.Latitud, colaborador.Longitud);

                var sucursalColaborador = new SucursalColaborador()
                {
                    ColaboradorId = colaboradorId,
                    SucursalId = sucursalId,
                    DistanciaKm = kmsDeDistancia
                };

                await _unitOfWork.Repository<SucursalColaborador>().AddAsync(sucursalColaborador);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = true;

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

        public async Task<Respuesta<SucursalDto>> CrearSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {

            var respuesta = new Respuesta<SucursalDto>();

            var respuestaValidarDatosSucursal = _sucursalDomain.ValidarCreacionSucursal(sucursalDto);
            if (!respuestaValidarDatosSucursal.Valido)
            {
                respuesta.Valido = respuestaValidarDatosSucursal.Valido;
                respuesta.Mensaje = respuestaValidarDatosSucursal.Mensaje;
                return respuesta;
            }

            if (await _sucursalRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == sucursalDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            var sucursal = _mapper.Map<Sucursal>(sucursalDto);
            sucursal.UsuarioCrea = usuarioActualizaId;

            var convertirCordenadasADireccion = await  _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(sucursalDto.Latitud, sucursalDto.Longitud);
            sucursal.Direccion = convertirCordenadasADireccion;

            try
            {

                await _sucursalRepository.AddAsync(sucursal);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<SucursalDto>(sucursal); ;
                respuesta.Mensaje = Mensajes.EntidadGuardada;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<SucursalDto>>> ObtenerSucursales()
        {
            var respuesta = new Respuesta<List<SucursalDto>>();
            try
            {
                var sucursales = await _sucursalRepository.AsQueryable().ToListAsync();

                if (sucursales.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var sucursalesDto = new List<SucursalDto>();

                foreach (var sucursal in sucursales)
                {
                    sucursalesDto.Add(_mapper.Map<SucursalDto>(sucursal));
                }

                respuesta.Datos = sucursalesDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<SucursalColaboradorDto>>> ObtenerSucursalColaboradores()
        {
            var respuesta = new Respuesta<List<SucursalColaboradorDto>>();
            try
            {
                var sucursalesColaboradores = await _unitOfWork.Repository<SucursalColaborador>().AsQueryable().ToListAsync();

                if (sucursalesColaboradores.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var sucursalesColaboradoresDto = (from sc in _unitOfWork.Repository<SucursalColaborador>().AsQueryable().AsNoTracking()
                                                  join s in _unitOfWork.Repository<Sucursal>().AsQueryable().AsNoTracking() on sc.SucursalId equals s.SucursalId
                                                  join c in _unitOfWork.Repository<Colaborador>().AsQueryable().AsNoTracking() on sc.ColaboradorId equals c.ColaboradorId
                                                  select new SucursalColaboradorDto()
                                                  {
                                                      NombreColaborador = c.Nombre,
                                                      NombreSucursal = s.Nombre,
                                                      DistanciaKm = sc.DistanciaKm
                                                  }).ToList();

                respuesta.Datos = sucursalesColaboradoresDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }


        public async Task<Respuesta<SucursalDto>> ObtenerSucursalPorId(int sucursalId)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                if (sucursalId <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }
                var sucursal = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (sucursal == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var sucursalDto = _mapper.Map<SucursalDto>(sucursal);

                respuesta.Datos = sucursalDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<SucursalDto>> ActualizarSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                var sucursalEncontrada = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalDto.SucursalId);

                if (sucursalEncontrada == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Sucursal no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(sucursalDto, sucursalEncontrada);
                sucursalEncontrada.UsuarioActualiza = usuarioActualizaId;
                sucursalEncontrada.FechaActualizacion = DateTime.Now;

                var convertirCordenadasADireccion = await _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(sucursalDto.Latitud, sucursalDto.Longitud);
                sucursalEncontrada.Direccion = convertirCordenadasADireccion;

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = sucursalDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<bool>> EstadoSucursal(int sucursalId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var sucursalEncontrada = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (sucursalEncontrada == null)
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

                sucursalEncontrada.Activo = estado;
                respuesta.Datos = true;

                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

    }
}
