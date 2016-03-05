using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MemoryRepository : IRepository
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, object>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, object>>();

        public MemoryRepository()
        {

        }

        public Task<Guid[]> Create(string resource, params string[] items) 
        {
            return Task<Guid[]>.Run(() =>
            {
                return items.Select(jsonString =>
                {
                    var guid = Guid.NewGuid();
                    var converter = new ExpandoObjectConverter();
                    dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(jsonString, converter);
                    item.Id = guid;
                    dictionary[resource][item.Id] = item;
                    return guid;
                }).ToArray();
            });
        }

        public async Task Delete(string resource, params Guid[] Ids) 
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

        public Task<bool[]> Exist(string resource, params Guid[] Ids) 
        {
            return Task<bool[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource].ContainsKey(id)).ToArray();
            });
        }

        public Task<string[]> GetAll(string resource) 
        {
            return Task<string[]>.Run(() =>
            {
                return dictionary[resource].Values.Cast<string>().ToArray();
            });
        }

        public Task<string[]> GetByExample(string resource, string jsonText) 
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetById(string resource, params Guid[] Ids) 
        {
            return Task<string[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]).Cast<string>().ToArray();
            });
        }

        public async Task Update(string resource, params string[] items) 
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(items, jsonString =>
                {
                    var converter = new ExpandoObjectConverter();
                    dynamic item = JsonConvert.DeserializeObject<ExpandoObject>(jsonString, converter);
                    dictionary[resource][item.Id] = item;
                });
            });
        }
    }
}
