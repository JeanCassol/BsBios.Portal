using System;
using BsBios.Portal.Infra.Services.Contracts;
using log4net;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using StructureMap;

namespace BsBios.Portal.Infra.DataAccess
{
    internal class QuotaInterceptor : EmptyInterceptor
    {

        private static readonly ILog Log = LogManager.GetLogger("Quota");


        private bool DeveLogar(object entity)
        {
            return entity.GetType().Name.Equals("Quota");
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (DeveLogar(entity))
            {
                Log.Info($"{ObjectFactory.GetInstance<IFormatadorDeLog>().FormatarUsuario()} - Salvando registro: {entity}");
            }
            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            if (DeveLogar(entity))
            {
                Log.Info($"{ObjectFactory.GetInstance<IFormatadorDeLog>().FormatarUsuario()} - Atualizando registro: {entity}");
            }
            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override SqlString OnPrepareStatement(SqlString sql)
        {
            if (sql.IndexOf("Quota", 0, sql.Length, StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                Log.Info($"{ObjectFactory.GetInstance<IFormatadorDeLog>().FormatarUsuario()} - {sql}" );
            }
            return base.OnPrepareStatement(sql);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (DeveLogar(entity))
            {
                Log.Info($"{ObjectFactory.GetInstance<IFormatadorDeLog>().FormatarUsuario()} - Excluindo registro {entity}");
            }
            base.OnDelete(entity, id, state, propertyNames, types);
        }


    }

}