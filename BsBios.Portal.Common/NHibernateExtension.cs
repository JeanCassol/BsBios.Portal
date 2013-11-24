using NHibernate.Proxy;

namespace BsBios.Portal.Common
{
    public static class NHibernateExtension
    {
        public static T CastEntity<T>(this T entity)
        {
            var proxy = entity as INHibernateProxy;
            if (proxy != null)
            {
                return (T)proxy.HibernateLazyInitializer.GetImplementation();
            }

            return entity;
        }
    }
}
