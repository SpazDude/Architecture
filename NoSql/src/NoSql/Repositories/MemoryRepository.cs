using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
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

    public class JObjectMemoryRepository : IRepository<JObject>
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, JObject>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, JObject>>();

        private IIdFactory _idFactory;

        public JObjectMemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public Task<Guid[]> Create(string resource, params JObject[] items)
        {
            return Task<Guid[]>.Run(() =>
            {
                foreach (dynamic item in items)
                {
                    var guid = _idFactory.NewGuid();
                    item.Id = guid;
                    if (!dictionary.ContainsKey(resource))
                    {
                        dictionary.TryAdd(resource, new ConcurrentDictionary<Guid, JObject>());
                    }
                    dictionary[resource].AddOrUpdate(guid, g => item, (g, i) => item);
                }
                return items.Select(item => (Guid)(item.GetValue("Id").Value<Guid>())).ToArray();
            });
        }

        public async Task Delete(string resource, params Guid[] Ids)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(Ids, id =>
                {
                    JObject value;
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

        public Task<JObject[]> GetAll(string resource)
        {
            return Task<string[]>.Run(() =>
            {
                if (dictionary.Keys.Contains(resource))
                    return dictionary[resource].Values.ToArray();
                else
                    return new JObject[0];
            });
        }

        public Task<JObject[]> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<JObject[]> GetById(string resource, params Guid[] Ids)
        {
            return Task<string[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]).ToArray();
            });
        }

        public async Task Update(string resource, params JObject[] items)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(items, item =>
                {
                    var id = item.GetValue("Id").Value<Guid>();
                    dictionary[resource][id] = item;
                });
            });
        }
    }


    public class MemoryRepository<T> : IRepository<T> where T: IIdentifier
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, T>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, T>>();

        private IIdFactory _idFactory;

        public MemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public Task<Guid[]> Create(string resource, params T[] items)
        {
            return Task<Guid[]>.Run(() =>
            {
                foreach (dynamic item in items)
                {
                    var guid = _idFactory.NewGuid();
                    item.Id = guid;
                    if (!dictionary.ContainsKey(resource))
                    {
                        dictionary.TryAdd(resource, new ConcurrentDictionary<Guid, T>());
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
                    T value;
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

        public Task<T[]> GetAll(string resource)
        {
            return Task<string[]>.Run(() =>
            {
                if (dictionary.Keys.Contains(resource))
                    return dictionary[resource].Values.ToArray();
                else
                    return new T[0];
            });
        }

        public Task<T[]> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> GetById(string resource, params Guid[] Ids)
        {
            return Task<string[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]).ToArray();
            });
        }

        public async Task Update(string resource, params T[] items)
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
