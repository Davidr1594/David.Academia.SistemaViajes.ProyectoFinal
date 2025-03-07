using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps.Entities;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ManejoDistanciasService : IManejoDistanciasService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleMapsSetting _googleMapsSettings;

    public ManejoDistanciasService(HttpClient httpClient, IOptions<GoogleMapsSetting> googleMapsSettings)
    {
        _httpClient = httpClient;
        _googleMapsSettings = googleMapsSettings.Value;
    }

    public ManejoDistanciasService() { }

    public async Task<string> ObtenerDireccionDesdeCordenadas(decimal latitud, decimal longitud)
    {
        if (latitud < -90 || latitud > 90)
            throw new ArgumentOutOfRangeException(nameof(latitud), "La latitud debe estar entre -90 y 90.");
        if (longitud < -180 || longitud > 180)
            throw new ArgumentOutOfRangeException(nameof(longitud), "La longitud debe estar entre -180 y 180.");

        string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitud},{longitud}&key={_googleMapsSettings.ApiKey}";
        var respuesta = await _httpClient.GetStringAsync(url);
        using JsonDocument doc = JsonDocument.Parse(respuesta);

        if (doc.RootElement.TryGetProperty("status", out var status) && status.GetString() == "OK")
        {
            if (doc.RootElement.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
            {
                return results[0].GetProperty("formatted_address").GetString() ?? "Dirección no encontrada";
            }
        }
        throw new Exception("No se pudo obtener la dirección.");
    }

    public async Task<decimal> ObtenerDistanciaEntreSucursalColaborador(decimal? latitudOrigen, decimal? longitudOrigen, decimal? latitudDestino, decimal? longitudDestino)
    {
        if (latitudOrigen < -90 || latitudOrigen > 90 || longitudOrigen < -180 || longitudOrigen > 180 ||
            latitudDestino < -90 || latitudDestino > 90 || longitudDestino < -180 || longitudDestino > 180)
        {
            throw new ArgumentOutOfRangeException("Las coordenadas deben estar dentro de los rangos válidos.");
        }

        string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={latitudOrigen},{longitudOrigen}&destinations={latitudDestino},{longitudDestino}&key={_googleMapsSettings.ApiKey}&units=metric";
        var response = await _httpClient.GetStringAsync(url);
        using JsonDocument doc = JsonDocument.Parse(response);

        var rows = doc.RootElement.GetProperty("rows");
        if (rows.GetArrayLength() == 0)
            throw new Exception("No se encontraron resultados en la API.");

        var elements = rows[0].GetProperty("elements");
        if (elements[0].GetProperty("status").GetString() != "OK")
            throw new Exception("No se pudo calcular la distancia.");

        decimal distanceInMeters = elements[0].GetProperty("distance").GetProperty("value").GetDecimal();
        return distanceInMeters / 1000;
    }

    public async Task<Respuesta<decimal>> CalcularDistanciaTotalAjustadaAsync(List<ColaboradorConKmsDto> colaboradores, decimal umbralProximidadKm)
    {
        var respuesta = new Respuesta<decimal>();
        decimal distanciaTotal = 0;
        var colaboradoresProcesados = new HashSet<int>();

        for (int i = 0; i < colaboradores.Count; i++)
        {
            if (colaboradoresProcesados.Contains(colaboradores[i].ColaboradorId))
                continue;

            distanciaTotal += colaboradores[i].DistanciaKms;
            for (int j = i + 1; j < colaboradores.Count; j++)
            {
                decimal distanciaEntreColaboradores = await ObtenerDistanciaEntreSucursalColaborador(
                    colaboradores[i].latitud, colaboradores[i].longitud,
                    colaboradores[j].latitud, colaboradores[j].longitud);

                decimal distanciaMasLarga = Math.Max(colaboradores[i].DistanciaKms, colaboradores[j].DistanciaKms);
                if (distanciaEntreColaboradores <= umbralProximidadKm)
                {
                    distanciaTotal += Math.Abs(colaboradores[j].DistanciaKms - colaboradores[i].DistanciaKms);
                    colaboradoresProcesados.Add(colaboradores[j].ColaboradorId);
                }
                else if (distanciaEntreColaboradores > (distanciaMasLarga + umbralProximidadKm))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = $"Colaborador {colaboradores[j].Nombre} supera la distancia permitida.";
                    return respuesta;
                }
            }
        }
        respuesta.Valido = true;
        respuesta.Datos = distanciaTotal;
        return respuesta;
    }

    public async Task<Respuesta<bool>> ValidarDistanciaEntreColaboradoresExistentesEnViajeAsync(List<ColaboradorConKmsDto> colaboradoresAsignados, List<ColaboradorConKmsDto> nuevosColaboradores, decimal umbralMaximoKm)
    {
        var respuesta = new Respuesta<bool>();
        foreach (var nuevoColaborador in nuevosColaboradores)
        {
            foreach (var colaboradorAsignado in colaboradoresAsignados)
            {
                decimal distanciaEntreColaboradores = await ObtenerDistanciaEntreSucursalColaborador(
                    nuevoColaborador.latitud, nuevoColaborador.longitud,
                    colaboradorAsignado.latitud, colaboradorAsignado.longitud);

                decimal distanciaColaboradorMayor = Math.Max(nuevoColaborador.DistanciaKms, colaboradorAsignado.DistanciaKms);
                if (distanciaEntreColaboradores > (distanciaColaboradorMayor + umbralMaximoKm))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = $"El colaborador {nuevoColaborador.Nombre} está fuera del rango permitido.";
                    return respuesta;
                }
            }
        }
        respuesta.Valido = true;
        return respuesta;
    }
}




