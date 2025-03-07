using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Enum;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Viajes
{
    internal class ViajeDomainValidarBdTestData : TheoryData<Sucursal, Usuario, Transportista, List<ColaboradorConKmsDto>, List<int>, bool>
    {
        public ViajeDomainValidarBdTestData()
        {
            Add(SucursalValida(), UsuarioGerente(), TransportistaValido(), ColaboradoresValidos(), new List<int>(), true); //Todos existen
            Add(SucursalValida(), UsuarioGerente(), TransportistaValido(), ColaboradoresValidos(), ColaboradoresEnViaje(), false); //Ya existen colaboradores en viaje
            Add(SucursalInvalida(), UsuarioGerente(), TransportistaValido(), ColaboradoresValidos(), new List<int>(), false); //Sucursal no existe
            Add(SucursalValida(), UsuarioComun(), TransportistaValido(), ColaboradoresValidos(), new List<int>(), false); //Usuario no es gerente/admin
            Add(SucursalValida(), UsuarioGerente(), TransportistaInvalido(), ColaboradoresValidos(), new List<int>(), false); //Transportista no existe
            Add(SucursalValida(), UsuarioGerente(), TransportistaValido(), new List<ColaboradorConKmsDto>(), new List<int>(), false); //No hay colaboradores
            Add(SucursalValida(), UsuarioNoValid(), TransportistaValido(), new List<ColaboradorConKmsDto>(), new List<int>(), false); //UsuarioId no valido

        }

        private static Sucursal SucursalValida() => new() { SucursalId = 1 };
        private static Sucursal SucursalInvalida() => new() { SucursalId = 0 };
        private static Usuario UsuarioGerente() => new() { UsuarioId = 1, RolId = (int)RolEnum.Gerente };
        private static Usuario UsuarioComun() => new() { UsuarioId = 1, RolId = (int)RolEnum.Supervisor };
        private static Usuario UsuarioNoValid() => new() { UsuarioId = 0, RolId = (int)RolEnum.Supervisor };

        private static Transportista TransportistaValido() => new() { TransportistaId = 1 };
        private static Transportista TransportistaInvalido() => new() { TransportistaId = 0 };
        private static List<ColaboradorConKmsDto> ColaboradoresValidos() => new()
        {
            new ColaboradorConKmsDto { ColaboradorId = 1, Nombre = "Juan", DistanciaKms = 10 }
        };
        private static List<int> ColaboradoresEnViaje() => new()
        {1,2 };
    }
}
