using AutoMapper;
using Infrastructure.Data;
using Application.Interfaces;
using Infrastructure.Repositories;
using Domain.Interface;
using Infrastructure.Repository;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YGCContext _context;
        private readonly IMapper _mapper;
        private IUserRepository _userRepository;

        public UnitOfWork(YGCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _mapper);

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
