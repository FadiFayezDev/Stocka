using BuildingBlocks.Application.Common.Interfaces;
using BuildingBlocks.Application.Primitives.Identifiers;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Repositories.Commands
{
    public interface IBrandRepository : IRepository<Brand, BrandId>
    {
    }
}
