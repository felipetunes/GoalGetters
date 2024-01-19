using GoalGetters.Models;
using System.Collections.Generic;

namespace GoalGetters.Service
{
    public interface IApiService<T>
    {
        Task<IEnumerable<T>> GetByName(string name);
        Task<T> GetById(int name);
        Task<T> UpdateEntity<T>(int id, string name, string city, string country, int? idteam = null, DateTime? birth = null, string height = null);

    }
}