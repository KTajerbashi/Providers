using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.WebApp.Application.Repositories;
using UnitOfWork.WebApp.Application.Services;
using UnitOfWork.WebApp.Domain;

namespace UnitOfWork.WebApp.Controllers;

public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IPrivilegeRepository _privilegeRepository;

    public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IGroupRepository groupRepository, IPrivilegeRepository privilegeRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _groupRepository = groupRepository;
        _privilegeRepository = privilegeRepository;
    }

    [HttpPost("Handler")]
    public async Task<IActionResult> Handler(CancellationToken cancellation)
    {
        await _userRepository.BeginTransactionAsync(cancellation);
        try
        {
            UserEntity userEntity = new("Kamran", "Tajerbashi", "kamran-tajer", "@Tajer123");
            var userEntityId = await _userRepository.AddAsync(userEntity, cancellation);
            await _userRepository.SaveChangesAsync(cancellation);

            RoleEntity roleEntity = new("User", "User");
            var roleEntityId = await _roleRepository.AddAsync(roleEntity, cancellation);
            await _roleRepository.SaveChangesAsync(cancellation);

            var userRoleRecord = userEntity.AssignRole(roleEntity);
            await _userRepository.SaveChangesAsync(cancellation);

            GroupEntity groupEntity = new("AdminGroup", "Admin");
            var groupEntityId = await _groupRepository.AddAsync(groupEntity, cancellation);
            await _groupRepository.SaveChangesAsync(cancellation);

            userRoleRecord.AssignGroup(groupEntity);

            PrivilegeEntity privilegeEntity = new("CreateUser", "Security.User.Create");
            var privilegeEntityId = await _privilegeRepository.AddAsync(privilegeEntity, cancellation);
            await _privilegeRepository.SaveChangesAsync(cancellation);

            groupEntity.AddPrivilege(privilegeEntity);

            var userAggregate = await _userRepository.Aggregates.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.EntityId == userEntityId, cancellation);
            var userRoleEntity = userAggregate.UserRoles.Single(x => x.EntityId == userRoleRecord.EntityId);

            groupEntity.AddUserRole(userRoleEntity.Id);

            await _roleRepository.CommitTransactionAsync(cancellation);
        }
        catch (Exception ex)
        {
            await _groupRepository.RollbackTransactionAsync(cancellation);
            throw ex;
        }

        return Ok(ModelState);
    }

    [HttpPost("Handler-Virtual")]
    public async Task<IActionResult> HandlerVirtual(CancellationToken cancellation)
    {
        await _userRepository.BeginTransactionAsync(cancellation);
        try
        {

            UserEntity userEntity = new("Kamran", "Tajerbashi", "kamran-tajer", "@Tajer123");
            var userEntityId = await _userRepository.AddAsync(userEntity, cancellation);

            RoleEntity roleEntity = new("User", "User");
            var roleEntityId = await _roleRepository.AddAsync(roleEntity, cancellation);

            var userRoleRecord = userEntity.AssignRole(roleEntity);

            GroupEntity groupEntity = new("AdminGroup", "Admin");
            var groupEntityId = await _groupRepository.AddAsync(groupEntity, cancellation);

            userRoleRecord.AssignGroup(groupEntity);

            PrivilegeEntity privilegeEntity = new("CreateUser", "Security.User.Create");
            var privilegeEntityId = await _privilegeRepository.AddAsync(privilegeEntity, cancellation);

            groupEntity.AddPrivilege(privilegeEntity);

            var userAggregate = await _userRepository.Aggregates.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.EntityId == userEntityId, cancellation);

            //groupEntity.AddUserRole(userRoleRecord);

            await _roleRepository.CommitTransactionAsync(cancellation);

        }
        catch (Exception ex)
        {
            await _groupRepository.RollbackTransactionAsync(cancellation);
            throw ex;
        }

        return Ok(ModelState);
    }

}
