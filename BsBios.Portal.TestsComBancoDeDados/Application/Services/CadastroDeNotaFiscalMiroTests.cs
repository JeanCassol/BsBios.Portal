using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class CadastroDeNotaFiscalMiroTests
    {
        [TestMethod]
        public void ConsigoCadastrarEConsultarUmaNotaFiscalMiro()
        {
            var notaFiscalMiroVm = new NotaFiscalMiroVm
            {
                CnpjDoFornecedor = "52571057502595",
                Numero = "103535",
                Serie = "1"
            };

            var notasParaCadastrar = new ListaDeNotaFiscalMiro() { notaFiscalMiroVm};

            var cadastroDeNotaFiscalMiro = ObjectFactory.GetInstance<ICadastroDeNotaFiscalMiro>();

            cadastroDeNotaFiscalMiro.Salvar(notasParaCadastrar);

            var unitOfWorkNh = new UnitOfWorkNh(ObjectFactory.GetInstance<ISessionFactory>());

            IQueryable<NotaFiscalMiro> queryable = unitOfWorkNh.Session.Query<NotaFiscalMiro>();

            NotaFiscalMiro notaFiscalMiro = queryable.SingleOrDefault(
                x =>
                    x.CnpjDoFornecedor == notaFiscalMiroVm.CnpjDoFornecedor && x.Numero == notaFiscalMiroVm.Numero &&
                    x.Serie == notaFiscalMiroVm.Serie);


            Assert.IsNotNull(notaFiscalMiro);
        }
    }
}
