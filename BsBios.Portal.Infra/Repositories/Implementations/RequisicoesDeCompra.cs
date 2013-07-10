using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class RequisicoesDeCompra: CompleteRepositoryNh<RequisicaoDeCompra>, IRequisicoesDeCompra
    {
        public RequisicoesDeCompra(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public RequisicaoDeCompra BuscaPeloId(int id)
        {
            return Query.SingleOrDefault(x => x.Id == id);
        }

        public IRequisicoesDeCompra PertencentesAoGrupoDeCompra(string codigoDoGrupoDeCompras)
        {
            Query = Query.Where(x => x.CodigoGrupoDeCompra == codigoDoGrupoDeCompras);
            return this;
        }

        public IRequisicoesDeCompra SolicitadasApartirDe(DateTime data)
        {
            Query = Query.Where(x => x.DataDeSolicitacao >= data);
            return this;
        }

        public IRequisicoesDeCompra SolicitadasAte(DateTime data)
        {
            Query = Query.Where(x => x.DataDeSolicitacao <= data);
            return this;
        }

        public IRequisicoesDeCompra SemProcessoDeCotacao()
        {
            Query = Query.Where(x => !x.GerouProcessoDeCotacao);
            return this;
        }

        public IList<RequisicaoDeCompra> FiltraPorIds(int[] itensParaAdicionar)
        {
            return Query.Where(x => itensParaAdicionar.Contains(x.Id)).ToList();
        }

        public IRequisicoesDeCompra SomenteAtivas()
        {
            Query = Query.Where(req => req.Status == Enumeradores.StatusRequisicaoCompra.Ativo);
            return this;
        }
    }
}
