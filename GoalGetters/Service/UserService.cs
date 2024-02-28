using GoalGetters.Models;
using Newtonsoft.Json;
using System.Text;

namespace GoalGetters.Service
{
    public class UserService : ApiService<User>, IUserService
    {
        private readonly HttpClient _client;
        public UserService(HttpClient client) : base(client)
        {
            _client = client;
        }

        public async Task<User> Login(string username, string password)
        {
            string url = $"{ApiServiceConstants.urlApi}user/login";

            // Cria um objeto com os dados de login
            var loginData = new { Username = username, Password = password };

            // Converte o objeto para JSON
            var json = JsonConvert.SerializeObject(loginData);

            // Cria um novo StringContent que contém os dados de login como JSON
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<User>();
                return data;
            }
            else
            {
                // Retorna um usuário vazio
                return new User();
            }
        }
    }
}
