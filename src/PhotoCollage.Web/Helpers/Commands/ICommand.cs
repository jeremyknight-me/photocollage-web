using Ardalis.Result;
using MediatR;

namespace PhotoCollage.Web.Helpers.Commands;

internal interface ICommand : IRequest<Result>
{
}

internal interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
