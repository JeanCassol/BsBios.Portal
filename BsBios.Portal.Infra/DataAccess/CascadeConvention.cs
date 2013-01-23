using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace BsBios.Portal.Infra.DataAccess
{
    /// <summary>
    /// Determina que o nhibernate não vai fazer cascade all por defaul
    /// </summary>
    public class CascadeAll : IHasOneConvention, IHasManyConvention, IReferenceConvention, IHasManyToManyConvention
    {
        #region IHasManyConvention Members

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.None();
        }

        #endregion

        #region IHasManyToManyConvention Members

        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Cascade.None();
        }

        #endregion

        #region IHasOneConvention Members

        public void Apply(IOneToOneInstance instance)
        {
            instance.Cascade.None();
        }

        #endregion

        #region IReferenceConvention Members

        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.None();
        }

        #endregion

        public bool Accept(IOneToOneInstance instance)
        {
            return true;
        }

        public bool Accept(IOneToManyInstance instance)
        {
            return true;
        }

        public bool Accept(IManyToOneInstance instance)
        {
            return true;
        }
    }
}