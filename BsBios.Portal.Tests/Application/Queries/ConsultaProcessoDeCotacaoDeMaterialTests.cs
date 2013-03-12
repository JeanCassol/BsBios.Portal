using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
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
            Assert.IsNotNull(processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoListagem.DataTermino);
        }

        [TestMethod]
        public void QuandoListaProcessosDeCotacaoDeUmDeterminadoFornecedorRetornaApenasProcessosDesteFornecedor()
        {
            //crio dois fornecedores e adiciono cada um deles em duas cotações distintas
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao3 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao4 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();

            processoDeCotacao1.Atualizar(DateTime.Today);
            processoDeCotacao1.AdicionarFornecedor(fornecedor1);
            processoDeCotacao1.Abrir();

            processoDeCotacao2.Atualizar(DateTime.Today);
            processoDeCotacao2.AdicionarFornecedor(fornecedor1);
            processoDeCotacao2.Abrir();

            processoDeCotacao3.Atualizar(DateTime.Today);
            processoDeCotacao3.AdicionarFornecedor(fornecedor2);
            processoDeCotacao3.Abrir();
            
            processoDeCotacao4.Atualizar(DateTime.Today);
            processoDeCotacao4.AdicionarFornecedor(fornecedor2);
            processoDeCotacao4.Abrir();

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>()
                {processoDeCotacao1, processoDeCotacao2, processoDeCotacao3, processoDeCotacao4});

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            //consulta filtrando pelo fornecedor1
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10 }, 
               new ProcessoCotacaoMaterialFiltroVm()
                   {
                       CodigoFornecedor  = fornecedor1.Codigo
                   });
            Assert.AreEqual(2,kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().ToList();
            //verifico que está retornado os dois processos vinculados ao fornecedor 1
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao1.Id));
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao2.Id));
        }

        [TestMethod]
        public void QuandoConsultaProcessosDeCotacaoDeUmDeterminadoFornecedorNaoConsideraOsProcessosNaoIniciados()
        {
            //crio um fornecedor e adiciono ele em uma cotação aberta e uma não iniciada
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();

            processoDeCotacao1.Atualizar(DateTime.Today);
            processoDeCotacao1.AdicionarFornecedor(fornecedor1);
            processoDeCotacao1.Abrir();

            processoDeCotacao2.Atualizar(DateTime.Today);
            processoDeCotacao2.AdicionarFornecedor(fornecedor1);

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>() { processoDeCotacao1, processoDeCotacao2});

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            //consulta filtrando pelo fornecedor
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10 },
               new ProcessoCotacaoMaterialFiltroVm()
               {
                   CodigoFornecedor = fornecedor1.Codigo
               });
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().ToList();
            //verifico que está retornado apenas o processo que foi aberto
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao1.Id));
            
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
