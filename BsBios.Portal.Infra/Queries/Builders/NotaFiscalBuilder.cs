using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class NotaFiscalBuilder: Builder<NotaFiscal,NotaFiscalVm>
    {
        public override NotaFiscalVm BuildSingle(NotaFiscal model)
        {
            return new NotaFiscalVm
                {
                    Numero = model.Numero ,
                    Serie = model.Serie ,
                    DataDeEmissao = model.DataDeEmissao.ToShortDateString() ,
                    CnpjDoEmitente = model.CnpjDoEmitente ,
                    NomeDoEmitente = model.NomeDoEmitente ,
                    CnpjDoContratante = model.CnpjDoContratante ,
                    NomeDoContratante = model.NomeDoContratante,
                    NumeroDoContrato = model.NumeroDoContrato ,
                    Peso =  model.Peso  ,
                    Valor = model.Valor
                };
        }
    }
}
