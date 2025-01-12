using Ardalis.Result;

namespace PhotoCollage.Web.Helpers.Queries;

internal interface IQueryHandler<TQuery>
    where TQuery : IQuery
{
    Task<Result> Handle(TQuery query);
}

internal interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery
{
    Task<Result<TResponse>> Handle(TQuery query);
}
