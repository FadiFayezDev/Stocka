using Domain.Contracts;
using Domain.Entities.Core;
using Domain.Primitives;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Commands
{
    internal class EmployeeCommandRepository : CommandRepository<Employee, EmployeeId>, IEmployeeCommandRepository
    {
        private readonly AppDbContext _context;
        public EmployeeCommandRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}