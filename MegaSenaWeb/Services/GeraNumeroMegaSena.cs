using MegaSenaWeb.Interfaces;
using System.Text.Json;

namespace MegaSenaWeb.Services
{
    public class GeraNumeroMegaSena : IGeraNumeroMegaSena
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GeraNumeroMegaSena(IHttpClientFactory httpClientFactory) =>
            _httpClientFactory = httpClientFactory;

        public async Task<IEnumerable<int>> GetNumeroMegaSena()
        {
            using var httpClient = _httpClientFactory.CreateClient("geraNumeroMegaSena");
            var content = await httpClient.GetAsync("/Sorteio");

            if (!content.IsSuccessStatusCode)
            {
                return Enumerable.Empty<int>();
            }

            var result = await content.Content.ReadAsStringAsync();

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<int>>(result);

            return obj;

        }
    }
}
