using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.IoC
{
    [TestClass]
    public class RegisterTests
    {
        private void VerificaInterfacesRegistradas(Type type, string @namespace, IEnumerable<Type> interfaceDesconsideradas = null)
        {
            Assembly myAssembly = Assembly.GetAssembly(type);

            var interfaces = (from t in myAssembly.DefinedTypes
                        where t.IsInterface && t.Namespace == @namespace
                        select t).ToList();

            if (interfaceDesconsideradas != null)
            {
                interfaces.RemoveAll(x => interfaceDesconsideradas.Select(i => i.GetTypeInfo()).Contains(x));
            }

            Assert.IsTrue(interfaces.Any());

            foreach (TypeInfo tipo in interfaces)
            {
                var objeto = ObjectFactory.GetInstance(tipo);
                Assert.IsNotNull(objeto);
            }
        }

        [TestMethod]
        public void TodosApplicationServicesEstaoRegistrados()
        {
            VerificaInterfacesRegistradas(typeof(ICadastroCondicaoPagamento), "BsBios.Portal.Application.Services.Contracts");
        }

        [TestMethod]
        public void TodasQueriesEstaoRegistradas()
        {
            VerificaInterfacesRegistradas(typeof(IConsultaCondicaoPagamento), "BsBios.Portal.Application.Queries.Contracts");
        }

        [TestMethod]
        public void TodosDomainServicesEstaoRegistrados()
        {
            VerificaInterfacesRegistradas(typeof(ISelecionaFornecedor), "BsBios.Portal.Domain.Services.Contracts");
        }

        [TestMethod]
        public void TodosInfraServicesEstaoRegistrados()
        {
            VerificaInterfacesRegistradas(typeof(IAccountService), "BsBios.Portal.Infra.Services.Contracts");
        }

        [TestMethod]
        public void TodosRepositoriosEstaoRegistrados()
        {
            var interfacesDesconsideradas = new List<Type>()
                {
                    typeof (IReadOnlyRepository<>),
                    typeof (ICompleteRepository<>)
                };
            VerificaInterfacesRegistradas(typeof(IFornecedores), "BsBios.Portal.Infra.Repositories.Contracts",interfacesDesconsideradas );
        }


    }
}
