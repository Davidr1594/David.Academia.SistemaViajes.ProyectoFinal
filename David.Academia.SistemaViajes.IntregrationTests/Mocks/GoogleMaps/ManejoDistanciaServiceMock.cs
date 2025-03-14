using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AcademiaIntegrationTestAndMock.IntegrationTest.Mocks
{
    public static class ManejoDistanciasServiceMock
    {
        public static void AddDefaultManejoDistanciasServiceMock(this IServiceCollection services)
        {
            // Registra el substitute de IManejoDistanciasService
            services.AddSingleton(Substitute.For<IManejoDistanciasService>());
        }

        // Configuración exitosa: siempre devuelve "San Pedro Sula"
        public static void SetupExitosoObtenerDireccionDesdeCordenadas(IManejoDistanciasService servicio)
        {
            servicio.ObtenerDireccionDesdeCordenadas(Arg.Any<decimal>(), Arg.Any<decimal>())
                    .Returns(Task.FromResult("San Pedro Sula"));
        }

        // Configuración de error: devuelve null
        public static void SetupErrorObtenerDireccionDesdeCordenadas(IManejoDistanciasService servicio)
        {
            servicio.ObtenerDireccionDesdeCordenadas(Arg.Any<decimal>(), Arg.Any<decimal>())
                    .Returns(Task.FromResult<string>(null));
        }

        // Configuración para lanzar una excepción simulando un error
        public static void SetupThrowExceptionObtenerDireccionDesdeCordenadas(IManejoDistanciasService servicio)
        {
            servicio.ObtenerDireccionDesdeCordenadas(Arg.Any<decimal>(), Arg.Any<decimal>())
                    .ThrowsAsync(new Exception("Error en el servicio de distancias."));
        }

        public static void SetupExitosoCalcularDistanciaTotalAjustadaAsync(IManejoDistanciasService servicio)
        {
            var respuestaMock = new Respuesta<decimal>
            {
                Valido = true,
                Datos = 21.5m, // Un valor decimal de prueba
                Mensaje = "Distancia calculada exitosamente"
            };
            servicio.CalcularDistanciaTotalAjustadaAsync(Arg.Any<List<ColaboradorConKmsDto>>(), Arg.Any<decimal>())
                    .Returns(Task.FromResult(respuestaMock));
        }
            
    }
}