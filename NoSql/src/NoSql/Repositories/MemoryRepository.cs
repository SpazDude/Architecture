using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public Task<IEnumerable<Guid>> Create(params T[] item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(params Guid[] Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<bool>> Exist(params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetByExample(string jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetById(params Guid[] Id)
        {
            throw new NotImplementedException();
        }

        public Task Update(params T[] item)
        {
            throw new NotImplementedException();
        }
    }
}
