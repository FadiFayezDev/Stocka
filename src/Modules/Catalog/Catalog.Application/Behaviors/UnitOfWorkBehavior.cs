using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(ICatalogUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // For pure queries, we don't need to save changes.
        // MediatR commands generally don't return responses in our setup unless they are returning IDs.
        // We can check if it's a command by an interface or namespace, but unconditionally saving
        // at the end of the request is fine since EF only commits if there are tracked modifications.

        var response = await next();

        // Note: domain events dispatcher should ideally run BEFORE SaveChangesAsync to avoid anomalies,
        // but EF interceptor or override SaveChangesAsync in DbContext handles that anyway.
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return response;
    }
}
