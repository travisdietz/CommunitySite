using CommunitySite.Core.Domain;

namespace CommunitySite.Core.Data
{
    public interface Repository
    {
        void Save(Member member);
    }
}