namespace David.Academia.SistemaViajes.ProyectoFinal._Features._Common
{
    public class Respuesta<T>
    {
        public bool Valido { get; set; }
        public string Mensaje { get; set; }
        public string? DetalleError { get; set; }
        public T? Datos { get; set; }

        public Respuesta()
        {
            Valido = true;
            Mensaje = string.Empty;

        }
    }
}
