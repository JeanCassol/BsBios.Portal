using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacao : CompleteRepositoryNh<ProcessoDeCotacao>, IProcessosDeCotacao
    {
        public ProcessosDeCotacao(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IProcessosDeCotacao BuscaPorId(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }

        public IProcessosDeCotacao FiltraPorFornecedor(string codigoFornecedor)
        {
            Query = Query.Where(x => x.FornecedoresParticipantes.Any(fp => fp.Fornecedor.Codigo == codigoFornecedor));

            return this;
        }

        public IProcessosDeCotacao NomeDoFornecedorContendo(string nomeDoFornecedor)
        {
            Query =
                Query.Where(
                    x =>
                        x.FornecedoresParticipantes.Any(
                            y => y.Fornecedor.Nome.ToLower().Contains(nomeDoFornecedor.ToLower())));

            return this;
        }

        public IProcessosDeCotacao DesconsideraNaoIniciados()
        {
            Query = Query.Where(x => x.Status != Enumeradores.StatusProcessoCotacao.NaoIniciado);
            return this;
        }

        public IProcessosDeCotacao FiltraPorTipo(Enumeradores.TipoDeCotacao tipoDeCotacao)
        {
            if (tipoDeCotacao == Enumeradores.TipoDeCotacao.Frete)
            {
                Query = Query.Where(x => x is ProcessoDeCotacaoDeFrete);
            }
            if (tipoDeCotacao == Enumeradores.TipoDeCotacao.Material)
            {
                Query = Query.Where(x => x is ProcessoDeCotacaoDeMaterial);
            }
            return this;
        }

        public IProcessosDeCotacao DoProduto(string codigoDoProduto)
        {
            Query = Query.Where(x => x.Itens.Any(i => i.Produto.Codigo.ToLower() == codigoDoProduto.ToLower()));
            return this;
        }

        public IProcessosDeCotacao CodigoDoProdutoContendo(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                Query = Query.Where(x => x.Itens.Any(i => i.Produto.Codigo.ToLower().Contains(codigo.ToLower())));
            }

            return this;
        }

        public IProcessosDeCotacao DescricaoDoProdutoContendo(string descricao)
        {
            if (!string.IsNullOrEmpty(descricao))
            {
                Query = Query.Where(x => x.Itens.Any(i => i.Produto.Descricao.ToLower().Contains(descricao.ToLower())));

            }
            return this;
        }

        public IProcessosDeCotacao FiltraPorStatus(Enumeradores.StatusProcessoCotacao status)
        {
            Query = Query.Where(x => x.Status == status);
            return this;
        }

        public IProcessosDeCotacao DesconsideraCancelados()
        {
            Query = Query.Where(x => x.Status != Enumeradores.StatusProcessoCotacao.Cancelado);
            return this;
        }

        public IProcessosDeCotacao SomenteComFornecedoresSelecionados()
        {
            Query = Query.Where(x => x.FornecedoresParticipantes.Any(fp => fp.Cotacao != null  && fp.Cotacao.Selecionada));

            return this;
        }

        public IProcessosDeCotacao SomenteComFornecedoresNaoSelecionados()
        {
            Query = Query.Where(x => x.FornecedoresParticipantes.Any(y => y.Cotacao == null || !y.Cotacao.Selecionada));

            return this;
        }

        public IProcessosDeCotacao Fechado()
        {
            return FiltraPorStatus(Enumeradores.StatusProcessoCotacao.Fechado);
        }

        public IProcessosDeCotacao EfetuadosPeloComprador(string loginComprador)
        {
            Query = Query.Where(x => x.Comprador.Login == loginComprador);
            return this;
        }

        public IProcessosDeCotacao FechadosAPartirDe(DateTime data)
        {
            Query = Query.Where(x => x.DataDeFechamento >= data);
            return this;
        }

        public IProcessosDeCotacao FechadosAte(DateTime data)
        {
            Query = Query.Where(x => x.DataDeFechamento <= data);
            return this;
        }

    }
}