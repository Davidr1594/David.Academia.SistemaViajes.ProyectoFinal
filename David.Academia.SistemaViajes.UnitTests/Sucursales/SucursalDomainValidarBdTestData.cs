using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Sucursales
{
    public class SucursalDomainValidarBdTestData :TheoryData<Colaborador?, Sucursal?, bool>
    {
        public SucursalDomainValidarBdTestData()
        {
            Add(new Colaborador(), new Sucursal(), true); //ambos existen
            Add(null, new Sucursal(), false);            //Colaborador no existe
            Add(new Colaborador(), null, false);         //Sucursal no existe
            Add(null, null, false);                      //Ambos no existe
        }
        
    }
}
