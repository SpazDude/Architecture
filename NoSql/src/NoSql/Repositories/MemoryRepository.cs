using Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MemoryRepository<T> : IRepository<T> where T : IId
    {
        private ConcurrentDictionary<Guid, T> dictionary = new ConcurrentDictionary<Guid, T>();

        public MemoryRepository()
        {

        }

        public Task<Guid[]> Create(params T[] items)
        {
            return Task<Guid[]>.Run(() =>
            {
                return items.Select(item =>
                {
                    item.Id = Guid.NewGuid();
                    dictionary[item.Id] = item;
                    return item.Id;
                }).ToArray();
            });
        }

        public async Task Delete(params Guid[] Ids)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(Ids, id =>
                {
                    T value;
                    dictionary.TryRemove(id, out value);
                });
            });
        }

        public Task<bool[]> Exist(params Guid[] Ids)
        {
            return Task<bool[]>.Run(() =>
            {
                return Ids.Select(id => dictionary.ContainsKey(id)).ToArray();
            });
        }

        public Task<T[]> GetAll()
        {
            return Task<T[]>.Run(() =>
            {
                return dictionary.Values.ToArray();
            });
        }

        public Task<T[]> GetByExample(string jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<T[]> GetById(params Guid[] Ids)
        {
            return Task<T[]>.Run(() =>
            {
                return Ids.Select(id => dictionary[id]).ToArray();
            });
        }

        public async Task Update(params T[] items)
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(items, item =>
                {
                    dictionary[item.Id] = item;
                });
            });
        }
    }
}
