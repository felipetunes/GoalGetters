using GoalGetters.Models;
using System.Collections.Generic;

namespace GoalGetters.Interfaces
{
    public interface IApiService<T>
    {
        Task<IEnumerable<T>> GetByName(string name);
    }
}