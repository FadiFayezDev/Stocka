using Identity.Application.Repositories.Commands;
using Identity.Domain.Customer;
using Identity.Domain.Parties;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Commands.Bases;

namespace Identity.Infrastructure.Repositories.Commands
{
    internal class CustomerRepository : Repository<Customer, PartyId>, ICustomerRepository
    {
        public CustomerRepository(AppIdentityDbContext context) : base(context)
        {
        }
    }
}
