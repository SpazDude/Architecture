using DynamicApi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicApi.Web.Repositories
{
    public interface IRepository<T> where T: IId
    {
        IEnumerable<T> GetAll();
    }
}
