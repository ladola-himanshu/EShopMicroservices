using MediatR;

namespace BuilingBlocks.CQRS
{
    
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse>
        where TQuery : notnull, IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
