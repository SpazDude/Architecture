using Microsoft.AspNet.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Infrastructure;
using Microsoft.AspNet.Mvc;

namespace DynamicApi.Web.Controllers
{
    public class GenericControllerActivator : DefaultControllerActivator
    {
        public GenericControllerActivator(ITypeActivatorCache typeActivatorCache) : base(typeActivatorCache)
        {
        }
        public override object Create(ActionContext actionContext, Type controllerType)
        {
            return base.Create(actionContext, controllerType);
        }
    }
}
