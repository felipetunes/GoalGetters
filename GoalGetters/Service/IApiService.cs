using GoalGetters.Models;
using System.Collections.Generic;
using X.PagedList;

namespace GoalGetters.Service
{
    public interface IApiService<T> where T : class
    {
        Task<List<T>> GetByName(string name);
        Task<T> GetById(int id);
        Task<T> Update(T entity);
        Task<HttpResponseMessage> Delete(int id);
        Task<List<T>> GetAll(string endpoint);
        Task<string> Create(T obj);
    }
}