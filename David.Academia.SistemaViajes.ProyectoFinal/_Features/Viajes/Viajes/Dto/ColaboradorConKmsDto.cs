namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto
{
    public class ColaboradorConKmsDto
    {
        
        public int ColaboradorId { get; set; }
        public string Nombre { get; set; }
        public string DireccionDestino { get; set; }
        public decimal DistanciaKms { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }

    }
}
