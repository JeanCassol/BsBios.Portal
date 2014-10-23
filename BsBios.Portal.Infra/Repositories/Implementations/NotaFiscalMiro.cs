using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class NotasFiscaisMiro: CompleteRepositoryNh<NotaFiscalMiro>, INotasFiscaisMiro
    {
        public NotasFiscaisMiro(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public INotasFiscaisMiro FiltraPelaChave(string cnpjDoFornecedor, string numero, string serie)
        {
            this.Query = this.Query.Where(nf => nf.CnpjDoFornecedor == cnpjDoFornecedor
                                                && nf.Numero == numero
                                                && nf.Serie == serie);

            return this;
        }
    }
}
