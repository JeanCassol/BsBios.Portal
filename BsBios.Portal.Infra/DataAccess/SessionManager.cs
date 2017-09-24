using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using BsBios.Portal.Infra.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;
using NHibernate.SqlCommand;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;
using NHibernate.Validator.Cfg.Loquacious;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.DataAccess
{
    /// <summary>
    /// Classe reponsavel por fazer a configuração do nhibernate
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Unico metodo publico da classe que recebe uma string de conexao para configurar o nhibernate
        /// </summary>
        /// <param name="connString"></param>
        public static void ConfigureDataAccess(string connString)
        {
            //Chama o configurador do nhibernate utilizando o structuremap para fazer a injeção de dependencia
            ObjectFactory.Configure(i => ConfigureDataAccess(i, connString));
            //verifica se tudo foi configurado com sucesso
            ObjectFactory.AssertConfigurationIsValid();
        }

        private static void ConfigureDataAccess(ConfigurationExpression i, string connString)
        {
            ConfigureDataAccess(i, OracleDataClientConfiguration.Oracle10.ConnectionString(c => c.Is(connString)).ShowSql());

        }

        private static void ConfigureDataAccess(ConfigurationExpression i, IPersistenceConfigurer databaseConfigurer)
        {
            //Configura o IoC para session factory do nhibernate ser singleton por toda a aplicação
            i.For<ISessionFactory>()
                .Singleton()
                .Use(ConfigureNHibernate(databaseConfigurer, out var validatorEngine));

            //Configura o IoC para criar uma nova sessão a cada requisição
            i.For<ISession>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use(() =>
                     ObjectFactory.GetInstance<ISessionFactory>
                         ().OpenSession());

            //Configura o validador de entidades do nhibernate
            i.For<ValidatorEngine>()
                .Singleton()
                .Use(validatorEngine);
        }

        /// <summary>
        /// Metodo responsavel por executar o mapeamento das classes com o banco de dados
        /// </summary>
        /// <param name="databaseConfigurer"></param>
        /// <param name="validatorEngine"></param>
        /// <returns></returns>
        private static ISessionFactory ConfigureNHibernate(IPersistenceConfigurer databaseConfigurer,
                                                           out ValidatorEngine validatorEngine)
        {
            ValidatorEngine ve = null;

            ISessionFactory factory = Fluently.Configure()
                .Database(databaseConfigurer)
                .Mappings(m =>
                          m.FluentMappings.AddFromAssemblyOf<FornecedorMap>()
                              .Conventions.Add(typeof (CascadeAll))
                )
                .Mappings(m =>
                    m.HbmMappings.AddFromAssemblyOf<FornecedorMap>())
                .Cache(x =>
                        x.UseQueryCache()
                        .UseSecondLevelCache()
                        .ProviderClass<SysCacheProvider>()
                    )
                .ExposeConfiguration(c =>
                {
                        c.SetInterceptor(new ShowSqlInterceptor());
                                             ve = ConfigureValidator(c);
                                             c.SetProperty("adonet.batch_size", "5");
                                             c.SetProperty("generate_statistics", "false");
                                             //c.SetProperty("cache.use_second_level_cache", "true");
                                         })
                
                                         .BuildConfiguration().BuildSessionFactory();

            validatorEngine = ve;
            return factory;
        }

        /// <summary>
        /// Configura o validados do nhibernate para validar as classes do dominio
        /// </summary>
        /// <param name="nHibernateConfiguration"></param>
        /// <returns></returns>
        private static ValidatorEngine ConfigureValidator(Configuration nHibernateConfiguration)
        {
            var configure = new NHibernate.Validator.Cfg.Loquacious.FluentConfiguration();
            
            configure.Register(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.Namespace != null && t.Namespace.Equals("BsBios.Portal.Domain.Entities"))
                    .ValidationDefinitions())
                .SetDefaultValidatorMode(ValidatorMode.UseAttribute)
                .IntegrateWithNHibernate.ApplyingDDLConstraints().And.RegisteringListeners();

            var ve = new ValidatorEngine();
            ve.Configure(configure);
            nHibernateConfiguration.Initialize(ve);
            return ve;
        }
    }

    internal class ShowSqlInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Trace.WriteLine(sql.ToString());

            return sql;
        }


    }
}