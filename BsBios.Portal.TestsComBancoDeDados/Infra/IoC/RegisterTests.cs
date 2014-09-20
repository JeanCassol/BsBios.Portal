using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.IoC
{
    [TestClass]
    public class RegisterTests
    {
        private void VerificaInterfacesRegistradas(Type type, string @namespace, IEnumerable<Type> interfaceDesconsideradas = null)
        {
            int interfacesNaoRegistradas = 0;
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
                var objeto = ObjectFactory.TryGetInstance(tipo);
                if (objeto == null)
                {
                    interfacesNaoRegistradas++;
                    Console.WriteLine("interface Não registrada: " + tipo.Namespace + "." + tipo.Name);
                }
            }

            if (interfacesNaoRegistradas > 0)
            {
                Assert.Fail("Existem interfaces não registradas no namespace " + @namespace);
            }
        }

        [TestMethod]
        public void TodosApplicationServicesEstaoRegistrados()
        {
            VerificaInterfacesRegistradas(typeof(ICadastroCondicaoPagamento), "BsBios.Portal.Application.Services.Contracts",new List<Type>
                {
                    typeof(IAberturaDeProcessoDeCotacaoServiceFactory),
                    typeof(IFechamentoDeProcessoDeCotacaoServiceFactory),
                    typeof(IReenviadorDeEmailDoProcessoDeCotacaoFactory)
                });
        }

        [TestMethod]
        public void TodasQueriesEstaoRegistradas()
        {
            VerificaInterfacesRegistradas(typeof(IConsultaCondicaoPagamento), "BsBios.Portal.Application.Queries.Contracts");
        }

        [TestMethod]
        public void TodosDomainServicesEstaoRegistrados()
        {
            //VerificaInterfacesRegistradas(typeof(ISelecionaFornecedor), "BsBios.Portal.Domain.Services.Contracts");
        }

        [TestMethod]
        public void TodosInfraServicesEstaoRegistrados()
        {
            //removi IComunicacaoSap dos testes porque estão sendo instanciados manualmente. Se isto mudar tem que remover
            var interfacesDesconsideradas = new List<Type>()
                {
                    typeof (IComunicacaoSap),
                    typeof(IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory)
                };
            VerificaInterfacesRegistradas(typeof(IAccountService), "BsBios.Portal.Infra.Services.Contracts",interfacesDesconsideradas);
        }

        [TestMethod]
        public void TodosRepositoriosEstaoRegistrados()
        {
            var interfacesDesconsideradas = new List<Type>()
                {
                    typeof (IReadOnlyRepository<>),
                    typeof (ICompleteRepository<>)
                };
            VerificaInterfacesRegistradas(typeof(IFornecedores), "BsBios.Portal.Domain.Repositories",interfacesDesconsideradas );
        }


    }

}
