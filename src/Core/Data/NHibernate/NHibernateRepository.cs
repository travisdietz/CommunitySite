using CommunitySite.Core.Domain;
using NHibernate;

namespace CommunitySite.Core.Data.NHibernate
{
    public class NHibernateRepository : Repository
    {
        readonly ISessionFactory _sessionFactory;


        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        public void Save(Member member)
        {
            using(var session = _sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(member);
            }
        }
    }
}