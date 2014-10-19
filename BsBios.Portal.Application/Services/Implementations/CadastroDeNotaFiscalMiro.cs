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
                    OrdemDeTransporte ordemDeTransporte = _processadorDeNotaFiscal.Processar(notaFiscal);
                    if (ordemDeTransporte != null)
                    {
                        _ordensDeTransporte.Save(ordemDeTransporte);
                    }

                    _notasFiscaisMiro.Save(notaFiscal);

                    _unitOfWorkNh.Commit();
                }
                catch (Exception)
                {
                    _unitOfWorkNh.RollBack();
                    throw;
                }

            }
        }

        public void Salvar(ListaDeNotaFiscalMiro notasFiscais)
        {
            IEnumerable<NotaFiscalMiro> conhecimentosDeTransporteGerados = RealizarCadastro(notasFiscais);

            var processar = new Task(() => Processar(conhecimentosDeTransporteGerados));

            processar.Start();

        }
    }

}