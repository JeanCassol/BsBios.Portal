namespace BsBios.Portal.ViewModel
{
    public class PaginacaoVm
    {
        public int Take { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// núumero de registros que devem ser desconsiderados
        /// </summary>
        public int Skip
        {
            get { return (Page - 1) * PageSize; }
        }
    }
}
