using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.UseCases.Commands.Account.Create
{
    public class CreateAccountCommand : IRequest<Response<AccountDto>>
    {
        public string Name { get; set; } = null!;
        public int Type { get; set; }
    }

    public class CreateAccountCommandHandler : BaseHandler<IAccountCommandRepository>, IRequestHandler<CreateAccountCommand, Response<AccountDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public CreateAccountCommandHandler(IMapper mapper, IAccountCommandRepository command, IUnitOfWork work, ICurrentUserContext currentUser) 
            : base(mapper, command, work)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<AccountDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var entity = new Domain.Entities.Accounting.Account(
                new BrandId(brandId), 
                request.Name, 
                (AccountType)request.Type);

            return await ExecuteCreateAsync<Domain.Entities.Accounting.Account, AccountDto>(
                entity,
                async (acc) => await _repo.CreateAsync(acc),
                cancellationToken);
        }
    }
}