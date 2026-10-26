using BuildingBlocks.Application.Common.Interfaces;

namespace Core.Application.Common.Interfaces
{
    /// <summary>
    /// Module-scoped unit-of-work for the Core module.
    /// Inherits the shared contract so the contract is defined once,
    /// while the distinct type keeps DI registrations isolated per module.
    /// </summary>
    public interface ICoreUnitOfWork : IUnitOfWork
    {
    }
}
