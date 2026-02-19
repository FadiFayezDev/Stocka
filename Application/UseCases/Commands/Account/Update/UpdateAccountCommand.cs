using Application.Bases;
using Application.Common.Interfaces;
using Application.Dtos.Accounting;
using Application.QueryRepositories;
using AutoMapper;
using Domain.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Account.Update
{
    public class UpdateAccountCommand : IRequest<Response<AccountDto>>
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class UpdateAccountCommandHandler : BaseHandler<IAccountCommandRepository, IAccountQueryRepository>, IRequestHandler<UpdateAccountCommand, Response<AccountDto>>
    {
        public UpdateAccountCommandHandler(IMapper mapper, IAccountCommandRepository command, IAccountQueryRepository query, IUnitOfWork work)
            : base(mapper, command, query, work)
        {
        }

        public async Task<Response<AccountDto>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _command.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<AccountDto>("Account not found");

            _mapper.Map(request, existing);

            return await ExecuteUpdateAsync<Domain.Entities.Accounting.Account, AccountDto>(
                existing,
                async (acc) => await _command.UpdateAsync(acc),
                cancellationToken);
        }
    }
}