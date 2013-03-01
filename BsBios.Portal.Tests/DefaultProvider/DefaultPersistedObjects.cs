using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultPersistedObjects
    {
        private static readonly IUnitOfWorkNh UnitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        private static ISession _session = ObjectFactory.GetInstance<ISession>();
        public static void PersistirUsuario(Usuario usuario)
        {
            //Queries.RemoverUsuariosCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            usuarios.Save(usuario);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirFornecedor(Fornecedor fornecedor)
        {
            //Queries.RemoverFornecedoresCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            fornecedores.Save(fornecedor);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirProduto(Produto produto)
        {
            try
            {
                _session.BeginTransaction();
                _session.Save(produto);
                if (_session.Transaction.IsActive)
                {
                    _session.Transaction.Commit();
                }

            }
            catch (Exception)
            {
                if (_session.Transaction.IsActive)
                {
                    _session.Transaction.Rollback();
                }
                throw;
            }
        }

        public static void PersistirRequisicaoDeCompra(RequisicaoDeCompra requisicaoDeCompra)
        {
            PersistirUsuario(requisicaoDeCompra.Criador);
            PersistirFornecedor(requisicaoDeCompra.FornecedorPretendido);
            PersistirProduto(requisicaoDeCompra.Material);

            UnitOfWorkNh.BeginTransaction();
            var requisicoesDeCompra = ObjectFactory.GetInstance<IRequisicoesDeCompra>();
            requisicoesDeCompra.Save(requisicaoDeCompra);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirProcessoDeCotacaoDeMaterial(ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial)
        {
            PersistirRequisicaoDeCompra(processoDeCotacaoDeMaterial.RequisicaoDeCompra);
            UnitOfWorkNh.BeginTransaction();
            UnitOfWorkNh.Session.Save(processoDeCotacaoDeMaterial);
            UnitOfWorkNh.Commit();
        }
    }
}
