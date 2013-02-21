using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Tests.Domain.Model
{
    public class PedidoDeCotacao : IAggregateRoot
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }
        public virtual Enumeradores.StatusPedidoCotacao Status { get; protected set; }
        public virtual DateTime DataLimiteDeRetorno { get; protected set; }

        protected PedidoDeCotacao(){}

        public PedidoDeCotacao(RequisicaoDeCompra requisicaoDeCompra, DateTime dataLimiteDeRetorno)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            Status = Enumeradores.StatusPedidoCotacao.NaoIniciado;
        }
    }
}
