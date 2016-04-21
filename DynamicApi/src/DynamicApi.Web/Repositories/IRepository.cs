using DynamicApi.Web.Models;
using System.Collections.Generic;

namespace DynamicApi.Web.Repositories
{
    public interface IRepository<T> where T: IId
    {
        IEnumerable<T> GetAll();
    }
}
