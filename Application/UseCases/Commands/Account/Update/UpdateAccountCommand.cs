using Application.Bases;
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

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Response<AccountDto>>
    {
        private readonly IAccountCommandRepository _repository;
        private readonly IAccountQueryRepository _queries;
        private readonly IMapper _mapper;

        public UpdateAccountCommandHandler(IAccountCommandRepository repository, IAccountQueryRepository queries, IMapper mapper)
        {
            _repository = repository;
            _queries = queries;
            _mapper = mapper;
        }

        public async Task<Response<AccountDto>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var existing = await _queries.GetByIdAsync(request.Id);
            if (existing == null)
                return new Response<AccountDto>("Not found");

            _mapper.Map(request, existing);
            //var success = await _repository.UpdateAsync(existing);

            //if (!success)
            //    return new Response<AccountDto>(false, "Failed to update account");

            var dto = _mapper.Map<AccountDto>(existing);
            return new Response<AccountDto>(dto, "Updated Successfully");
        }
    }
}