using GoalGetters.Models;

namespace GoalGetters.Service
{
    public interface IPlayerService : IApiService<Player>
    {
        Task<List<Player>> GetPlayersByTeamId(int id);
        Task<Player> UpdatePlayer(int id, string name, string city, string country, int idteam, DateTime birth, string height, int position, string imagepath, int shirtnumber);


    }
}
