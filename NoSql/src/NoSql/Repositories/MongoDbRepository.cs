using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public class MongoDbRepository : IRepository<object>
    {
        private string _connectionString;
        private string _database;
        private IIdFactory _idFactory;

        public MongoDbRepository(IIdFactory idFactory, string connectionString, string database)
        {
            _idFactory = idFactory;
            _connectionString = connectionString;
            _database = database;
        }

        private IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_connectionString);
            return client.GetDatabase(_database);
        }

        private IMongoCollection<object> GetCollection(string resourceName)
        {
            return GetDatabase().GetCollection<dynamic>(resourceName);
        }

        public void DeleteResources(string resource)
        {
            GetDatabase().DropCollection(resource);
        }

        public async Task<Guid[]> Create(string resource, params dynamic[] items)
        {
            var collection = GetCollection(resource);
            foreach (var item in items)
            {
                item.Id = _idFactory.NewGuid();
            }
            await collection.InsertManyAsync(items);
            return items.Select(x => (Guid)x.Id).ToArray();
        }

        public async Task Delete(string resource, params Guid[] Ids)
        {
            var collection = GetCollection(resource);
            await collection.DeleteManyAsync(x => Ids.Any(y => ((IId)x).Id == y));
        }

        public Task<bool[]> Exist(string resource, params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic[]> GetAll(string resource)
        {
            var collection = GetCollection(resource);
            var result = await collection.FindAsync(x => true);
            return result.ToList().ToArray();
        }

        public Task<dynamic[]> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic[]> GetById(string resource, params Guid[] Ids)
        {
            var collection = GetCollection(resource);
            var result = await collection.FindAsync(x => Ids.Any(y => ((IId)x).Id == y));
            return result.ToList().ToArray();
        }

        public async Task Update(string resource, params dynamic[] items)
        {
            var collection = GetCollection(resource);
            var tasks = items.Select(async x =>
                 await collection.UpdateOneAsync(Builders<dynamic>.Filter.Eq("Id", ((IId)x).Id), x)
            ).ToArray();
            await Task.WhenAll(tasks);
        }        
    }
}
