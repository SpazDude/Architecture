using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository
    {
        Task<Guid[]> Create(string resource, params dynamic[] items);
        Task<bool[]> Exist(string resource, params Guid[] Ids);
        Task<dynamic[]> GetAll(string resource);
        Task<dynamic[]> GetById(string resource, params Guid[] Ids);
        Task<dynamic[]> GetByExample(string resource, dynamic jsonText);
        Task Update(string resource, params dynamic[] items);
        Task Delete(string resource, params Guid[] Ids);
    }
}
