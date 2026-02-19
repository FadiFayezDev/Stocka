using Application.Bases;
using Application.Common.Interfaces;
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

    public class CreateBranchCommandHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<CreateBranchCommand, Response<BranchDto>>
    {
        public CreateBranchCommandHandler(IBranchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {
        }

        public async Task<Response<BranchDto>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = _mapper.Map<Domain.Entities.Core.Branch>(request);
            
            return await ExecuteCreateAsync<Domain.Entities.Core.Branch, BranchDto>(
                branch,
                async (b) => await _repo.CreateAsync(b),
                cancellationToken);
        }
    }
}
