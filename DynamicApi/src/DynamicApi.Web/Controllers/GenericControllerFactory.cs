using Microsoft.AspNet.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using DynamicApi.Web.Models;

namespace DynamicApi.Web.Controllers
{
    public class GenericControllerFactory : DefaultControllerFactory
    {
        private IControllerActivator _controllerActivator;
        private IEnumerable<IControllerPropertyActivator> _propertyActivators;
        public GenericControllerFactory(IControllerActivator controllerActivator, IEnumerable<IControllerPropertyActivator> propertyActivators) : base(controllerActivator, propertyActivators)
        {
            _controllerActivator = controllerActivator;
            _propertyActivators = propertyActivators;
        }

        public override object CreateController(ActionContext context)
        {
           var modelName = context.RouteData.Values["model"].ToString();

            var type = typeof(IId);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && string.Compare(p.Name, modelName, true) == 0);
            
            var controllerType = typeof(Controller<>).MakeGenericType(types.First());

            var controller = _controllerActivator.Create(context, controllerType);
            foreach (var propertyActivator in _propertyActivators)
            {
                propertyActivator.Activate(context, controller);
            }

            return controller;
            //return base.CreateController(actionContext);
        }
    }
}
