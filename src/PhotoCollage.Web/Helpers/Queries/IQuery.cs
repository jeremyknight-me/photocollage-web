using Ardalis.Result;
using MediatR;

namespace PhotoCollage.Web.Helpers.Queries;

internal interface IQuery : IRequest<Result>
{
}

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
