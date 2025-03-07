using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Colaborador_.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;

namespace David.Academia.SistemaViajes.UnitTests.Sucursales
{
    public class SucursalDomainTestData : TheoryData<SucursalDto?, bool>
    {

        public SucursalDomainTestData()
        {
            Add(SucursalNombreVacio(), false);
            Add(SucursalLatitudInvalidaMenor(), false);
            Add(SucursalLatitudInvalidaMayor(), false);
            Add(SucursalLongitudInvalidaMenor(), false);
            Add(SucursalLongitudInvalidaMayor(), false);
            Add(SucursalTelefonoVacio(), false);
            Add(SucursalTodosLosCamposValidos(), true);
            Add(null, false);

        }

        public SucursalDto SucursalNombreVacio() => new SucursalDto()
        {
            Nombre = "",
            Latitud = 10.0m,
            Longitud = 20.0m,
            Telefono = "95663355"
        };

        public SucursalDto SucursalLatitudInvalidaMenor() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = -91.0m, 
            Longitud = 20.0m,
            Telefono = "95663355"
        };

        public SucursalDto SucursalLatitudInvalidaMayor() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = 91.0m, 
            Longitud = 20.0m,
            Telefono = "95663355"
        };

        public SucursalDto SucursalLongitudInvalidaMenor() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = 10.0m,
            Longitud = -181.0m, 
            Telefono = "95663355"
        };

        public SucursalDto SucursalLongitudInvalidaMayor() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = 10.0m,
            Longitud = 181.0m, 
            Telefono = "95663355"
        };

        public SucursalDto SucursalTelefonoVacio() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = 10.0m,
            Longitud = 20.0m,
            Telefono = "" 
        };

        public SucursalDto SucursalTodosLosCamposValidos() => new SucursalDto()
        {
            Nombre = "Sucursal Central",
            Latitud = 10.0m,
            Longitud = 20.0m,
            Telefono = "95663355"
        };
    }
}
