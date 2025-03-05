using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Viajes
{
    internal class ViajeDomainValidarDatosDeEntradaTestData : TheoryData<Viaje, List<int>, bool>
    {
        public ViajeDomainValidarDatosDeEntradaTestData()
        {
            Add(ViajeValido(), ColaboradoresValidos(), true); 
            Add(null, ColaboradoresValidos(), false); 
            Add(ViajeSinSucursal(), ColaboradoresValidos(), false); 
            Add(ViajeSinTransportista(), ColaboradoresValidos(), false);
            Add(ViajeSinMoneda(), ColaboradoresValidos(), false); 
            Add(ViajeConFechaPasada(), ColaboradoresValidos(), false); 
            Add(ViajeSinHoraSalida(), ColaboradoresValidos(), false);
            Add(ViajeValido(), new List<int>(), false); 
        }

        private static Viaje ViajeValido() => new()
        {
            SucursalId = 1,
            TransportistaId = 1,
            MonedaId = 1,
            FechaViaje = DateTime.Now.AddDays(1),
            HoraSalida = new TimeSpan(10, 0, 0)
        };

        private static Viaje ViajeSinSucursal() => new()
        {
            SucursalId = 0, 
            TransportistaId = 1,
            MonedaId = 1,
            FechaViaje = DateTime.Now.AddDays(1),
            HoraSalida = new TimeSpan(10, 0, 0)
        };

        private static Viaje ViajeSinTransportista() => new()
        {
            SucursalId = 1,
            TransportistaId = 0, 
            MonedaId = 1,
            FechaViaje = DateTime.Now.AddDays(1),
            HoraSalida = new TimeSpan(10, 0, 0)
        };

        private static Viaje ViajeSinMoneda() => new()
        {
            SucursalId = 1,
            TransportistaId = 1,
            MonedaId = 0, 
            FechaViaje = DateTime.Now.AddDays(1),
            HoraSalida = new TimeSpan(10, 0, 0)
        };

        private static Viaje ViajeConFechaPasada() => new()
        {
            SucursalId = 1,
            TransportistaId = 1,
            MonedaId = 1,
            FechaViaje = DateTime.Now.AddDays(-1), 
            HoraSalida = new TimeSpan(10, 0, 0)
        };

        private static Viaje ViajeSinHoraSalida() => new()
        {
            SucursalId = 1,
            TransportistaId = 1,
            MonedaId = 1,
            FechaViaje = DateTime.Now.AddDays(1),
            HoraSalida = TimeSpan.Zero 
        };

        private static List<int> ColaboradoresValidos() => new() { 1, 2, 3 };
    }
}
