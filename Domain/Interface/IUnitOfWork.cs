using Domain.Interface;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IClassRepository ClassRepository { get; }
        void Save();
    }
}
