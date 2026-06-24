using Microsoft.EntityFrameworkCore;

namespace UnitOfWork.WebApp.Application.Pattern;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellation = default);
    Task CommitTransactionAsync(CancellationToken cancellation = default);
    Task RollbackTransactionAsync(CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}

public abstract class UnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    protected readonly TContext context;
    protected UnitOfWork(TContext context)
    {
        this.context = context;
    }
    public async Task BeginTransactionAsync(CancellationToken cancellation)
        => await context.Database.BeginTransactionAsync(cancellation);

    public async Task CommitTransactionAsync(CancellationToken cancellation)
        => await context.Database.CommitTransactionAsync(cancellation);

    public async Task RollbackTransactionAsync(CancellationToken cancellation)
        => await context.Database.RollbackTransactionAsync(cancellation);

    public async Task SaveChangesAsync(CancellationToken cancellation)
        => await context.SaveChangesAsync(cancellation);
}
