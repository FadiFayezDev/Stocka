using Application.Common.Interfaces;
using Application.Dtos.Core;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Repositories.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Entities.Core;
using Application.Common.Exceptions;
using Domain.Entities.Products;
using Domain.Primitives;

namespace Application.UseCases.BranchCases
{
    public class CreateBranchCommand : IRequest<BranchDto>
    {
        public string Name { get; set; } = null!;
    }

    public class CreateBranchCommandHanlder : IRequestHandler<CreateBranchCommand, BranchDto>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IBranchCommandRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<BranchDto> _logger;
        public CreateBranchCommandHanlder(
            ICurrentUserContext currentUserContext,
            IBranchCommandRepository branchRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BranchDto> logger)
        {
            _currentUserContext = currentUserContext;
            _branchRepository = branchRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BranchDto> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUserContext.ActiveBrandId;
            try
            {
                var branch = new Branch(new BrandId(brandId), request.Name);
                await _branchRepository.CreateAsync(branch);

                if (branch == null)
                    throw new BusinessException("brand not set.");

                var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                _logger.Log(LogLevel.Information, "Branch is created");

                return _mapper.Map<BranchDto>(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onboarding failed");
                throw;
            }
        }
    }

}
