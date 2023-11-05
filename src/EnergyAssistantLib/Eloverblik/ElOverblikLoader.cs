using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using UlfenDk.EnergyAssistant.Config;
using UlfenDk.EnergyAssistant.Repository;
using ArgumentNullException = System.ArgumentNullException;

namespace UlfenDk.EnergyAssistant.Eloverblik;

public class ElOverblikLoader
{
    private class JsonAuthenticationResult
    {
        [JsonPropertyName("result")]
        public required string Result { get; set; }
    }

    private const string BaseAddress = "https://api.eloverblik.dk/";

    private readonly EnergyAssistantRepository _repository;
    private readonly ILogger<ElOverblikLoader> _logger;


    private DateTimeOffset _tokenExpiresAt = default;
    private string _accessToken = "";
    
    public ElOverblikLoader(EnergyAssistantRepository repository, ILogger<ElOverblikLoader> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ElOverblikResult?> GetUsageForPeriod(DateOnly from, DateOnly to)
    {
        try
        {
            var settings = await _repository.GetElOverblikSettingsAsync();
            
            _logger.LogInformation("Fetching usage for period {from} {to}.", from, to);
            await AuthenticateAsync();

            using var httpClient = GetHttpClient(_accessToken);
            var response = await httpClient.PostAsJsonAsync(
                $"/api/meterdata/gettimeseries/{from:yyyy-MM-dd}/{to:yyyy-MM-dd}/Hour",
                new
                {
                    meteringPoints = new
                    {
                        meteringPoint = new string[]
                        {
                            settings.MeteringPointId
                        }
                    }
                });

            if (response.IsSuccessStatusCode)
            {
                var jsonStream = await response.Content.ReadAsStreamAsync();
                
                var result = await JsonSerializer.DeserializeAsync<ElOverblikData>(jsonStream);

                _logger.LogInformation("Successfully fetched usage data.");
                
                return result?.Results?.FirstOrDefault() ?? null;
            }
            
            _logger.LogError("Failed to fetch usage data.");

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch usage data: {message}", ex.Message);

            return null;
        }
    }
    
    private async Task AuthenticateAsync()
    {
        if (DateTimeOffset.Now.AddMinutes(5) > _tokenExpiresAt)
        {
            try
            {
                var settings = await _repository.GetElOverblikSettingsAsync();
                
                _logger.LogInformation("Refresh token is invalid or non-existing.");
                
                using var httpClient = GetHttpClient(settings.ApiToken);

                var result = await httpClient.GetFromJsonAsync<JsonAuthenticationResult>("api/token");

                if (result is not null)
                {
                    _tokenExpiresAt = DateTimeOffset.Now.AddHours(24);
                    _accessToken = result.Result;
                    _logger.LogInformation("New refresh token acquired, valid until {expiry}", _tokenExpiresAt);
                }
                else
                {
                    _logger.LogError("Failed to refresh token.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh token: {message}", ex.Message);
            }
        }
    }

    private HttpClient GetHttpClient(string token)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseAddress)
        };
        
        httpClient.DefaultRequestHeaders.Add("Authentication", $"Bearer {token}");

        return httpClient;
    }
}