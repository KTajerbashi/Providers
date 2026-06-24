using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using UnitOfWork.WebApp.Common;

namespace UnitOfWork.WebApp.Domain;

[Table("Users", Schema = "Security")]
public class UserEntity : AggregateRoot
{
    private UserEntity()
    {

    }

    public UserEntity(
        string firstName,
        string lastName,
        string username,
        string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Password = password;
    }

    public void Update(
        string firstName,
        string lastName,
        string username)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }

    private readonly List<UserRoleEntity> _userRoles = new();
    public virtual IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles;

    public void AddUserRole(UserRoleEntity entity)
    {
        _userRoles.Add(entity);
    }
}

[Table("Roles", Schema = "Security")]
public class RoleEntity : AggregateRoot
{
    public string Title { get; private set; }
    public string Key { get; private set; }
    private RoleEntity() { }

    public RoleEntity(string title, string key)
    {
        Title = title;
        Key = key;
    }

    private readonly List<UserRoleEntity> _userRoles = new();
    public virtual IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles;
}

[Table("UserRoles", Schema = "Security")]
public class UserRoleEntity : Entity
{
    public int RoleId { get; private set; }
    public virtual RoleEntity RoleEntity { get; private set; }

    public int UserId { get; private set; }
    public virtual UserEntity UserEntity { get; private set; }

    private UserRoleEntity() { }

    public UserRoleEntity(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    private readonly List<UserRoleGroupEntity> _userRoleGroups = new();
    public virtual IReadOnlyCollection<UserRoleGroupEntity> UserRoleGroups => _userRoleGroups;
}

[Table("Groups", Schema = "Security")]
public class GroupEntity : AggregateRoot
{
    public string Title { get; private set; }
    public string Key { get; private set; }
    private GroupEntity() { }
    public GroupEntity(string title, string key)
    {
        Title = title;
        Key = key;
    }
    private readonly List<UserRoleGroupEntity> _userRoleGroups = new();
    public virtual IReadOnlyCollection<UserRoleGroupEntity> UserRoleGroups => _userRoleGroups;

    private readonly List<GroupPrivilegeEntity> _groupPrivileges = new();
    public virtual IReadOnlyCollection<GroupPrivilegeEntity> GroupPrivileges => _groupPrivileges;
    public void AddUserRole(UserRoleGroupEntity entity)
    {
        _userRoleGroups.Add(entity);
    }
}

[Table("UserRoleGroups", Schema = "Security")]
public class UserRoleGroupEntity : Entity
{
    public int GroupId { get; private set; }
    public virtual GroupEntity GroupEntity { get; private set; }

    public int UserRoleId { get; private set; }
    public virtual UserRoleEntity UserRoleEntity { get; private set; }

    private UserRoleGroupEntity() { }

    public UserRoleGroupEntity(int userRoleId, int groupId)
    {
        UserRoleId = userRoleId;
        GroupId = groupId;
    }

}

[Table("Privileges", Schema = "Security")]
public class PrivilegeEntity : AggregateRoot
{
    public string Title { get; private set; }
    public string Command { get; private set; }

    private readonly List<GroupPrivilegeEntity> _groupPrivileges = new();
    public virtual IReadOnlyCollection<GroupPrivilegeEntity> GroupPrivileges => _groupPrivileges;
}
[Table("GroupPrivileges", Schema = "Security")]
public class GroupPrivilegeEntity : Entity
{
    public int GroupId { get; private set; }
    public virtual GroupEntity GroupEntity { get; private set; }

    public int PrivilegeId { get; private set; }
    public virtual PrivilegeEntity PrivilegeEntity { get; private set; }
}
