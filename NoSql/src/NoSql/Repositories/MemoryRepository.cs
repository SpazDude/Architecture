using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MemoryRepository : IRepository
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<ObjectId, dynamic>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<ObjectId, dynamic>>();

        private IIdFactory _idFactory;

        public MemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public async Task Create(string resource, params dynamic[] items)
        {
            if (!dictionary.ContainsKey(resource))
            {
                dictionary.TryAdd(resource, new ConcurrentDictionary<ObjectId, dynamic>());
            }
            var tasks = items.Select(async x => await Task.Run(() =>
            {
                var id = _idFactory.NewId();
                x._id = id;
                dictionary[resource].AddOrUpdate(id, g => x, (g, i) => x);
            }));
            await Task.WhenAll(tasks);   
        }

        public async Task Delete(string resource, params ObjectId[] Ids)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(Ids, id =>
                {
                    object value;
                    dictionary[resource].TryRemove(id, out value);
                });
            });
        }

        public Task<IEnumerable<bool>> Exist(string resource, params ObjectId[] Ids)
        {
            return Task<bool[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource].ContainsKey(id));
            });
        }

        public Task<IEnumerable<dynamic>> GetAll(string resource)
        {
            return Task<IEnumerable<string>>.Run(() =>
            {
                return dictionary[resource].Values.AsEnumerable();
            });
        }

        public Task<IEnumerable<dynamic>> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dynamic>> GetById(string resource, params ObjectId[] Ids)
        {
            return Task<IEnumerable<string>>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]);
            });
        }

        public async Task Update(string resource, params dynamic[] items)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(items, item =>
                {
                    dictionary[resource][item._id] = item;
                });
            });
        }
    }

    public interface IIdentifier
    {
        ObjectId Id { get; set; }
    }
}
