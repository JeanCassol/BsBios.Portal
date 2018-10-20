using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaHistoricoCotacao : IConsultaHistoricoCotacao
    {
        private readonly ICotacaoHistoricoRepository _historicos;

        public ConsultaHistoricoCotacao(ICotacaoHistoricoRepository historicos)
        {
            _historicos = historicos;
        }

        public IList<CotacaoHistoricoListagemVm> ListarPorCotacao(int idFornecedorParticipante)
        {
            return this._historicos
                .DoFornecedorParticipante(idFornecedorParticipante)
                .GetQuery()
                .OrderBy(historico => historico.Id)
                .ToList()
                .Select(historico => new CotacaoHistoricoListagemVm
                {
                    Usuario = historico.Usuario,
                    Data = historico.DataHora.ToString(Constantes.FormatoDeCampoDataHora),
                    Acao = historico.Descricao,
                }).ToList();
        }
    }
}