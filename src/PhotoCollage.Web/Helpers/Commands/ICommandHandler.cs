using Ardalis.Result;
using MediatR;

namespace PhotoCollage.Web.Helpers.Commands;

internal interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand, IRequest<Result>
{
}

internal interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>, IRequest<Result<TResponse>>
{
}
