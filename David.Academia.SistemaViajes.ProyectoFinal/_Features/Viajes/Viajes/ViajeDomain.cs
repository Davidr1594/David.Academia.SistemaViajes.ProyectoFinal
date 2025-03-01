using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common0;
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

        public  Respuesta<bool> EsGerente(Usuario usuarioCrea)
        {
            var respuesta = new Respuesta<bool>();

            if (usuarioCrea.RolId != (int)RolEnum.Gerente && usuarioCrea.RolId != (int)RolEnum.Administrador)
            { 
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.UsuarioNoEsGerente;
                return respuesta;
            }

            respuesta.Valido = true;
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

        public Respuesta<bool>ValidarDatosDeEntradaViaje(ViajeDto viajeDto)
        {
            var respuesta = new Respuesta<bool> { Valido = true };


            if (viajeDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.DatosDeEntradaInvalido;
                return respuesta;
            }

            if (viajeDto.SucursalId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "la sucursal");
                return respuesta;
            }

            if (viajeDto.TransportistaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "el transportista");
                return respuesta;
            }

            if (viajeDto.MonedaId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.DatosDeEntradaInvalidoEntidad, "la moneda.");
                return respuesta;
            }

            if (viajeDto.FechaViaje.Date < DateTime.Now.Date)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.FechaNoValida;
                return respuesta;
            }


            if (viajeDto.HoraSalida == TimeSpan.Zero)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.HoraNoValida;
                return respuesta;
            }

            if (viajeDto.ColaboradoresId == null || !viajeDto.ColaboradoresId.Any())
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


        public Respuesta<bool> ValidarRespuestasDeBD(Sucursal sucursal, Usuario usucarioCrea, Transportista transportista, List<ColaboradorConKmsDto> colaboradoresDetalle, List<int> colaboradoresEnViaje )
        {
            var respuesta = new Respuesta<bool> { Valido = true };


            if (sucursal.SucursalId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Sucursal");

                return respuesta;
            }
            if (usucarioCrea.UsuarioId == 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Gerente");
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

    }


}
