using GoalGetters.Models;
using System.Collections.Generic;

namespace GoalGetters.Service
{
    public interface IApiService<T>
    {
        Task<IEnumerable<T>> GetByName(string name);
        Task<T> GetById(int name);
        Task<T> UpdatePlayer(int id, string name, int idteam, string city, string country, DateTime birth, string height);

    }
}