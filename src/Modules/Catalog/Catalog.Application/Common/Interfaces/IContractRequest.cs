namespace Catalog.Application.Common.Interfaces
{
    /// <summary>
    /// Marker implemented by the Application-side MediatR wrapper records that carry a pure
    /// contract record (declared in Catalog.Contracts) through the pipeline.
    /// Pipeline behaviors can use it to run the contract's validators against the carried contract.
    /// </summary>
    public interface IContractRequest
    {
        /// <summary>The pure contract record being dispatched.</summary>
        object Contract { get; }
    }
}
