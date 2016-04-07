using System;

namespace NoSql.Repositories
{
    public interface IIdFactory
    {
        System.Guid NewGuid();
    }

    public class IdFactory : IIdFactory
    {
        public Guid NewGuid() 
        {
            return Guid.NewGuid();
        }
    }

    public class TestIdFactory : IIdFactory
    {
        private byte count = 0;

        public Guid NewGuid()
        {
            return new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++);
        }
    }
}