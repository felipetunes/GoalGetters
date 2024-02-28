using GoalGetters.Models;

namespace GoalGetters.Service
{
    public interface IUserService : IApiService<User>
    {
        Task<User> Login(string username, string password);
    }
}
