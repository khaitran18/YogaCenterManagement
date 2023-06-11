using AutoMapper;
using Infrastructure.Data;
using Application.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YGCContext _context;
        private readonly IMapper _mapper;
        //private IAccountRepository _accountRepository;

        public UnitOfWork(YGCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_context, _mapper);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
