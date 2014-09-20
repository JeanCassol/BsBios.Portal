using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class ProcessoCotacaoIteracoesUsuarioTests: RepositoryTest
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
        [TestMethod]
        public void ConsigoSalvarUmaIteracaoDoUsuarioNoProcessoDeCotacaoEConsultarPosteriormente()
        {
            var repositorio = ObjectFactory.GetInstance<IProcessoCotacaoIteracoesUsuario>();
            var iteracaoUsuario = new ProcessoCotacaoIteracaoUsuario(10);
            try
            {
                UnitOfWorkNh.BeginTransaction();
                repositorio.Save(iteracaoUsuario);
                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            ProcessoCotacaoIteracaoUsuario iteracaoUsuarioConsultada = repositorio.BuscaPorIdParticipante(iteracaoUsuario.IdFornecedorParticipante);
            Assert.AreEqual(iteracaoUsuario.IdFornecedorParticipante, iteracaoUsuarioConsultada.IdFornecedorParticipante);
            Assert.IsFalse(iteracaoUsuarioConsultada.VisualizadoPeloFornecedor);
        }
    }
}
