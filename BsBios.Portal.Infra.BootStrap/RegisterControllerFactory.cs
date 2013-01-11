using System.Web.Mvc;

namespace BsBios.Portal.Infra.BootStrap
{
    public class RegisterControllerFactory : IBootstrapTask
    {
        private readonly IControllerFactory _controllerFactory;

        public RegisterControllerFactory(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        #region IBootstrapperTask Members

        public void Execute()
        {
            ControllerBuilder.Current.SetControllerFactory(_controllerFactory);
        }

        #endregion
    }
}