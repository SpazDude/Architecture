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

        private IMongoCollection<BsonDocument> GetCollection(string resourceName)
        {
            return GetDatabase().GetCollection<BsonDocument>(resourceName);
        }

        public void DeleteResources(string resource)
        {
            GetDatabase().DropCollection(resource);
        }

        public async Task<IEnumerable<Guid>> Create(string resource, params dynamic[] items)
        {
            var collection = GetCollection(resource);
            foreach (var item in items)
            {
                item.Id = _idFactory.NewGuid();
            }
            var bsonItems = items.Select(x => new BsonDocument(x));
            await collection.InsertManyAsync(bsonItems);
            return items.Select(x => (Guid)x.Id);
        }

        public async Task Delete(string resource, params Guid[] Ids)
        {
            var collection = GetCollection(resource);
            await collection.DeleteManyAsync(x => Ids.Any(y => ((IId)x).Id == y));
        }

        public Task<IEnumerable<bool>> Exist(string resource, params Guid[] Ids)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetAll(string resource)
        {
            var collection = GetCollection(resource);
            var result = await collection.FindAsync(x => true);
            return result.ToList().ToArray();
        }

        public Task<IEnumerable<dynamic>> GetByExample(string resource, dynamic jsonText)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> GetById(string resource, params Guid[] Ids)
        {
            var collection = GetCollection(resource);
            var result = await collection.FindAsync(x => Ids.Any(y => ((IId)x).Id == y));
            return result.ToList().ToArray();
        }

        public async Task Update(string resource, params dynamic[] items)
        {
            var collection = GetCollection(resource);
            var bsonItems = items.Select(x => new BsonDocument(x));
            var tasks = bsonItems.Select(async x =>
                 await collection.UpdateOneAsync(Builders<BsonDocument>.Filter.Eq("Id", ((IId)x).Id), x)
            ).ToArray();
            await Task.WhenAll(tasks);
        }        
    }
}
