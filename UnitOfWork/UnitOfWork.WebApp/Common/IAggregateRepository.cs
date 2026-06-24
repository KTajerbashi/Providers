using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using UnitOfWork.WebApp.Application.Pattern;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Common;

public interface IAggregateRepository<TAggregate> : IUnitOfWork
    where TAggregate : AggregateRoot
{
    Task<Guid> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    Task<List<TAggregate>> GetAsync(
        Expression<Func<TAggregate, bool>>? predicate = null, 
        Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>>? include = null, 
        bool asNoTracking = true);
    Task<TAggregate> GetAsync(Guid entityId, CancellationToken cancellationToken = default);
    Task<List<TAggregate>> GetAsync(CancellationToken cancellationToken = default);
    IQueryable<TAggregate> Aggregates { get; }
}

public abstract class AggregateRepository<TAggregate> : UnitOfWork<DataContext>, IAggregateRepository<TAggregate>
    where TAggregate : AggregateRoot
{
    protected DbSet<TAggregate> SetEntity;
    protected AggregateRepository(DataContext context) : base(context)
    {
        SetEntity = context.Set<TAggregate>();
    }

    public IQueryable<TAggregate> Aggregates => SetEntity.AsQueryable();
    public async Task<List<TAggregate>> GetAsync(Expression<Func<TAggregate, bool>>? predicate = null, Func<IQueryable<TAggregate>, IIncludableQueryable<TAggregate, object>>? include = null, bool asNoTracking = true)
    {
        IQueryable<TAggregate> query = context.Set<TAggregate>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include is not null)
            query = include(query);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }
    public async Task<Guid> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
    {
        aggregate.EntityId = Guid.NewGuid();
        aggregate.IsDeleted = false;
        aggregate.IsActive = true;
        var record = await context.AddAsync(aggregate, cancellationToken);
        return record.Entity.EntityId;
    }

    public async Task<TAggregate> GetAsync(Guid entityId, CancellationToken cancellationToken = default)
        => await SetEntity.FirstOrDefaultAsync(x => x.EntityId == entityId, cancellationToken);

    public async Task<List<TAggregate>> GetAsync(CancellationToken cancellationToken = default)
       => await SetEntity.ToListAsync(cancellationToken);
}