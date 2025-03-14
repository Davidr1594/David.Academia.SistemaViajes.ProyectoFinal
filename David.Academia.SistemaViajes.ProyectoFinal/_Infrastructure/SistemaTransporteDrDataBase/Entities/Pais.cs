using System.Diagnostics.CodeAnalysis;

namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    [ExcludeFromCodeCoverage]
    public class Pais
    {
        public int PaisId { get; set; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; }
        public int MonedaId { get; set; }
        public bool Activo { get; set; }

 
        public Moneda MonedaNavigation { get; set; }

        public ICollection<EstadoDepartamento> EstadosDepartamentos { get; set; } = new List<EstadoDepartamento>();

        public Pais()
        { 
            Nombre = string.Empty;
            Prefijo = string.Empty;
        }
    }
}
