namespace David.Academia.SistemaViajes.ProyectoFinal._Features._Common
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
        public const string DatosDeEntradaInvalido = "No se recibieron datos validos.";
        public const string DatosDeEntradaInvalidoEntidad = "No se recibieron Datos validos de {0}";
        public const string EntidadNoExiste = "{0} no existe en la base de datos";
        public const string EntidadesNoEncontradas = "No se pudieron obtener {0}";
        public const string ErrorGuardarEntidad = "Ocurrió un error al guardar";
        public const string EntidadGuardada = "Se guardó con éxito.";
        public const string ErrorExcepcion = "Ocurrió un error inesperado";
        public const string FechaNoValida = "La fecha recibida no es válida";
        public const string HoraNoValida = "La hora no es válida";
        public const string NoHayEntidades = "No se econtraron resultados";
        public const string EntidadActivada = "Registro ha sido activado";
        public const string EntidadInactivada = "Registro ha sido Inactidado";
        public const string DatoNoValidoEspecifico = "El dato para {0} no es valido";
        public const string NoSeEncontroEntidadNombre = "No se encotró {0} con este nombre";
        public const string ContraseniaIncorrecta = "La clave es incorrecta";
        public const string AccessoCorrecto = "Acceso correcto.";
        public const string LatitudNoValida = "La latitud debe estar entre -90 y 90 grados.";
        public const string LongitudNoValida = "La longitud debe estar entre -180 y 180 grados.";
        public const string YaExisteRegistro = "Ya existe un registro con este nombre";
        public const string YaExisteCorreo = "Ya existe un registro con este correo";
        public const string YaExisteEstaRelacion = "Ya existe esta relación";
        public const string ElCampoEsRequerido = "El dato {0} es requerido.";
        public const string CampoExcedeCaracteres = "El campo {0} no puede superar los {1} caracteres";
        public const string CorreoNoValido = "El correo electrónico no es válido";
        public const string ElCampoDebeSerMayorCero = "El campo {0} debe ser mayor a cero.";

        #endregion


    }
}
