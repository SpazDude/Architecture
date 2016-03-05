using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository
    {
        Task<Guid[]> Create(string resource, params string[] items);
        Task<bool[]> Exist(string resource, params Guid[] Ids);
        Task<string[]> GetAll(string resource);
        Task<string[]> GetById(string resource, params Guid[] Ids);
        Task<string[]> GetByExample(string resource, string jsonText);
        Task Update(string resource, params string[] items);
        Task Delete(string resource, params Guid[] Ids);
    }
}
