using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.IoC
{
    [TestClass]
    public class RegisterTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ObjectFactory.Configure(x =>
                {
                    x.For<TesteAberturaDeProcessoDeCotacaoDeFrete>()
                     .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                     .Use<TesteAberturaDeProcessoDeCotacaoDeFrete>()
                     .Ctor<IAberturaDeProcessoDeCotacaoServiceFactory>()
                     .Is<AberturaDeProcessoDeCotacaoDeFreteServiceFactory>();

                    x.For<TesteAberturaDeProcessoDeCotacaoDeMaterial>()
                     .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                     .Use<TesteAberturaDeProcessoDeCotacaoDeMaterial>()
                     .Ctor<IAberturaDeProcessoDeCotacaoServiceFactory>()
                     .Is<AberturaDeProcessoDeCotacaoDeMaterialServiceFactory>();

                }
                );
        }
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
                    typeof(IAberturaDeProcessoDeCotacaoServiceFactory)
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
                    typeof (IComunicacaoSap)
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
            VerificaInterfacesRegistradas(typeof(IFornecedores), "BsBios.Portal.Infra.Repositories.Contracts",interfacesDesconsideradas );
        }

        [TestMethod]
        public void ConsigoInstanciarServicoDeAberturaDoProcessoDeCotacaoDeFrete()
        {
            var testeInstancia = ObjectFactory.GetInstance<TesteAberturaDeProcessoDeCotacaoDeFrete>();
            var aberturaService = testeInstancia.Construir();

            Assert.IsNotNull(aberturaService);
            Assert.IsInstanceOfType(aberturaService, typeof(AberturaDeProcessoDeCotacaoDeFrete));
        }

        [TestMethod]
        public void ConsigoInstanciarServicoDeAberturaDoProcessoDeCotacaoDeMaterial()
        {
            var testeInstancia = ObjectFactory.GetInstance<TesteAberturaDeProcessoDeCotacaoDeMaterial>();
            var aberturaService = testeInstancia.Construir();

            Assert.IsNotNull(aberturaService);
            Assert.IsInstanceOfType(aberturaService, typeof(AberturaDeProcessoDeCotacaoDeMaterial));
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeProcessoDeCotacaoDeFrete()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeFreteAberturaController>();
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeProcessoDeCotacaoDeMaterial()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeMaterialAberturaController>();
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeFechamentoDeProcessoDeCotacaoDeFrete()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeFreteFechamentoController>();
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ConsigoInstanciarControllerDeAberturaDeFechamentoDeCotacaoDeMaterial()
        {
            var controller = ObjectFactory.GetInstance<ProcessoDeCotacaoDeMaterialFechamentoController>();
            Assert.IsNotNull(controller);
        }


    }

    internal class TesteAberturaDeProcessoDeCotacaoDeFrete
    {
        private readonly IAberturaDeProcessoDeCotacaoServiceFactory _aberturaDeProcessoDeCotacaoDeFreteServiceFactory;
        public TesteAberturaDeProcessoDeCotacaoDeFrete(IAberturaDeProcessoDeCotacaoServiceFactory aberturaDeProcessoDeCotacaoDeFreteServiceFactory)
        {
            _aberturaDeProcessoDeCotacaoDeFreteServiceFactory = aberturaDeProcessoDeCotacaoDeFreteServiceFactory;
        }

        public IAberturaDeProcessoDeCotacaoService Construir()
        {
            return _aberturaDeProcessoDeCotacaoDeFreteServiceFactory.Construir();
        }
    }

    internal class TesteAberturaDeProcessoDeCotacaoDeMaterial
    {
        private readonly IAberturaDeProcessoDeCotacaoServiceFactory _aberturaDeProcessoDeCotacaoDeMaterialServiceFactory;
        public TesteAberturaDeProcessoDeCotacaoDeMaterial(IAberturaDeProcessoDeCotacaoServiceFactory aberturaDeProcessoDeCotacaoDeMaterialServiceFactory)
        {
            _aberturaDeProcessoDeCotacaoDeMaterialServiceFactory = aberturaDeProcessoDeCotacaoDeMaterialServiceFactory;
        }

        public IAberturaDeProcessoDeCotacaoService Construir()
        {
            return _aberturaDeProcessoDeCotacaoDeMaterialServiceFactory.Construir();
        }

    }


}
