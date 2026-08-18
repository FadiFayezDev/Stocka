using Application.Bases;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Purchasing;
using AutoMapper;
using Domain.Entities.Purchasing;
using Domain.Primitives;
using Domain.Repositories.Commands;
using MediatR;

namespace Application.UseCases.SupplierCases
{
    public class RegisterSupplierCommand : IRequest<Response<SupplierDto>>
    {
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class RegisterSupplierCommandHandler : BaseHandler<ISupplierCommandRepository>, IRequestHandler<RegisterSupplierCommand, Response<SupplierDto>>
    {
        private readonly ICurrentUserContext _currentUser;
        public RegisterSupplierCommandHandler(ISupplierCommandRepository repository, IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
            : base(mapper, repository, unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async Task<Response<SupplierDto>> Handle(RegisterSupplierCommand request, CancellationToken cancellationToken)
        {
            var brandId = _currentUser.ActiveBrandId;

            var entity = new Domain.Entities.Purchasing.Supplier(new BrandId(brandId), request.Name, request.Phone, request.Email, request.Address);
            return await ExecuteCreateAsync<Domain.Entities.Purchasing.Supplier, SupplierDto>(
                entity,
                async (s) => await _repo.CreateAsync(s),
                cancellationToken);
        }
    }
}
