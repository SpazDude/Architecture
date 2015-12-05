using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository<T> where T : IId
    {
        Task<Guid[]> Create(params T[] items);
        Task<bool[]> Exist(params Guid[] Ids);
        Task<T[]> GetAll() ;
        Task<T[]> GetById(params Guid[] Ids);
        Task<T[]> GetByExample(string jsonText);
        Task Update(params T[] items);
        Task Delete(params Guid[] Ids);
    }
}
