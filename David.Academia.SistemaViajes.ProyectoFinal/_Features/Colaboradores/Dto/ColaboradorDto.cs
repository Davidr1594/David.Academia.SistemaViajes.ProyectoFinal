namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Dto
{
    public class ColaboradorDto
    {
        public int ColaboradorId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int PuestoId { get; set; }
        public int CiudadId { get; set; }
        public bool Activo { get; set; } = true;


        public ColaboradorDto()
        {
            Nombre = string.Empty;
            Apellido = string.Empty;
            Telefono = string.Empty;
            Dni = string.Empty;
            Email = string.Empty;
            Direccion = string.Empty;
        }
    }

}
