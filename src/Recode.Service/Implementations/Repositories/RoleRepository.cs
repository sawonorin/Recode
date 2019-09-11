using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vigipay.Orbit.Core.Exceptions;
using Vigipay.Orbit.Core.Interfaces.Repositories;
using Vigipay.Orbit.Core.Interfaces.Services;
using Vigipay.Orbit.Core.Models;
using Recode.Data.AppEntity;
using Vigipay.Orbit.Core.Utilities;
using System.Linq;
using Recode.Service.Utilities;

namespace Recode.Service.Implementations.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbContext _dbcontext;
        private readonly ILoggerService _logService;

        public RoleRepository(DbContext dbContext, ILoggerService loggerService)
        {
            _dbcontext = dbContext;
            _logService = loggerService;
        }

        public async Task<bool> AddUserToRole(long roleId, string userId)
        {
            UserRole entity = await _dbcontext.Set<UserRole>()
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.UserId == userId);

            if (entity != null)
            {
                if (!entity.IsActive)
                {
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    int count = await _dbcontext.SaveChangesAsync();
                    return count > 0;
                }
                return true;
            }

            _dbcontext.Set<UserRole>().Add(new UserRole
            {
                RoleId = roleId,
                UserId = userId
            });

            int result = await _dbcontext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<RoleModel> CreateRole(RoleModel model)
        {
            Role entity = await _dbcontext.Set<Role>()
                .FirstOrDefaultAsync(d => d.CorporateId == model.CooperateId && d.RoleName == model.RoleName);
            if (entity != null) throw new AlreadyExistException($"Role {model.RoleName} already exist");

            _dbcontext.Set<Role>().Add(new Role
            {
                Description = model.Description,
                RoleName = model.RoleName,
                RoleType = model.RoleType.ToString(),
                CorporateId = model.CooperateId
            });

            int count = await _dbcontext.SaveChangesAsync();
            return model;
        }

        public async Task<RoleModel> CreateOrGetRole(RoleModel model)
        {
            Role entity = await _dbcontext.Set<Role>()
                .FirstOrDefaultAsync(d => d.CorporateId == model.CooperateId && d.RoleName == model.RoleName);
            if (entity != null) return new RoleModel
            {
                CooperateId = entity.CorporateId,
                Description = entity.Description,
                Id = entity.Id,
                RoleName = entity.RoleName,
                RoleType = entity.RoleType.ToEnum<RoleType>()
            };
            Role role = new Role
            {
                Description = model.Description,
                RoleName = model.RoleName,
                RoleType = model.RoleType.ToString(),
                CorporateId = model.CooperateId
            };
            _dbcontext.Set<Role>().Add(role);
            int count = await _dbcontext.SaveChangesAsync();
            model.Id = role.Id;
            return model;
        }

        public async Task<bool> ActivateRole(long roleId)
        {
            var entity = await _dbcontext.Set<Role>().FirstOrDefaultAsync(x => x.Id == roleId);
            if (entity == null)
            {
                throw new NotFoundException("Role does not exist");
            }

            if (entity.IsActive)
            {
                return true;
            }

            entity.IsActive = true;
            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> DeActivateRole(long roleId)
        {
            var entity = await _dbcontext.Set<Role>().FirstOrDefaultAsync(x => x.Id == roleId);
            if (entity == null)
            {
                throw new NotFoundException("Role does not exist");
            }

            if (!entity.IsActive)
            {
                return true;
            }

            entity.IsActive = false;
            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> UpdateRole(RoleModel model)
        {
            var entity = await _dbcontext.Set<Role>().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                return false;
            }
            entity.RoleName = model.RoleName;
            entity.Description = model.Description;

            int count = await _dbcontext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<Page<RoleModel>> Get(int pageSize, int pageNumber, long corporateId)
        {
            var query = _dbcontext.Set<Role>()
                .Where(e => e.CorporateId == corporateId)
                .Select(x => new RoleModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    RoleName = x.RoleName,
                    RoleType = x.RoleType.ToEnum<RoleType>(),
                    DateCreated = x.DateCreated,
                    IsActive = x.IsActive,
                    CooperateId = x.CorporateId
                })
                .OrderByDescending(x => x.DateCreated);

            return await query.ToPageListAsync(pageNumber, pageSize);
        }

        public async Task<int> AddPermissions(long roleId, long[] permissionIds)
        {
            var rolePerms = await _dbcontext.Set<Permission>()
                .Include(s => s.RolePermissions)
                .Where(x => permissionIds.Any(d => d == x.Id))
                .Select(p => new RolePermission
                {
                    PermissionId = p.Id,
                    RoleId = roleId
                }).ToArrayAsync();
            if (rolePerms.Length <= 0)
            {
                return 0;
            }
            rolePerms = rolePerms.Distinct().ToArray();
            _dbcontext.Set<RolePermission>().AddRange(rolePerms);
            int count = await _dbcontext.SaveChangesAsync();
            return count;
        }

        public async Task<RoleModel> Get(long roleId)
        {
            return await _dbcontext.Set<Role>()
                .Select(x => new RoleModel
                {
                    CooperateId = x.CorporateId,
                    Description = x.Description,
                    Id = x.Id,
                    RoleName = x.RoleName,
                    RoleType = x.RoleType.ToEnum<RoleType>(),
                    DateCreated = x.DateCreated,
                    IsActive = x.IsActive
                })
               .FirstOrDefaultAsync(d => d.Id == roleId);
        }

        public async Task<RoleModel> Get(long roleId, long corporateId)
        {
            return await _dbcontext.Set<Role>()
                .Select(x => new RoleModel
                {
                    CooperateId = x.CorporateId,
                    Description = x.Description,
                    Id = x.Id,
                    RoleName = x.RoleName,
                    RoleType = x.RoleType.ToEnum<RoleType>(),
                    DateCreated = x.DateCreated,
                    IsActive = x.IsActive
                })
               .FirstOrDefaultAsync(d => d.Id == roleId && d.CooperateId == corporateId);
        }

        public async Task<RoleModel> GetByCorporate(long corporateId)
        {
            return await _dbcontext.Set<Role>()
                .Select(x => new RoleModel
                {
                    CooperateId = x.CorporateId,
                    Description = x.Description,
                    Id = x.Id,
                    RoleName = x.RoleName,
                    RoleType = x.RoleType.ToEnum<RoleType>(),
                    DateCreated = x.DateCreated,
                    IsActive = x.IsActive
                })
               .FirstOrDefaultAsync(d => d.CooperateId == corporateId && d.IsActive);
        }

        public async Task<RoleModel[]> GetRoleByUserId(string UserId)
        {
            return await _dbcontext.Set<UserRole>()
                 .Include(d => d.Role)
                 .Where(s => s.UserId == UserId)
                 .Select(x => new RoleModel
                 {
                     CooperateId = x.Role.CorporateId,
                     Description = x.Role.Description,
                     Id = x.Id,
                     RoleName = x.Role.RoleName,
                     RoleType = x.Role.RoleType.ToEnum<RoleType>(),
                     DateCreated = x.DateCreated,
                     IsActive = x.IsActive
                 }).ToArrayAsync();
        }

        public async Task<RoleModel> GetUserRole(string UserId, long roleId)
        {
            return await _dbcontext.Set<UserRole>()
                 .Include(d => d.Role)
                 .Where(s => s.UserId == UserId && s.RoleId == roleId)
                 .Select(x => new RoleModel
                 {
                     CooperateId = x.Role.CorporateId,
                     Description = x.Role.Description,
                     Id = x.Id,
                     RoleName = x.Role.RoleName,
                     RoleType = x.Role.RoleType.ToEnum<RoleType>(),
                     DateCreated = x.DateCreated,
                     IsActive = x.IsActive
                 }).FirstOrDefaultAsync();
        }

        public async Task<int> RemovePermissions(long roleId, long[] permissionIds)
        {
            var rolePerms = await _dbcontext.Set<RolePermission>()
                .Where(x => permissionIds.Any(d => d == x.PermissionId) && x.RoleId == roleId)
                .ToArrayAsync();
            if (rolePerms.Length <= 0)
            {
                return 0;
            }
            _dbcontext.Set<RolePermission>().RemoveRange(rolePerms);
            int count = await _dbcontext.SaveChangesAsync();
            return count;
        }
    }
}
