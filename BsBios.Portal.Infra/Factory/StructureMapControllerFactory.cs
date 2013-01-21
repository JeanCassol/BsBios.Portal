using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace BsBios.Portal.Infra.Factory
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                Type controllerType = base.GetControllerType(requestContext, controllerName);
                return ObjectFactory.GetInstance(controllerType) as IController;
            }
            catch (Exception)
            {
                //Use the default logic
                return base.CreateController(requestContext, controllerName);
            }
        }
    }
}