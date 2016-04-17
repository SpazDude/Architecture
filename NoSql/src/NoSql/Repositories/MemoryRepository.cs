using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MemoryRepository<T> : IRepository<T> where T: IId
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<Guid, T>> dictionary =
            new ConcurrentDictionary<string, ConcurrentDictionary<Guid, T>>();

        private IIdFactory _idFactory;

        public MemoryRepository(IIdFactory idFactory)
        {
            _idFactory = idFactory;
        }

        public async Task Create(string resource, params T[] items)
        {
            if (!dictionary.ContainsKey(resource))
            {
                dictionary.TryAdd(resource, new ConcurrentDictionary<Guid, T>());
            }
            var tasks = items.Select(async x => await Task.Run(() =>
            {
                var id = _idFactory.NewId();
                x.Id = id;
                dictionary[resource].AddOrUpdate(id, g => x, (g, i) => x);
            }));
            await Task.WhenAll(tasks);   
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

        public Task<IEnumerable<bool>> Exist(string resource, params Guid[] Ids)
        {
            return Task<bool[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource].ContainsKey(id));
            });
        }

        public Task<IEnumerable<T>> GetAll(string resource)
        {
            return Task<IEnumerable<string>>.Run(() =>
            {
                return dictionary[resource].Values.AsEnumerable();
            });
        }

        public Task<IEnumerable<T>> GetByExample(string resource, T jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetById(string resource, params Guid[] Ids)
        {
            return Task<IEnumerable<string>>.Run(() =>
            {
                return Ids.Select(id => dictionary[resource][id]);
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
}
