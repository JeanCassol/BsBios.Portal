﻿using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados
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

        public static void PersistirEntidade(object entidade)
        {
            if (entidade == null)
            {
                return;
            }
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                Session.Save(entidade);
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

        public static void PersistirUsuario(Usuario usuario)
        {
            PersistirEntidade(usuario);
        }

        public static void PersistirFornecedor(Fornecedor fornecedor)
        {
            PersistirEntidade(fornecedor);
        }

        public static void PersistirIva(Iva iva)
        {
            PersistirEntidade(iva);
        }

        private static void PersistirIncoterm(Incoterm incoterm)
        {
            PersistirEntidade(incoterm);
        }

        private static void PersistirCondicaoDePagamento(CondicaoDePagamento condicaoDePagamento)
        {
            PersistirEntidade(condicaoDePagamento);
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
            PersistirEntidade(produto);
        }

        private static void PersistirUnidadeDeMedida(UnidadeDeMedida unidadeMedida)
        {
            PersistirEntidade(unidadeMedida);
        }

        private static void PersistirItinerario(Itinerario itinerario)
        {
            PersistirEntidade(itinerario);
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
                PersistirUnidadeDeMedida(requisicaoDeCompra.UnidadeMedida);
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

                foreach (ProcessoDeCotacaoDeMaterialItem processoDeCotacaoItem in processoDeCotacaoDeMaterial.Itens)
                {
                    PersistirRequisicaoDeCompra(processoDeCotacaoItem.RequisicaoDeCompra);
                }

                if (processoDeCotacaoDeMaterial.Comprador != null)
                {
                    PersistirUsuario(processoDeCotacaoDeMaterial.Comprador);    
                }
                

                foreach (var fornecedorParticipante in processoDeCotacaoDeMaterial.FornecedoresParticipantes)
                {
                    PersistirFornecedor(fornecedorParticipante.Fornecedor);
                    if (fornecedorParticipante.Cotacao != null)
                    {
                        var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao;
                        PersistirCondicaoDePagamento(cotacao.CondicaoDePagamento);
                        PersistirIncoterm(cotacao.Incoterm);
                        var ivas =
                            cotacao.Itens.Where(x => ((CotacaoMaterialItem) x).Iva != null)
                                   .Select(y => ((CotacaoMaterialItem) y).Iva)
                                   .Distinct()
                                   .ToList();
                        foreach (var iva in ivas)
                        {
                            PersistirIva(iva);    
                        }
                        
                    }
                    

                }
                

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


        public static void PersistirProcessosDeCotacaoDeMaterial(IList<ProcessoDeCotacaoDeMaterial> processosDeCotacao)
        {
            try
            {
                Session.BeginTransaction();
                foreach (var processoDeCotacao in processosDeCotacao)
                {
                    PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao);
                }

                Session.Transaction.Commit();
            }
            catch (Exception)
            {
                RollbackSessionTransaction();
                throw;
            }
        }

        public static void PersistirProcessoDeCotacaoDeFrete(ProcessoDeCotacaoDeFrete processo)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }

                if (processo.Comprador != null)
                {
                    PersistirUsuario(processo.Comprador);
                }


                foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
                {
                    PersistirFornecedor(fornecedorParticipante.Fornecedor);
                }
                var unidadesDeMedida = processo.Itens.Select(x => x.UnidadeDeMedida).Distinct();
                foreach (var unidadeDeMedida in unidadesDeMedida)
                {
                    PersistirUnidadeDeMedida(unidadeDeMedida);    
                }
                
                PersistirItinerario(processo.Itinerario);
                var produtos = processo.Itens.Select(x => x.Produto).Distinct();
                foreach (var produto in produtos)
                {
                    PersistirProduto(produto);    
                }
                

                Session.Save(processo);
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

        public static void PersistirQuota(Quota quota)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                PersistirFornecedor(quota.Fornecedor);
                Session.Save(quota);
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

        public static void PersistirAgendamentoDeCarga(AgendamentoDeCarga agendamento)
        {
            try
            {
                bool controlarTransacao = !Session.Transaction.IsActive;
                if (controlarTransacao)
                {
                    Session.BeginTransaction();
                }
                Session.Save(agendamento);
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



