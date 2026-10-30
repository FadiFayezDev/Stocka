using BuildingBlocks.Application.Common.Interfaces;

namespace Identity.Application.Common.Interfaces
{
    /// <summary>
    /// Module-scoped unit-of-work for the Identity module.
    /// Inherits the shared contract so the contract is defined once,
    /// while the distinct type keeps DI registrations isolated per module.
    /// </summary>
    public interface IIdentityUnitOfWork : IUnitOfWork
    {
    }
}
