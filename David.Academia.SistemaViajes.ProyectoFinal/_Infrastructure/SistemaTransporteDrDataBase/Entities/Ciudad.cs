using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class Ciudad
    {
        public int CiudadId { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int EstadoId { get; set; }

        public EstadoDepartamento? EstadoDepartamentoNavigation { get; set; }


        public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();

        public Ciudad()
        {
            Nombre = string.Empty;
        }
    }


}
