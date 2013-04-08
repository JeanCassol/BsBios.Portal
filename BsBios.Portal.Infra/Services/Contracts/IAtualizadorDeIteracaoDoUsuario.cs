using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IAtualizadorDeIteracaoDoUsuario
    {
        void Atualizar(int idIteracaoUsuario);
        void Adicionar(IList<FornecedorParticipante> fornecedorParticipantes);
    }
}