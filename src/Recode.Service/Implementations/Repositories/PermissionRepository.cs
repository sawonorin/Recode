using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vigipay.Orbit.Core.Exceptions;
using Vigipay.Orbit.Core.Interfaces.Repositories;
using Vigipay.Orbit.Core.Models;
using Recode.Data.AppEntity;

namespace Recode.Service.Implementations.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DbContext _dbContext;

        public PermissionRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Claim[]> GetClaims(string UserId)
        {
            return await _dbContext.Set<RolePermission>()
                .Include(s => s.Permission)
                .Include(x => x.Role)
                .ThenInclude(d => d.UserRoles)
                .Where(e => e.Role.UserRoles.Any(x => x.UserId == UserId) && e.Role.IsActive)
                .Select(d => new Claim(ClaimTypes.Role.ToString(), d.Permission.PermissionName)).ToArrayAsync();
        }

        public async Task<PermissionModel[]> GetValidPermissionsByIds(long[] ids)
        {
            var permissions = await _dbContext.Set<Permission>().Where(x => ids.Any(d => d == x.Id) && x.IsActive).ToArrayAsync();
            if (permissions.Length <= 0) return Array.Empty<PermissionModel>();
            return permissions.Select(x => new PermissionModel
            {
                Description = x.Description,
                Id = x.Id,
                PermissionName = x.PermissionName,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            }).ToArray();
        }

        public async Task<PermissionModel[]> GetPermissionsByCorporateId(long corporateId)
        {
            return await _dbContext.Set<RolePermission>()
                .Include(d => d.Role)
                .Include(d => d.Permission)
                .Where(x => x.Role.CorporateId == corporateId).Select(x => new PermissionModel
                {
                    Description = x.Permission.Description,
                    Id = x.Permission.Id,
                    PermissionName = x.Permission.PermissionName,
                    IsActive = x.Permission.IsActive,
                    IsDeleted = x.Permission.IsDeleted
                }).ToArrayAsync();
        }

        public async Task<bool> AddPermission(PermissionModel[] models)
        {
            var existingIds = _dbContext.Set<Permission>()
                .Where(x => models.Any(p => p.PermissionName == x.PermissionName))
                .Select(x => new Permission
                {
                    Description = x.Description,
                    PermissionName = x.PermissionName
                }).Distinct().ToList();

            List<Permission> permissions = models.Select(x => new Permission
            {
                Description = x.Description,
                PermissionName = x.PermissionName
            }).ToList();

            var e = permissions.RemoveAll(x => existingIds.Any(p => p.PermissionName == x.PermissionName));


            if (permissions.Count <= 0 && models.Length > 0)
            {
                throw new AlreadyExistException($"Permission(s) already exist");
            }

            _dbContext.Set<Permission>().AddRange(permissions);
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<PermissionModel[]> GetPermissions()
        {
            return await _dbContext.Set<Permission>()
                .Select(x => new PermissionModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    PermissionName = x.PermissionName,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                }).ToArrayAsync();
        }

        public async Task<bool> Activate(long Id)
        {
            var entity = await _dbContext.Set<Permission>().FirstOrDefaultAsync(x => x.Id == Id);
            if (entity == null)
            {
                throw new NotFoundException("Permission does not exist");
            }

            if (entity.IsActive)
            {
                return true;
            }

            entity.IsActive = true;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> Deactivate(long Id)
        {
            var entity = await _dbContext.Set<Permission>().FirstOrDefaultAsync(x => x.Id == Id);
            if (entity == null)
            {
                throw new NotFoundException("Permission does not exist");
            }

            if (!entity.IsActive)
            {
                return true;
            }

            entity.IsActive = false;
            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<bool> Update(PermissionModel model)
        {
            var entity = await _dbContext.Set<Permission>().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                throw new NotFoundException("Permission does not exist");
            }

            entity.PermissionName = model.PermissionName;
            entity.Description = model.Description;

            int count = await _dbContext.SaveChangesAsync();
            return count > 0;
        }

        public async Task<PermissionModel[]> GetPermissions(long roleId)
        {
            return await _dbContext.Set<RolePermission>()
                .Include(d => d.Permission)
                .Where(x => x.RoleId == roleId)
                  .Select(x => new PermissionModel
                  {
                      Description = x.Permission.Description,
                      Id = x.Permission.Id,
                      PermissionName = x.Permission.PermissionName,
                      IsActive = x.Permission.IsActive,
                      IsDeleted = x.Permission.IsDeleted
                  }).ToArrayAsync();
        }
    }
}
