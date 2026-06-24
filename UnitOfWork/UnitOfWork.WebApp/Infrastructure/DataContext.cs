using Microsoft.EntityFrameworkCore;
using UnitOfWork.WebApp.Domain;

namespace UnitOfWork.WebApp.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
       : base(options)
    {
    }
    public DbSet<UserEntity> UserEntities => Set<UserEntity>();
    public DbSet<RoleEntity> RoleEntities => Set<RoleEntity>();
    public DbSet<UserRoleEntity> UserRoleEntities => Set<UserRoleEntity>();
    public DbSet<GroupEntity> GroupEntities => Set<GroupEntity>();
    public DbSet<UserRoleGroupEntity> UserRoleGroupEntities => Set<UserRoleGroupEntity>();
    public DbSet<PrivilegeEntity> PrivilegeEntities => Set<PrivilegeEntity>();
    public DbSet<GroupPrivilegeEntity> GroupPrivilegeEntities => Set<GroupPrivilegeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
