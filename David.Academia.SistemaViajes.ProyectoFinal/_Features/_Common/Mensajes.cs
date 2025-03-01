namespace David.Academia.SistemaViajes.ProyectoFinal._Features._Common0
{
    public static class Mensajes
    {
        #region Mensajes de Domain ViajaServiw
        public const string DistanciaFueraDeRango = "La distancia entre colaborador: {0} y la sucursal está fuera del rango permitido.";
        public const string TotalKmsSuperado = "La cantidad de colaboradores sobrepasan los {0} kms.";
        public const string UsuarioNoEsGerente = "El usuario que crea el viaje no es gerente.";
        public const string KmsViajeExcedidos = "No se pueden agregar los colaboradores porque sobrepasa la cantidad máxima del viaje total en kms: {0}";
        public const string FechaViajeInvalida = "La fecha del viaje no puede ser anterior a hoy.";
        public const string KmsNegativos = "Los kilómetros totales no pueden ser negativos.";
        public const string MontoNegativo = "El monto total no puede ser negativo.";
        public const string HoraSalidaInvalida = "La hora de salida no puede ser 00:00.";
        public const string ColaboradorRequerido = "Debe asignar al menos un colaborador.";
        public const string ColaboradoresYaTienenViaje = "Algunos Colaboradores ya tiene un viaje asignado el dia de hoy.";
        public const string NoSeRecibieronColaboradores = "No se recibieron colaboradores para el viaje.";
        public const string NoHayVijeDisponible = "No hay viaje disponible para asignar el colaborador, favor cree un viaje.";
        public const string ColaboradorNoAsignadoALaSucursal = "Hay colaboradores que no estan asignados a la sucursal";

        #endregion

        #region Mensajes Generales
        public const string DatosDeEntradaInvalido = "No se recibieron Datos validos.";
        public const string DatosDeEntradaInvalidoEntidad = "No se recibieron Datos validos de {0}";
        public const string EntidadNoExiste = "{0} No existe en la base de datos";
        public const string EntidadesNoEncontradas = "No se pudieron obtener {0}";
        public const string ErrorGuardarEntidad = "Ocurrió un error al guardar";
        public const string EntidadGuardada = "Se guardó con éxito.";
        public const string ErrorExcepcion = "Ocurrió un error inesperado";
        public const string FechaNoValida = "La recibida no es válida";
        public const string HoraNoValida = "La hora no es válida";
        public const string NoHayEntidades = "No se econtraron resultados";
        public const string EntidadActivada = "Registro ha sido activado";
        public const string EntidadInactivada = "Registro ha sido Inactidado";
       




        #endregion


    }
}
