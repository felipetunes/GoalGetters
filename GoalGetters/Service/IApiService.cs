using GoalGetters.Models;
using System.Collections.Generic;

namespace GoalGetters.Service
{
    public interface IApiService<T>
    {
        Task<IEnumerable<T>> GetByName(string name);
        Task<T> GetById(int name);
    }
}