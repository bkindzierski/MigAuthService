using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Dialect;
using NHibernate.Linq;
using System.Reflection;

using FluentNHibernate.Cfg;


namespace RagAppGuide.DAL
{
    public class NHibernateSession
    {
        public ISessionFactory session {  get;  set; }
        
        public NHibernateSession() {
                           
                if (session == null) {
                    ISessionFactory _session = CreateSessionFactory();
                    session = _session;
                }            
        }

        //DVDTA CONNECT
        private static ISessionFactory CreateSessionFactory()
        {
            ISessionFactory factory = null;

            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                //x.ConnectionString = "DataSource=S10B4642;Database=S10B4642;userID=SVCDNETDV;Password=SVCDNETDV;Pooling=false;Naming=System;LibraryList=DVDTA;";
                x.ConnectionString = "DataSource=S10B4642;Database=S10B4642;userID=SVCDNETUA;Password=SVCDNETUA;Pooling=false;Naming=System;LibraryList=MIGPUADTAM;";
                x.Dialect<DB2400Dialect>();
                x.Driver<DB2400Driver>();
            });

            factory = Fluently.Configure(cfg)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))                               
                .ExposeConfiguration(x => x.SetProperty("connection.release_mode", "on_close"))
                .BuildSessionFactory();

            return factory;
        }
        
        //SQL SERVER DEV-SQL-SND\KINDZIERSKI CONNECT
        //private static ISessionFactory CreateSessionFactory()
        //{
        //    return Fluently.Configure()

        //        .Database(MsSqlConfiguration.MsSql2012
        //                   .ConnectionString(c => c
        //                   .Server("DEV-SQL-SND\\KINDZIERSKI")
        //                   .Database("RiskAppGuide")
        //                   .Username("RiskAppGuide_User")
        //                   .Password("CnirhM^#JZM?PG@y>0"))
        //       )
        //       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
        //       .BuildSessionFactory();
        //}

    }
}
