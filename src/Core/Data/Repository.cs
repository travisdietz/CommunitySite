namespace CommunitySite.Core.Data
{
    public interface Repository
    {
        void Save<T>(T member);
    }
}