using CommunitySite.Core.Domain;

namespace CommunitySite.Core.Data
{
    public interface MemberRepository
    {
        void Save(Member member);
    }
}