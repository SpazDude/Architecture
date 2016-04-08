using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository<T>
    {
        Task<Guid[]> Create(string resource, params T[] items);
        Task<bool[]> Exist(string resource, params Guid[] Ids);
        Task<T[]> GetAll(string resource);
        Task<T[]> GetById(string resource, params Guid[] Ids);
        Task<T[]> GetByExample(string resource, dynamic jsonText);
        Task Update(string resource, params T[] items);
        Task Delete(string resource, params Guid[] Ids);
    }
}
