using GoalGetters.Models;
using Newtonsoft.Json;

namespace GoalGetters.Service
{
    public class PlayerService : ApiService<Player>, IPlayerService
    {
        private readonly HttpClient _client;
        public PlayerService(HttpClient client) : base(client)
        {
            _client = client;
        }

        public async Task<List<Player>> GetPlayersByTeamId(int id)
        {
            var response = await GetClient().GetAsync($"{ApiServiceConstants.urlApi}player/getbyidteam/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao obter jogadores: {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<List<Player>>(json);
            return players;
        }

        public async Task<Player> UpdatePlayer(int id, string name, string city, string country, int idteam, DateTime birth, string height, int position, string imagepath, int shirtnumber)
        {
            // Verifique se os argumentos são válidos
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new ArgumentException("Name, city and country must not be null or empty.");
            }

            // Constrói a URL
            string url = $"{ApiServiceConstants.urlApi}player/update?id={id}&name={name}&city={city}&country={country}&idteam={idteam}&birth={birth.ToString("dd/MM/yyyy")}&height={height}&position={position}&imagepath={imagepath}&shirtnumber={shirtnumber}";

            // Envia a solicitação PUT
            HttpResponseMessage response = await _client.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Player>();
                return result;
            }
            else
            {
                // Handle the error
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error updating entity: {response.StatusCode} - {error}");
            }
        }
    }
}
