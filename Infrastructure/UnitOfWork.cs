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
        private IClassRepository _classRepository;
        private IScheduleRepository _scheduleRepository;

        public UnitOfWork(YGCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _mapper);
        public IClassRepository ClassRepository => _classRepository ??= new ClassRepository(_context, _mapper);
        public IScheduleRepository ScheduleRepository => _scheduleRepository ??= new ScheduleRepository(_context, _mapper);
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
