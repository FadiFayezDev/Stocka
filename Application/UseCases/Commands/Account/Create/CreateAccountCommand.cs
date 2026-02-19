using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Account.Create
{
    public class CreateAccountCommand : IRequest<Response<AccountDto>>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class CreateAccountCommandHandler : BaseHandler<IAccountCommandRepository, IAccountQueryRepository>, IRequestHandler<CreateAccountCommand, Response<AccountDto>>
    {
        public CreateAccountCommandHandler(IMapper mapper, IAccountCommandRepository command, IAccountQueryRepository query, IUnitOfWork work) 
            : base(mapper, command, query, work)
        {
        }

        public async Task<Response<AccountDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.Account>(request);
            
            return await ExecuteCreateAsync<Domain.Entities.Accounting.Account, AccountDto>(
                entity,
                async (acc) => await _command.CreateAsync(acc),
                cancellationToken);
        }
    }
}