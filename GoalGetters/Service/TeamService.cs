using GoalGetters.Models;

namespace GoalGetters.Service
{
    public class TeamService : ApiService<Team>, ITeamService
    {
        private readonly HttpClient _client;
        public TeamService(HttpClient client) : base(client)
        {
            _client = client;
        }

        public async Task<Team> UpdateTeam(int id, string name, string city, string country)
        {
            // Verifique se os argumentos são válidos
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                throw new ArgumentException("Name, city and country must not be null or empty.");
            }

            // Constrói a URL
            string url = $"{ApiServiceConstants.urlApi}team/update?id={id}&name={name}&city={city}&country={country}";

            // Envia a solicitação PUT
            HttpResponseMessage response = await _client.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Team>();
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
