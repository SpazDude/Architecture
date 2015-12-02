using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository<T> where T : IId
    {
        Task<IEnumerable<Guid>> Create(params T[] items);
        Task<IEnumerable<bool>> Exist(params Guid[] Ids);
        Task<IEnumerable<T>> GetAll() ;
        Task<IEnumerable<T>> GetById(params Guid[] Ids);
        Task<IEnumerable<T>> GetByExample(string jsonText);
        Task Update(params T[] items);
        Task Delete(params Guid[] Ids);
    }
}
