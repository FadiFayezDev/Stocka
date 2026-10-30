using MediatR;

namespace Identity.Application.Common.Interfaces
{
    public interface IBaseCommand { }

    public interface ICommand : IRequest, IBaseCommand { }

    public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand { }

}
