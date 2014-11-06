using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class CadastroDeQuotaTests: RepositoryTest
    {

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestInitialize]
        public void TesteInitialize()
        {
            RemoveQueries.RemoverTodosCadastros();
        }

        [TestMethod]
        public void PossoAtualizarUmaQuotaComCarregamentoInserindoUmDescarregamentoDoMesmoProduto()
        {
            //PREPARAÇÃO DOS DADOS

            Quota quotaDeCarregamento = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today, EntidadesPersistidas.ObterFarelo());
            DefaultPersistedObjects.PersistirQuota(quotaDeCarregamento);

            var cadastroQuota = ObjectFactory.GetInstance<ICadastroQuota>();

            var quotasSalvarVm = new QuotasSalvarVm
            {
                CodigoDoTerminal = quotaDeCarregamento.Terminal.Codigo,
                Data = quotaDeCarregamento.Data,
                Quotas = new List<QuotaSalvarVm>
                {
                    //primeiro quota é a mesma que já foi persistida
                    new QuotaSalvarVm
                    {
                        CodigoFornecedor = quotaDeCarregamento.Fornecedor.Codigo,
                        CodigoMaterial = quotaDeCarregamento.Material.Codigo,
                        FluxoDeCarga = (int) quotaDeCarregamento.FluxoDeCarga,
                        Peso = quotaDeCarregamento.PesoDisponivel
                    },
                    new QuotaSalvarVm
                    {
                        //segunda quota é para o mesmo produto e fornecedor, apenas com fluxo ao contrário (carregamento)                        
                        CodigoFornecedor = quotaDeCarregamento.Fornecedor.Codigo,
                        CodigoMaterial = quotaDeCarregamento.Material.Codigo,
                        FluxoDeCarga = (int) Enumeradores.FluxoDeCarga.Descarregamento,
                        Peso = 200
                    }
                }
            };

            //EXECUCÃO 
            cadastroQuota.Salvar(quotasSalvarVm);

            //ASSERTS 
            var quotas = ObjectFactory.GetInstance<IQuotas>();
            IList<Quota> quotasConsultadas = quotas.FiltraPorData(quotaDeCarregamento.Data).List().OrderBy(x => x.Id).ToList();

            Assert.AreEqual(2, quotasConsultadas.Count);

            //a última quota inserida foi a de descarregamento
            Quota quotaDeDescarregamento = quotasConsultadas.Last();
            Assert.AreEqual(200, quotaDeDescarregamento.PesoDisponivel);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quotaDeDescarregamento.FluxoDeCarga);
        }
    }
}
