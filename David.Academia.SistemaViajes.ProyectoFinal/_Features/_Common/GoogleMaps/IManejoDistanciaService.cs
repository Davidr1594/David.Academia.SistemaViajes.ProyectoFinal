
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps
{
    public interface IManejoDistanciasService
    {
        Task<string> ObtenerDireccionDesdeCordenadas(decimal latitud, decimal longitud);
        Task<decimal> ObtenerDistanciaEntreSucursalColaborador(decimal? latitudOrigen, decimal? longitudOrigen, decimal? latitudDestino, decimal? longitudDestino);
        Task<Respuesta<decimal>> CalcularDistanciaTotalAjustadaAsync(List<ColaboradorConKmsDto> colaboradores, decimal umbralProximidadKm);
        Task<Respuesta<bool>> ValidarDistanciaEntreColaboradoresExistentesEnViajeAsync(List<ColaboradorConKmsDto> colaboradoresAsignados, List<ColaboradorConKmsDto> nuevosColaboradores, decimal umbralMaximoKm);
    }
}
