using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Core;
using AutoMapper;
using Domain.Entities.Core;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.Features.Commands.Branch.Create
{
    public class CreateBranchCommand : IRequest<Response<BranchDto>>
    {
        public string Name { get; set; } = null!;
    }

    public class CreateBranchCommandHandler : BaseHandler<IBranchCommandRepository>, IRequestHandler<CreateBranchCommand, Response<BranchDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public CreateBranchCommandHandler(IBranchCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<BranchDto>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var branch = new Domain.Entities.Core.Branch(new BrandId(brandId), request.Name);

            return await ExecuteCreateAsync<Domain.Entities.Core.Branch, BranchDto>(
                branch,
                async (b) => await _repo.CreateAsync(b),
                cancellationToken);
        }
    }
}
