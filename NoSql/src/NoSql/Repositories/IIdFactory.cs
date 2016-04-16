using MongoDB.Bson;
using System;

namespace NoSql.Repositories
{
    public interface IIdFactory
    {
        ObjectId NewId();
    }

    public class IdFactory : IIdFactory
    {
        public ObjectId NewId()
        {
            return ObjectId.GenerateNewId();
        }
    }

    public class TestIdFactory : IIdFactory
    {
        private int count = 0;

        public ObjectId NewId()
        {
            return ObjectId.GenerateNewId(count++);
        }
    }
}