using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class DynamicMemoryRepository : IRepository<object>
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, dynamic>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, dynamic>>();

        private IIdFactory _idFactory;

        public DynamicMemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public Task<IEnumerable<Guid>> Create(string resource, params dynamic[] items)
        {
            return Task<IEnumerable<Guid>>.Run(() =>
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
                return items.Select(item => (Guid)(item.Id));
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

        public Task<IEnumerable<bool>> Exist(string resource, params Guid[] Ids)
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

        public Task<IEnumerable<dynamic>> GetById(string resource, params Guid[] Ids)
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
                    dictionary[resource][item.Id] = item;
                });
            });
        }
    }

    public interface IIdentifier
    {
        Guid Id { get; set; }
    }
}
