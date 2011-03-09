using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Authentication;
using CommunitySite.Core.Services.Configuration;
using NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace CommunitySite.Core.Dependencies
{
    public static class Registrar
    {
        public static void RegisterDependencies()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<RepositoryRegistry>();
                    x.AddRegistry<ServiceRegistry>();
                });
        }
    }

    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<ConfigurationService>().Use<WebConfigurationService>();
            For<AuthenticationService>().Use<WebAuthenticationService>();
        }
    }

    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<Repository>().Use<NHibernateRepository>();
            For<MemberRepository>().Use<NHibernateMemberRepository>();

            For<ISessionFactory>().Singleton().TheDefault.Is.ConstructedBy(() => 
                ObjectFactory.GetInstance<AutoMappedConfiguration>().CreateSessionFactory());
        }
    }
}