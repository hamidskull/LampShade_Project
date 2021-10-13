using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class RoleRepository : RepositoryBase<long, Role>, IRoleRepository
    {
        private readonly AccountContext _context;
        public RoleRepository(AccountContext context) : base(context)
        {
            _context = context;
        }

        public EditRole GetDetails(long id)
        {
            var role = _context.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermission(x.Permissions)
            }).AsNoTracking().FirstOrDefault(x => x.Id == id);

            role.Permissions = role.MappedPermissions.Select(x => x.Code).ToList();

            return role;
        }
        private static List<PermissionDTO> MapPermission(List<Permission> permissions)
        {
            return permissions.Select(x => new PermissionDTO(x.Code, x.Name)).ToList();
        }

        public List<RoleViewModel> RoleList()
        {
            return _context.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi()
            }).ToList();
        }
                                                 
        public List<int> GetRolePermissions(long roleId)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == roleId).Permissions.Select(x => x.Code).ToList();
        }
    }
}
