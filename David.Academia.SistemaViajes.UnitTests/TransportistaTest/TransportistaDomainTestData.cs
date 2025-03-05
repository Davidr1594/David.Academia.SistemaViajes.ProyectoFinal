using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Transportistas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.TransportistaTest
{
    internal class TransportistaDtoTestData : TheoryData<TransportistaDto, bool>
    {
        public TransportistaDtoTestData()
        {
            Add(TransportistaNulo(), false);
            Add(TransportistaNombreVacio(), false);
            Add(TransportistaApellidoVacio(), false);
            Add(TransportistaDniVacio(), false);
            Add(TransportistaDniExcedeLimite(), false);
            Add(TransportistaTelefonoVacio(), false);
            Add(TransportistaTelefonoExcedeLimite(), false);
            Add(TransportistaEmailVacio(), false);
            Add(TransportistaEmailNoValido(), false);
            Add(TransportistaTarifaInvalida(), false);
            Add(TransportistaTodosLosCamposValidos(), true);
        }

        private static TransportistaDto TransportistaNulo() => null!;

        private static TransportistaDto TransportistaNombreVacio() => new TransportistaDto
        {
            Nombre = "",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaApellidoVacio() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaDniVacio() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "",
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaDniExcedeLimite() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "123456789012345678901", // Más de 20 caracteres
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaTelefonoVacio() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaTelefonoExcedeLimite() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "123456789012345678901", // Más de 20 caracteres
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaEmailVacio() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "",
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaEmailNoValido() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "email.hotmail.com", // No tiene '@'
            TarifaPorKm = 10
        };

        private static TransportistaDto TransportistaTarifaInvalida() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 0 // Debe ser mayor a 0
        };

        private static TransportistaDto TransportistaTodosLosCamposValidos() => new TransportistaDto
        {
            Nombre = "David",
            Apellido = "Reyes",
            Dni = "050115566522",
            Telefono = "95663355",
            Email = "email@hotmail.com",
            TarifaPorKm = 10
        };
    }
}
