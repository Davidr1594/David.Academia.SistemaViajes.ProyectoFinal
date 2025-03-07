using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.ColaboradorTest
{
    internal class ColaboradorDtoTestData :TheoryData<ColaboradorDto?, bool>
    {
        public ColaboradorDtoTestData()
        {
            Add(ColaboradorNombreVacio(), false);
            Add(ColaboradorApellidoVacio(), false);
            Add(ColaboradorTelefonoVacio(), false);
            Add(ColaboradorDniVacio(), false);
            Add(ColaboradorFechaNacimientoVacio(), false);
            Add(ColaboradorEmailVacio(), false);
            Add(ColaboradorLatitudInvalidaMenor(), false);
            Add(ColaboradorLongitudInvalidaMenor(), false);
            Add(ColaboradorCiudadNoExiste(), false);
            Add(ColaboradorPuestoNoExiste(), false);
            Add(ColaboradorEmailNoValido(), false);
            Add(ColaboradorTodosLosCamposValidos(), true);
            Add(null, false);


        }

        public ColaboradorDto ColaboradorNombreVacio() => new ColaboradorDto()
        {
            Nombre = "",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorApellidoVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId =1

        };
        public ColaboradorDto ColaboradorTelefonoVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorDniVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorFechaNacimientoVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = default,
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorEmailVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15,0, 0,0, DateTimeKind.Utc),
            Email = "",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorEmailNoValido() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email.hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorDireccionVacio() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorLatitudInvalidaMenor() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = -91.0m, // ❌ Fuera de rango permitido
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorLongitudInvalidaMenor() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m, // ❌ Fuera de rango permitido
            Longitud = -191.0m,
            PuestoId = 1,
            CiudadId = 1
        };

        public ColaboradorDto ColaboradorPuestoNoExiste() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 0,
            CiudadId = 1
        };
        public ColaboradorDto ColaboradorCiudadNoExiste() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 0
        };

        public ColaboradorDto ColaboradorTodosLosCamposValidos() => new ColaboradorDto()
        {
            Nombre = "David",
            Apellido = "Reyes",
            Telefono = "95663355",
            Dni = "050115566522",
            FechaNacimiento = new DateTime(1995, 12, 15, 0, 0, 0, DateTimeKind.Utc),
            Email = "email@hotmail.com",
            Direccion = "Direccion",
            Latitud = 0.0m,
            Longitud = 0.0m,
            PuestoId = 1,
            CiudadId = 1
        };

    }
}
