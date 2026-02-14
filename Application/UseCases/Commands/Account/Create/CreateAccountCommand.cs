using Application.Bases;
using Application.Dtos.Accounting;
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

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response<AccountDto>>
    {
        private readonly IAccountCommandRepository _repository;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(IAccountCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<AccountDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Accounting.Account>(request);
            
            var success = await _repository.CreateAsync(entity);
            if (!success)
                return new Response<AccountDto>(false, "Failed to create account");

            var dto = _mapper.Map<AccountDto>(entity);
            return new Response<AccountDto>(dto, "Created Successfully");
        }
    }
}