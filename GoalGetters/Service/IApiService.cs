using GoalGetters.Models;
using System.Collections.Generic;
using X.PagedList;

namespace GoalGetters.Service
{
    public interface IApiService<T>
    {
        Task<List<T>> GetByName(string name);
        Task<T> GetById(int name);
        Task<T> Update<T>(int id, string name, string city, string country, int? idteam = null, DateTime? birth = null, string height = null, string position = null);
        Task<HttpResponseMessage> Delete(int id, string entity);
        Task<List<T>> GetAll<T>(string endpoint);
        Task<string> Insert<T>(T obj);
    }
}