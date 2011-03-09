using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace CommunitySite.Core.Dependencies
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, System.Type controllerType)
        {
            IController result = null;
            try
            {
                if (controllerType == null) return base.GetControllerInstance(requestContext, controllerType);
                result = ObjectFactory.GetInstance(controllerType) as Controller;

            }
            catch (StructureMapException)
            {
                Debug.WriteLine(ObjectFactory.WhatDoIHave());
                throw;
            }

            return result;
        }
    }
}