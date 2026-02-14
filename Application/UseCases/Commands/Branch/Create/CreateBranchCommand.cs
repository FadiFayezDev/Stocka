using Application.Bases;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Branch.Create
{
    public class CreateBranchCommand : IRequest<Response<BranchDto>>
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, Response<BranchDto>>
    {
        private readonly IBranchCommandRepository _repository;
        private readonly IMapper _mapper;

        public CreateBranchCommandHandler(IBranchCommandRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BranchDto>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = _mapper.Map<Domain.Entities.Core.Branch>(request);
            var success = await _repository.CreateAsync(branch);
            
            if (!success)
                return new Response<BranchDto>(false, "Failed to create branch");

            var dto = _mapper.Map<BranchDto>(branch);
            return new Response<BranchDto>(dto, "Created Successfully");
        }
    }
}
