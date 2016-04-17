using System;

namespace NoSql.Repositories
{
    public interface IIdFactory
    {
        Guid NewId();
    }

    public class IdFactory : IIdFactory
    {
        public Guid NewId()
        {
            return Guid.NewGuid();
        }
    }

    public class TestIdFactory : IIdFactory
    {
        private int count = 0;

        public Guid NewId()
        {
            return new Guid(count, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
        }
    }
}