using MegaSenaWeb.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace MegaSenaWeb.Services
{
    public class GeraNumeroMegaSena : IGeraNumeroMegaSena
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GeraNumeroMegaSena> _logger;


        public GeraNumeroMegaSena(IHttpClientFactory httpClientFactory, ILogger<GeraNumeroMegaSena> logger) {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<IEnumerable<int>> GetNumeroMegaSena()
        {
            try {

                using var httpClient = _httpClientFactory.CreateClient("geraNumeroMegaSena");
                var content = await httpClient.GetAsync("/Sorteio");

                if (!content.IsSuccessStatusCode) {
                    return Enumerable.Empty<int>();
                }

                
                var result = await content.Content.ReadAsStringAsync();
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(result);
                _logger.LogInformation(1, $"Os números sorteados foram {result}");

                return obj;

            }
            catch (Exception ex) {
                _logger.LogError(2, $"Ocorreu um erro ao retornar os números: {ex.Message}" );
                return null;
            }

        }
    }
}
