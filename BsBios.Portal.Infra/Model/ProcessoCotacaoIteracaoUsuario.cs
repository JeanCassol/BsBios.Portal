using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Model
{
    public class ProcessoCotacaoIteracaoUsuario: IAggregateRoot
    {
        public virtual int IdFornecedorParticipante { get; protected set; }
        public virtual bool VisualizadoPeloFornecedor { get; protected set; }

        protected ProcessoCotacaoIteracaoUsuario(){}
        public ProcessoCotacaoIteracaoUsuario(int idFornecedorParticipante)
        {
            IdFornecedorParticipante = idFornecedorParticipante;
        }
        public virtual void FornecedorVisualizou()
        {
            VisualizadoPeloFornecedor = true;
        }
    }

}
