using System.ComponentModel.DataAnnotations.Schema;
using UnitOfWork.WebApp.Common;

namespace UnitOfWork.WebApp.Domain;

[Table("Users", Schema = "Security")]
public class UserEntity : AggregateRoot
{
    private UserEntity() { }

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

    public UserRoleEntity AssignRole(RoleEntity role)
    {
        if (_userRoles.Any(x => x.RoleId == role.Id))
            throw new InvalidOperationException("Role already assigned.");

        var userRole = new UserRoleEntity(Id, role);

        _userRoles.Add(userRole);

        return userRole;
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

    public UserRoleEntity(int userId, RoleEntity role)
    {
        UserId = userId;
        RoleEntity = role;
    }

    private readonly List<UserRoleGroupEntity> _userRoleGroups = new();
    public virtual IReadOnlyCollection<UserRoleGroupEntity> UserRoleGroups => _userRoleGroups;

    public UserRoleGroupEntity AssignGroup(GroupEntity group)
    {
        if (_userRoleGroups.Any(x => x.GroupId == group.Id))
            throw new InvalidOperationException("Group already assigned.");

        var userRoleGroup = new UserRoleGroupEntity(Id, group);

        _userRoleGroups.Add(userRoleGroup);

        return userRoleGroup;
    }
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
    public void AddUserRole(int userRoleId)
    {
        if (_userRoleGroups.Any(x => x.UserRoleId == userRoleId))
            return;

        _userRoleGroups.Add(new(userRoleId, Id));
    }

    public void AddUserRole(UserRoleEntity userRole)
    {
        if (_userRoleGroups.Any(x => x.UserRoleId == userRole.Id))
            return;

        _userRoleGroups.Add(new(userRole, Id));
    }
    public void AddPrivilege(PrivilegeEntity privilege)
    {
        if (_groupPrivileges.Any(x => x.PrivilegeId == privilege.Id))
            return;

        _groupPrivileges.Add(new GroupPrivilegeEntity(Id, privilege));
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
    public UserRoleGroupEntity(int userRoleId, GroupEntity group)
    {
        UserRoleId = userRoleId;
        GroupEntity = group;
    }

    public UserRoleGroupEntity(UserRoleEntity userRole, int groupId)
    {
        UserRoleEntity = userRole;
        GroupId = groupId;
    }

}

[Table("Privileges", Schema = "Security")]
public class PrivilegeEntity : AggregateRoot
{
    public string Title { get; private set; }
    public string Command { get; private set; }
    private PrivilegeEntity() { }

    public PrivilegeEntity(string title, string command)
    {
        Title = title;
        Command = command;
    }

    private readonly List<GroupPrivilegeEntity> _groupPrivileges = new();
    public virtual IReadOnlyCollection<GroupPrivilegeEntity> GroupPrivileges => _groupPrivileges;
}
[Table("GroupPrivileges", Schema = "Security")]
public class GroupPrivilegeEntity : Entity
{
    private GroupPrivilegeEntity()
    {

    }
    public GroupPrivilegeEntity(int groupId, int privilegeId)
    {
        GroupId = groupId;
        PrivilegeId = privilegeId;
    }

    public GroupPrivilegeEntity(int groupId, PrivilegeEntity privilege)
    {
        GroupId = groupId;
        PrivilegeEntity = privilege;
    }

    public int GroupId { get; private set; }
    public virtual GroupEntity GroupEntity { get; private set; }

    public int PrivilegeId { get; private set; }
    public virtual PrivilegeEntity PrivilegeEntity { get; private set; }
}
