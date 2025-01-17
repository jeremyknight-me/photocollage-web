using Ardalis.Result;
using MediatR;

namespace PhotoCollage.Web.Helpers.Queries;

internal interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result>
    where TQuery : IQuery, IRequest<Result>
{
}

internal interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>, IRequest<Result<TResponse>>
{
}
