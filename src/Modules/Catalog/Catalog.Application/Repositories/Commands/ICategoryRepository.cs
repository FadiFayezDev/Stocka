using BuildingBlocks.Application.Primitives.Identifiers;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Repositories.Commands.Bases;
using Catalog.Domain.Entities;
using Catalog.Domain.Primitives;

namespace Catalog.Application.Repositories.Commands
{
    public interface ICategoryRepository : IRepository<Category, CategoryId>
    {

    }
}