using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MongoDbRepository : IRepository<object>
    {


        //public void foo(string resourceName)
        //{
        //    var client = new MongoClient();
        //    var database = client.GetDatabase(resourceName);
        //    var collection = database.GetCollection<object>(resourceName);
        //}

        public Task<Guid[]> Create(string resource, params dynamic[] items)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string resource, params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool[]> Exist(string resource, params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic[]> GetAll(string resource)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic[]> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic[]> GetById(string resource, params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public Task Update(string resource, params dynamic[] items)
        {
            throw new NotImplementedException();
        }
    }
}
