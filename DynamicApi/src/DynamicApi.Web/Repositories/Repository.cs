using DynamicApi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicApi.Web.Repositories
{
    public class Repository<T> : IRepository<T> where T : IId, new()
    {
        IList<T> _storage = new List<T> {
            new T { Id = 1 }, new T { Id = 2 }
        };

        public IEnumerable<T> GetAll()
        {
            return _storage;
        }
    }
}
