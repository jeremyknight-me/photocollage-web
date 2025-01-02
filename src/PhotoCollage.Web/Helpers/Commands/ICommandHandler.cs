using Ardalis.Result;

namespace PhotoCollage.Web.Helpers.Commands;

internal interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command);
}

internal interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand
{
    Task<Result<TResponse>> Handle(TCommand command);
}
