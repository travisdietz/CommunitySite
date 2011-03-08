using System;
using CommunitySite.Core.Domain;

namespace CommunitySite.Core.Data.NHibernate
{
    public class NHibernateMemberRepository : MemberRepository
    {
        readonly Repository _repository;

        public NHibernateMemberRepository(Repository repository)
        {
            _repository = repository;
        }

        public void Save(Member member)
        {
            _repository.Save(member);
        }
    }
}