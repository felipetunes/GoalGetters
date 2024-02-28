using GoalGetters.Models;

namespace GoalGetters.Service
{
    public interface ITeamService : IApiService<Team>
    {
        Task<Team> UpdateTeam(int id, string name, string city, string country);
    }
}