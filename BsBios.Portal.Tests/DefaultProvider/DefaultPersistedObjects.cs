using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultPersistedObjects
    {
        //private static readonly IUnitOfWorkNh UnitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        private static readonly ISession Session = ObjectFactory.GetInstance<ISession>();
        private static void RollbackSessionTransaction()
        {
            if (Session.Transaction != null && Session.Transaction.IsActive)
            {
                Session.Transaction.Rollback();
            }
        }
        public static void PersistirUsuario(Usuario usuario)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                Session.Save(usuario);
                if (controlarTransacao)
                {
                    Session.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }

        public static void PersistirFornecedor(Fornecedor fornecedor)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                   
                }
                Session.Save(fornecedor);
                if (controlarTransacao)
                {
                    Session.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }

        private static void PersistirFornecedores(IEnumerable<Fornecedor> fornecedores)
        {
            foreach (var fornecedor in fornecedores)
            {
                PersistirFornecedor(fornecedor);
            }
        }

        public static void PersistirProduto(Produto produto)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                Session.Save(produto);
                if (controlarTransacao)
                {
                    Session.Transaction.Commit();
                }

            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }

        public static void PersistirRequisicaoDeCompra(RequisicaoDeCompra requisicaoDeCompra)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                PersistirUsuario(requisicaoDeCompra.Criador);
                PersistirFornecedor(requisicaoDeCompra.FornecedorPretendido);
                PersistirProduto(requisicaoDeCompra.Material);
                Session.Save(requisicaoDeCompra);
                if (controlarTransacao)
                {
                    Session.Transaction.Commit();
                }

            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }

        public static void PersistirProcessoDeCotacaoDeMaterial(ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                PersistirRequisicaoDeCompra(processoDeCotacaoDeMaterial.RequisicaoDeCompra);
                PersistirFornecedores(processoDeCotacaoDeMaterial.FornecedoresParticipantes.Select(x => x.Fornecedor));
                Session.Save(processoDeCotacaoDeMaterial);
                if (controlarTransacao)
                {
                    Session.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }
    }
}



