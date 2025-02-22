namespace David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities
{
    public class ValoracionViaje
    {
        public int ValoracionId { get; set; }
        public int TransportistaId { get; set; }
        public int ColaboradorId { get; set; }
        public byte Calificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Transportista TransportistaNavigation { get; set; }   
        public Colaborador ColaboradorNavigation { get; set; }  

        public ValoracionViaje()
        { 
            Comentario = string.Empty;
        }
    }
}
