using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<Guid>> Create(string resource, params T[] items);
        Task<IEnumerable<bool>> Exist(string resource, params Guid[] Ids);
        Task<IEnumerable<T>> GetAll(string resource);
        Task<IEnumerable<T>> GetById(string resource, params Guid[] Ids);
        Task<IEnumerable<T>> GetByExample(string resource, dynamic jsonText);
        Task Update(string resource, params T[] items);
        Task Delete(string resource, params Guid[] Ids);
    }
}
