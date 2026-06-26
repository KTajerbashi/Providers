using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UnitOfWork.WebApp.Infrastructure;

namespace UnitOfWork.WebApp.Application.Pattern;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellation = default);
    Task CommitTransactionAsync(CancellationToken cancellation = default);
    Task RollbackTransactionAsync(CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}

public abstract class UnitOfWork<TContext> : IUnitOfWork
    where TContext : DataContext
{
    protected readonly TContext _context;
    protected UnitOfWork(TContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellation = default)
    {
        if (_context._transaction != null)
            throw new InvalidOperationException("Transaction already started.");

        _context._transaction = await _context.Database.BeginTransactionAsync(cancellation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        await _context.SaveChangesAsync(cancellation);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellation = default)
    {
        if (_context._transaction == null)
            throw new InvalidOperationException("No active transaction.");

        await _context.SaveChangesAsync(cancellation);
        await _context._transaction.CommitAsync(cancellation);
        await _context._transaction.DisposeAsync();

        _context._transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellation = default)
    {
        if (_context._transaction == null)
            return;

        await _context._transaction.RollbackAsync(cancellation);
        await _context._transaction.DisposeAsync();

        _context._transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_context._transaction != null)
            await _context._transaction.DisposeAsync();

        await _context.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
