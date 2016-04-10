using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSql.Repositories
{
    public interface IId
    {
        Guid Id { get; }
    }
}
