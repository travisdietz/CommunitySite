using CommunitySite.Core.Data.NHibernate;
using CommunitySite.Core.Services.Configuration;
using FakeItEasy;
using Machine.Specifications;
using NHibernate;

namespace CommunitySite.Integration
{
    public class When_connecting_to_application_storage
    {
        Establish context = () =>
            {
                _configurationService = A.Fake<ConfigurationService>();
                A.CallTo(() => _configurationService.ConnectionString)
                    .Returns("data source=.;initial catalog=CommunitySite;user id=cs-user;password=cs-password;");
                _configuration = new AutoMappedConfiguration(_configurationService);
            };

        Because of = () => _sessionFactory = _configuration.CreateSessionFactory();

        It should_get_the_location_for_storage_from_configuration = () =>
            A.CallTo(() => _configurationService.ConnectionString).MustHaveHappened();

        It should_successfully_connect_to_storage = () =>
            _sessionFactory.OpenSession().ShouldNotBeNull();

        static ConfigurationService _configurationService;
        static AutoMappedConfiguration _configuration;
        static ISessionFactory _sessionFactory;
    }
}