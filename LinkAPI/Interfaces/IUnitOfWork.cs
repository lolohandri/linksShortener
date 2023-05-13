using LinkAPI.Repository;

namespace LinkAPI.Interfaces
{
    public interface IUnitOfWork
    {
        LinkRepository LinkRepository { get; }
        UserRepository UserRepository { get; }
        void Save();
    }
}
