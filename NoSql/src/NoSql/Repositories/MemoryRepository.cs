using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MemoryRepository : IRepository
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, dynamic>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, dynamic>>();

        private IIdFactory _idFactory;

        public MemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public Task<Guid[]> Create(string resource, params dynamic[] items)
        {
            return Task<Guid[]>.Run(() =>
            {
                foreach (dynamic item in items)
                {
                    var guid = _idFactory.NewGuid();
                    item.Id = guid;
                    if (!dictionary.ContainsKey(resource))
                    {
                        dictionary.TryAdd(resource, new ConcurrentDictionary<Guid, dynamic>());
                    }
                    dictionary[resource].AddOrUpdate(guid, g => item, (g, i) => item);
                }
                return items.Select(item => (Guid)(item.Id)).ToArray();
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

        public Task<dynamic[]> GetAll(string resource)
        {
            return Task<string[]>.Run(() =>
            {
                if (dictionary.Keys.Contains(resource))
                    return dictionary[resource].Values.ToArray();
                else
                    return new dynamic[0];
            });
        }

        public Task<dynamic[]> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic[]> GetById(string resource, params Guid[] Ids)
        {
            return Task<string[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]).ToArray();
            });
        }

        public async Task Update(string resource, params dynamic[] items)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(items, item =>
                {
                    dictionary[resource][item.Id] = item;
                });
            });
        }
    }
}
