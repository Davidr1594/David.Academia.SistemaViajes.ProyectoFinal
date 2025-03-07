using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Viajes
{
    public class ViajeDomainProcesarDatosTestData : TheoryData<Viaje, int, decimal, Transportista, bool>
    {
        public ViajeDomainProcesarDatosTestData()
        {
            Add(ViajeValido(), 1, 100, TransportistaValido(), true);  //Caso válido
            Add(ViajeValido(), 1, 0, TransportistaValido(), true);    //Sin distancia
            Add(ViajeValido(), 1, 50, TransportistaInvalido(), true); //Transportista sin tarifa
        }

        private static Viaje ViajeValido() => new()
        {
            ViajeId = 1,
            UsuarioCrea = 1,
            TotalKms = 0,
            MontoTotal = 0,
            EstadoId = 0,
            Activo = false
        };

        private static Transportista TransportistaValido() => new()
        {
            TransportistaId = 1,
            TarifaPorKm = 10
        };

        private static Transportista TransportistaInvalido() => new()
        {
            TransportistaId = 1,
            TarifaPorKm = 0
        };
    }
}
