using CommunitySite.Core.Data;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Authentication;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace CommunitySite.Core.Dependencies
{
    public static class Registrar
    {
        public static void RegisterDependencies()
        {
            ObjectFactory.Initialize(x=>
                {
                    x.UseDefaultStructureMapConfigFile = false;
                    x.AddRegistry<RepositoryRegistry>();
                    x.AddRegistry<ServiceRegistry>();
                });
        }
    }

    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            For<AuthenticationService>().Use<WebAuthenticationService>();
        }
    }

    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<MemberRepository>().Use<NHibernateMemberRepository>();
            For<Repository>().Use<NHibernateRepository>();
        }
    }
}