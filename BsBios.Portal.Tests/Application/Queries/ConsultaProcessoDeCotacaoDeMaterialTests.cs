using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeMaterialTests
    {
        [TestMethod]
        public void ConsultaProcessoRetornaObjetoEsperado()
        {
            Tests.Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            ProcessoCotacaoMaterialCadastroVm processoCotacaoMaterialCadastroVm = consultaProcesso.ConsultaProcesso(processoDeCotacaoDeMaterial.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoCotacaoMaterialCadastroVm.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Codigo,processoCotacaoMaterialCadastroVm.CodigoMaterial);
            Assert.IsTrue(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoCotacaoMaterialCadastroVm.DataLimiteRetorno);
            Assert.AreEqual("Não Iniciado",processoCotacaoMaterialCadastroVm.DescricaoStatus);
        }

        [TestMethod]
        public void ConsultaListagemDeProcessosRetornaObjetoEsperado()
        {
            Tests.Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10}, null);

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoMaterialListagemVm  processoListagem = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().First();
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoListagem.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Codigo, processoListagem.CodigoMaterial);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Descricao, processoListagem.Material);
            Assert.AreEqual(1000, processoListagem.Quantidade);
            Assert.AreEqual("Não Iniciado", processoListagem.Status);
            Assert.IsNull(processoListagem.DataTermino);
        }

        [TestMethod]
        public void ConsultaFornecedoresParticipantesRetornaObjetoEsperado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            Console.WriteLine("INICIANDO CONSULTA");
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.FornecedoresParticipantes(processoDeCotacaoDeMaterial.Id);
            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            IList<FornecedorCadastroVm> viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor1.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor2.Codigo));

        }
    }
}
