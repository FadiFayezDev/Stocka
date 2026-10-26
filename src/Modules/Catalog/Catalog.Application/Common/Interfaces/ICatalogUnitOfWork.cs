using BuildingBlocks.Application.Common.Interfaces;

namespace Catalog.Application.Common.Interfaces
{
    /// <summary>
    /// Module-scoped unit-of-work for the Catalog module.
    /// Inherits the shared contract so the contract is defined once,
    /// while the distinct type keeps DI registrations isolated per module.
    /// </summary>
    public interface ICatalogUnitOfWork : IUnitOfWork
    {
    }
}
