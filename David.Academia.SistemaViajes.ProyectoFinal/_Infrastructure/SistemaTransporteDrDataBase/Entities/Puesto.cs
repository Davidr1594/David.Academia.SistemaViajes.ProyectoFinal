namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class Puesto
    {
        public int PuestoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }

        public ICollection<Colaborador> Colaboradores { get; set; }
        public Puesto()
        { 
            Nombre = string.Empty;
            Descripcion = string.Empty; 
            Colaboradores = new List<Colaborador>();
        }
    }


}
