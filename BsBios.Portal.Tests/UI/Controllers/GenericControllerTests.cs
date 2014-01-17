using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.UI.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class GenericControllerTests
    {
        [TestMethod]
        public void TodosControllersTemSecurityFilter()
        {
            Assembly myAssembly = Assembly.GetAssembly(typeof(AccountController));

            var controllers = (from t in myAssembly.DefinedTypes
                              where t.IsClass && t.Namespace == "BsBios.Portal.UI.Controllers"
                              && t.IsSubclassOf(typeof(Controller))
                              select t).ToList();

            controllers.RemoveAll(x => new List<Type>
                {
                    typeof(AccountController), 
                    typeof(GerenciadorUsuarioController),
                    typeof(HomeController),
                    typeof(CriptografiaController),
                    typeof(MonitorDeOrdemDeTransporteController),
                    typeof(MunicipioController),
                    typeof(BaseController),
                    typeof(RelatorioDeProcessoDeCotacaoDeFreteVisualizacaoController),
                }.Select(i => i.GetTypeInfo()).Contains(x));

            var controllersSemSecurityFilter = (from controller 
                                                    in controllers let atributos = Attribute.GetCustomAttributes(controller) 
                                                where atributos.All(x => x.GetType() != typeof (SecurityFilter)) 
                                                select controller).Cast<Type>().ToList();

            foreach (var type in controllersSemSecurityFilter)
            {
                Console.WriteLine(type.Name);
            }
            
            Assert.AreEqual(0,controllersSemSecurityFilter.Count);


        }
    }
}
