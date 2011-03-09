using System.Linq;
using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Configuration;
using FakeItEasy;
using Machine.Specifications;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace CommunitySite.Integration
{
    public class When_creating_the_community_site_database
    {
        Establish context = () =>
        {
            _configurationService = A.Fake<ConfigurationService>();
            A.CallTo(() => _configurationService.ConnectionString)
                .Returns("data source=.;initial catalog=CommunitySite;user id=cs-user;password=cs-password;");
            _configuration = new AutoMappedConfiguration(_configurationService);
        };

        Because of = () => _sessionFactory = _configuration.CreateSessionFactory();

        It should_create_the_database_schema = () => 
            new SchemaExport(_configuration.DbConfiguration).Create(true, true);

        It should_add_test_members_to_the_database = () =>
            {
                using(var session = _sessionFactory.OpenSession())
                    TestData.Members.ToList().ForEach(session.SaveOrUpdate);
            };
            

        static ConfigurationService _configurationService;
        static AutoMappedConfiguration _configuration;
        static ISessionFactory _sessionFactory;
    }
}