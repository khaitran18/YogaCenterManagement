namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IAccountRepository AccountRepository { get; }
        void Save();
    }
}
