using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Enum;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes
{
    public class ViajeDomain
    {
        private const decimal RADIO_CERCANIA_KM = 5.0m;


        public Respuesta<decimal> ValidarKmsDeColaborador(List<ColaboradorConKmsDto> colaboradoresDetalle,  decimal maximoKms)
        {
            var respuesta = new Respuesta<decimal>();


            decimal totalKms = 0;
            const decimal limiteDeDistanciaEntreColaboradorYSucursal = 50;
            foreach (var colaborador in colaboradoresDetalle)
            {
                totalKms = totalKms + colaborador.DistanciaKms;

                if (colaborador.DistanciaKms <= 0 || colaborador.DistanciaKms > limiteDeDistanciaEntreColaboradorYSucursal)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = string.Format(Mensajes.DistanciaFueraDeRango, colaborador.Nombre);
                    return respuesta;
                }
            }
            if (totalKms > maximoKms)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.TotalKmsSuperado;

                return respuesta;
            }

            respuesta.Valido = true;
            respuesta.Datos = totalKms;
            return respuesta;
        }

        public Respuesta<bool> ValidarKmsDeViajeAlAgregarColaborador(decimal totalKmsViaje, Viaje viaje, decimal maximoKms)
        {
            var respuesta = new Respuesta<bool>();

            if ((viaje.TotalKms + totalKmsViaje) > maximoKms)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.KmsViajeExcedidos, maximoKms);

                return respuesta;
            }
            respuesta.Valido = true;
            return respuesta;
        }

        public Respuesta<Viaje>ValidarDatosDeEntradaViaje(Viaje viajeACrear, List<int> colaboradoresId) 
        {
            var respuesta = new Respuesta<Viaje> { Valido = true };


            if (viajeACrear == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (viajeACrear.SucursalId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "la sucursal");
                return respuesta;
            }

            if (viajeACrear.TransportistaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "el transportista");
                return respuesta;
            }

            if (viajeACrear.MonedaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "la moneda.");
                return respuesta;
            }

            if (viajeACrear.FechaViaje.Date < DateTime.Now.Date)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.FechaNoValida;
                return respuesta;
            }


            if (viajeACrear.HoraSalida == TimeSpan.Zero)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.HoraNoValida;
                return respuesta;
            }

            if (colaboradoresId == null || !colaboradoresId.Any())
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.NoSeRecibieronColaboradores;
                return respuesta;
            }

            return respuesta;
        }
        public Respuesta<bool> ValidarRespuestasDeBDAgregarColaboradores(Viaje viaje, List<ColaboradorConKmsDto> colaboradoresConKms, List<ColaboradorConKmsDto> colaboradoresValidos)
        {
            var respuesta = new Respuesta<bool> { Valido = true };

            if (viaje.ViajeId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.NoHayVijeDisponible;
                return respuesta;
            }

            if (!colaboradoresConKms.Any())
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ColaboradorNoAsignadoALaSucursal;
                return respuesta;
            }
            if (!colaboradoresValidos.Any())
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ColaboradoresYaTienenViaje;
                return respuesta;
            }


            return respuesta;
        }


        public Respuesta<bool> ValidarRespuestasDeBD(Sucursal sucursal, Usuario usuarioCrea, Transportista transportista, List<ColaboradorConKmsDto> colaboradoresDetalle, List<int> colaboradoresEnViaje )
        {
            var respuesta = new Respuesta<bool> { Valido = true };


            if (sucursal.SucursalId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Sucursal");

                return respuesta;
            }
            if (usuarioCrea.UsuarioId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Gerente");
                return respuesta;
            }
            if (usuarioCrea.RolId != (int)RolEnum.Gerente && usuarioCrea.RolId != (int)RolEnum.Administrador)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.UsuarioNoEsGerente;
                return respuesta;
            }
            if (transportista.TransportistaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Transportista");
                return respuesta;
            }
            if (!colaboradoresDetalle.Any())
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadesNoEncontradas, "Los Colaboradores");
                return respuesta;
            }
            if (colaboradoresEnViaje.Any())
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ColaboradoresYaTienenViaje;
                return respuesta;
            }

            return respuesta;
        }

        public Respuesta<Viaje> ProcesarDatosDeViajeAGuardar(Viaje viajeACrear, int usuarioCreaId, decimal totalKmsConCalculoDistancia, Transportista transportista)
        {
            var respuesta = new Respuesta<Viaje>();

            viajeACrear.UsuarioCrea = usuarioCreaId;
            viajeACrear.TotalKms = totalKmsConCalculoDistancia;
            viajeACrear.MontoTotal = transportista.TarifaPorKm * totalKmsConCalculoDistancia;
            viajeACrear.EstadoId = (int)EstadoViajeEnum.Pendiente;
            viajeACrear.Activo = true;

            respuesta.Datos = viajeACrear;
            return respuesta;
        }

        public Respuesta<List<ViajeDetalle>> ProcesarDatosDetalleViajeAGuardar(List<ColaboradorConKmsDto> colaboradoresDetalle, int viajeId)
        {
            var respuesta = new Respuesta<List<ViajeDetalle>>();

            var viajeDetalleAGuardar = new List<ViajeDetalle>();
            foreach (var colaborador in colaboradoresDetalle)
            {
                var viajeDetalle = new ViajeDetalle()
                {
                    ViajeId = viajeId,
                    ColaboradorId = colaborador.ColaboradorId,
                    DireccionDestino = colaborador.DireccionDestino,
                    Kms = colaborador.DistanciaKms,
                };

                viajeDetalleAGuardar.Add(viajeDetalle);
            }
            respuesta.Datos = viajeDetalleAGuardar;

            return respuesta;

        }

        public Respuesta<List<ViajeDetalle>>ProcesarDatosAgregarColaboradorAViaje(List<ColaboradorConKmsDto> colaboradoresValidos, int viajeId)
        {
            var respuesta = new Respuesta<List<ViajeDetalle>>();

            var viajeDetalleAGuardar = new List<ViajeDetalle>();

            foreach (var colaborador in colaboradoresValidos)
            {
                var viajeDetalle = new ViajeDetalle()
                {
                    ViajeId = viajeId,
                    ColaboradorId = colaborador.ColaboradorId,
                    DireccionDestino = colaborador.DireccionDestino,
                    Kms = colaborador.DistanciaKms,
                };

                viajeDetalleAGuardar.Add(viajeDetalle);
            }
            respuesta.Datos = viajeDetalleAGuardar;

            return respuesta;
        }
    }


}
