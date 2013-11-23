using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class QuantidadeLiberadaSuperouQuantidadeAdquiridaException: Exception
    {
        private readonly decimal _quantidadeLiberadaParaOrdemDeTransporte;
        private readonly decimal _quantidadeLiberadaTotal;
        private readonly decimal _quantidadeAdquirida;
        public QuantidadeLiberadaSuperouQuantidadeAdquiridaException( decimal quantidadeLiberadaParaOrdemDeTransporte, decimal quantidadeLiberadaTotal, decimal quantidadeAdquirida)
        {
            _quantidadeLiberadaParaOrdemDeTransporte = quantidadeLiberadaParaOrdemDeTransporte;
            _quantidadeLiberadaTotal = quantidadeLiberadaTotal;
            _quantidadeAdquirida = quantidadeAdquirida;
        }

        public override string Message
        {

            get
            {
                return "Não é permitido alterar a quantidade liberada da Ordem de Transporte para " + _quantidadeLiberadaParaOrdemDeTransporte.ToString(Constantes.FormatatoDeCampoDeQuantidade) +
                         ", pois a quantidade liberada total (" + _quantidadeLiberadaTotal.ToString(Constantes.FormatatoDeCampoDeQuantidade) + ")  será superior à quantidade total adquirida no processo de cotação (" +
                         _quantidadeAdquirida.ToString(Constantes.FormatatoDeCampoDeQuantidade) + ").";
            }
        }
    }
}