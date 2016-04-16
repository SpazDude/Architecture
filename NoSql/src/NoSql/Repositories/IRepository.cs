using Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IRepository
    {
        Task Create(string resource, params dynamic[] items);
        Task<IEnumerable<bool>> Exist(string resource, params ObjectId[] Ids);
        Task<IEnumerable<dynamic>> GetAll(string resource);
        Task<IEnumerable<dynamic>> GetById(string resource, params ObjectId[] Ids);
        Task<IEnumerable<dynamic>> GetByExample(string resource, dynamic jsonText);
        Task Update(string resource, params dynamic[] items);
        Task Delete(string resource, params ObjectId[] Ids);
    }
}
