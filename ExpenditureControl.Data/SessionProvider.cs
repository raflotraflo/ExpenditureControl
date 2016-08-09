using System;
using System.Web;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cache;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using ExpenditureControl.Data.Domain;

namespace ExpenditureControl.Data
{
    public class SessionProvider
    {
        private readonly string _connectionString;
        private ISessionFactory _sessionFactory;

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
        }

        public SessionProvider()
        {
        }

        public SessionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                        MsSqlConfiguration.MsSql2008
                            .ConnectionString(x => x.FromAppSetting("SqlConnection"))
                            .UseReflectionOptimizer()
                            .AdoNetBatchSize(1000))
                .CurrentSessionContext("thread_static")
                .ExposeConfiguration(
                        x =>
                        {
                            // Increase the timeout for long running queries
                            x.SetProperty("command_timeout", "600");

                            // Allows you to have non-virtual and non-public methods in your entities
                            x.SetProperty("use_proxy_validator", "false");
                        })
                //.Mappings(m => m.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .BuildSessionFactory();
        }
    }
}
