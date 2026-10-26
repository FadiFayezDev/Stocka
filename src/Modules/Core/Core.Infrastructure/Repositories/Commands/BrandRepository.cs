using BuildingBlocks.Application.Primitives.Identifiers;
using Core.Application.Repositories.Commands;
using Core.Domain.Entities;
using Core.Infrastructure.Contexts;
using Core.Infrastructure.Repositories.Commands.Bases;

namespace Core.Infrastructure.Repositories.Commands
{
    internal class BrandRepository : Repository<Brand, BrandId>, IBrandRepository
    {
        public BrandRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
