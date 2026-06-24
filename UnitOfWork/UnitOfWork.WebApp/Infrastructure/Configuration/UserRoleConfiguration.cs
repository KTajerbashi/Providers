using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitOfWork.WebApp.Domain;

namespace UnitOfWork.WebApp.Infrastructure.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.UserEntity)
               .WithMany(x => x.UserRoles)
               .HasForeignKey(x => x.UserId)
               .IsRequired();

        builder.HasOne(x => x.RoleEntity)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();
    }
}


public class UserRoleGroupConfiguration : IEntityTypeConfiguration<UserRoleGroupEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleGroupEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.UserRoleEntity)
               .WithMany(x => x.UserRoleGroups)
               .HasForeignKey(x => x.UserRoleId)
               .IsRequired();

        builder.HasOne(x => x.GroupEntity)
            .WithMany(x => x.UserRoleGroups)
            .HasForeignKey(x => x.GroupId)
            .IsRequired();
    }
}

public class GroupPrivilegeConfiguration : IEntityTypeConfiguration<GroupPrivilegeEntity>
{
    public void Configure(EntityTypeBuilder<GroupPrivilegeEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.GroupEntity)
               .WithMany(x => x.GroupPrivileges)
               .HasForeignKey(x => x.GroupId)
               .IsRequired();

        builder.HasOne(x => x.PrivilegeEntity)
            .WithMany(x => x.GroupPrivileges)
            .HasForeignKey(x => x.PrivilegeId)
            .IsRequired();
    }
}

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
    }
}
