namespace BsBios.Portal.Infra.BootStrap
{
    public class RegisterModelBinders : IBootstrapTask
    {
        #region IBootstrapperTask Members

        public void Execute()
        {
            //ModelBinders.Binders.Add(typeof (IList<FerramentaVM>), new SalvarFerramentasModelBinder());
        }

        #endregion
    }
}