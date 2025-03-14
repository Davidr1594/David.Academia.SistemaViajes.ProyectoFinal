using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.UnitTests.Viajes
{
    internal class ViajeDomainValidarKmsColaborador : TheoryData<List<ColaboradorConKmsDto>, decimal, bool>
    {
        public ViajeDomainValidarKmsColaborador()
        {
            Add(ColaboradoresValidos(), 100, true);  //Total dentro del límite
            Add(ColaboradoresConDistanciaInvalida(), 100, false);  //Distancia <= 0 o > 50
            Add(ColaboradoresSuperandoMaximo(), 50, false); //Excede el máximo de Kms permitidos
        }

        private List<ColaboradorConKmsDto> ColaboradoresValidos() => new()
        {
            new ColaboradorConKmsDto { Nombre = "Juan", DistanciaKms = 10 },
            new ColaboradorConKmsDto { Nombre = "Pedro", DistanciaKms = 30 }
        };

        private List<ColaboradorConKmsDto> ColaboradoresConDistanciaInvalida() => new()
        {
            new ColaboradorConKmsDto { Nombre = "Ana", DistanciaKms = 0 },
            new ColaboradorConKmsDto { Nombre = "Luis", DistanciaKms = 55 } 
        };

        private List<ColaboradorConKmsDto> ColaboradoresSuperandoMaximo() => new()
        {
            new ColaboradorConKmsDto { Nombre = "Carlos", DistanciaKms = 60 },
            new ColaboradorConKmsDto { Nombre = "Sofía", DistanciaKms = 50 } 
        };
    }
}
