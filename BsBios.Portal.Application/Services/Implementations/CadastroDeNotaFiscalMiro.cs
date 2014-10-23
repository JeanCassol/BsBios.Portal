using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroDeNotaFiscalMiro : ICadastroDeNotaFiscalMiro
    {
        private readonly IUnitOfWorkNh _unitOfWorkNh;
        private readonly INotasFiscaisMiro _notasFiscaisMiro;
        private readonly IOrdensDeTransporte _ordensDeTransporte;
        private readonly IProcessadorDeNotaFiscalMiro _processadorDeNotaFiscal;

        public CadastroDeNotaFiscalMiro(IUnitOfWorkNh unitOfWorkNh, INotasFiscaisMiro notasFiscaisMiro, 
            IProcessadorDeNotaFiscalMiro processadorDeNotaFiscal, IOrdensDeTransporte ordensDeTransporte)
        {
            _unitOfWorkNh = unitOfWorkNh;
            _notasFiscaisMiro = notasFiscaisMiro;
            _processadorDeNotaFiscal = processadorDeNotaFiscal;
            _ordensDeTransporte = ordensDeTransporte;
        }

        private IEnumerable<NotaFiscalMiro> RealizarCadastro(IEnumerable<NotaFiscalMiroVm> notasFiscais)
        {

            var notasFiscaisGeradas = new List<NotaFiscalMiro>();
            try
            {
                _unitOfWorkNh.BeginTransaction();

                foreach (var notaFiscalVm in notasFiscais)
                {
                    var notaFiscalMiro =
                        new NotaFiscalMiro(notaFiscalVm.CnpjDoFornecedor, notaFiscalVm.Numero, notaFiscalVm.Serie);


                    _notasFiscaisMiro.Save(notaFiscalMiro);

                    notasFiscaisGeradas.Add(notaFiscalMiro);
                }
                _unitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                _unitOfWorkNh.RollBack();
                throw;
            }

            return notasFiscaisGeradas;
            
        }

        private void Processar(IEnumerable<NotaFiscalMiro> notasFiscais)
        {
            foreach (var notaFiscal in notasFiscais)
            {
                try
                {
                    _unitOfWorkNh.BeginTransaction();

                    //como estou chamando o método processar a partir de uma thread, tenho que carregar novamente a entidade
                    NotaFiscalMiro notaFiscalParaProcessamento = _notasFiscaisMiro.FiltraPelaChave(notaFiscal.CnpjDoFornecedor, notaFiscal.Numero, notaFiscal.Serie).Single();

                    OrdemDeTransporte ordemDeTransporte = _processadorDeNotaFiscal.Processar(notaFiscalParaProcessamento);
                    if (ordemDeTransporte != null)
                    {
                        _ordensDeTransporte.Save(ordemDeTransporte);
                    }

                    _notasFiscaisMiro.Save(notaFiscalParaProcessamento);

                    _unitOfWorkNh.Commit();
                }
                catch (Exception)
                {
                    _unitOfWorkNh.RollBack();
                    throw;
                }

            }
            //estou liberando os recursos da unit of work explicitamente porque a requisição http já deve ter concluído e aqui é uma thread
            _unitOfWorkNh.Dispose();
        }

        public void Salvar(ListaDeNotaFiscalMiro notasFiscais)
        {
            IEnumerable<NotaFiscalMiro> conhecimentosDeTransporteGerados = RealizarCadastro(notasFiscais);

            var processar = new Task(() => Processar(conhecimentosDeTransporteGerados));

            processar.Start();

        }
    }

}