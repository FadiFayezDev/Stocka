using Domain.Entities.Core;
using Domain.Repositories.Commands;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories.Commands
{
    public class BranchCommandRepository : CommandRepository<Branch>, IBranchCommandRepository
    {
        private readonly AppDbContext _context;
        public BranchCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}