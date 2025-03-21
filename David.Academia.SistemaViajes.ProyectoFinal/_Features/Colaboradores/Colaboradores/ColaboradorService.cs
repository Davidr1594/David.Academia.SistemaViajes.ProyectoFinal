﻿using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaboradores;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
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
        private readonly IManejoDistanciasService _manejoDistanciasService;
        private readonly ColaboradorDomain _colaboradorDomain;

        public ColaboradorService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, IManejoDistanciasService manejoDistanciasService, ColaboradorDomain colaboradorDomain)
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

            var respuestaValidarEntradaDatos = _colaboradorDomain.ValidarDatosDeEntrada(colaboradorDto);
            if (!respuestaValidarEntradaDatos.Valido)
            {
                respuesta.Valido = respuestaValidarEntradaDatos.Valido;
                respuesta.Mensaje = respuestaValidarEntradaDatos.Mensaje;
                return respuesta;
            }

            var yaExisteCorreo = await _colaboradorRepository.AsQueryable().AnyAsync(c => c.Email.ToLower() == colaboradorDto.Email.ToLower());
            var yaExisteNombre = await _colaboradorRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == colaboradorDto.Nombre.ToLower());
            var existePuesto = await _unitOfWork.Repository<Puesto>().AsQueryable().AnyAsync(p => p.PuestoId == colaboradorDto.PuestoId);
            var existeCiudad = await _unitOfWork.Repository<Ciudad>().AsQueryable().AnyAsync(c => c.CiudadId == colaboradorDto.CiudadId);

            var respuestaValidarBaseDatps = _colaboradorDomain.ValidarRespuestaDeBD(yaExisteCorreo,yaExisteNombre,existePuesto,existeCiudad);
            if (!respuestaValidarBaseDatps.Valido)
            {
                respuesta.Valido = respuestaValidarBaseDatps.Valido;
                respuesta.Mensaje = respuestaValidarBaseDatps.Mensaje;
                return respuesta;
            }
  

            var colaborador = _mapper.Map<Colaborador>(colaboradorDto);
            colaborador.UsuarioCrea = usuarioCreaId;


            try
            {
                var convertirCordenadasADireccion = await _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(colaboradorDto.Latitud, colaboradorDto.Longitud);
                

                if (string.IsNullOrWhiteSpace(convertirCordenadasADireccion))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.ErrorApiGoogle;
                    return respuesta;
                }
                colaborador.Direccion = convertirCordenadasADireccion;

                await _colaboradorRepository.AddAsync(colaborador);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ColaboradorDto>(colaborador);
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
                respuesta.DetalleError = ex.Message;
                respuesta.Mensaje = Mensajes.ErrorExcepcion;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<ColaboradorDto>>> ObtenerColaboradores()
        {
            var respuesta = new Respuesta<List<ColaboradorDto>>();
            try
            {
                var colaboradores = await _colaboradorRepository.AsQueryable().Where(c => c.Activo).ToListAsync();

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
                var colaborador = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId && c.Activo);

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

        public async Task<Respuesta<List<ColaboradorDto>>> ObtenerColaboradoresPorSucursalID(int sucursalId)
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

        public async Task<Respuesta<bool>> EstadoColaborador(int usuarioActualizaId, int colaboradorId, bool estado)
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
