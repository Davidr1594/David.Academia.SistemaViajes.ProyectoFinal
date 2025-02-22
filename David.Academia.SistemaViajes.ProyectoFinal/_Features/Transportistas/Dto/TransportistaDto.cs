namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Dto
{
    public class TransportistaDto
    {
        public int TransportistaId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public decimal TarifaPorKm { get; set; }
        public bool Activo { get; set; }
    }
}
