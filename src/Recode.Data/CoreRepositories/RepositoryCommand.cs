﻿
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recode.Data.EntityContract;
using Recode.Data;

namespace Recode.Repository.CoreRepositories
{
    ///---------------------------------------------------------------------------------------------
    /// <summary>
    /// Implement CQRS(Command Query Repository Patterns)
    /// </summary>
    /// <copyright>
    /// *****************************************************************************
    ///     ----- Fadipe Wasiu Ayobami . All Rights Reserved. Copyright (c) 2017
    /// *****************************************************************************
    /// </copyright>
    /// <remarks>
    /// *****************************************************************************
    ///     ---- Created For: Public Use (All Products)
    ///     ---- Created By: Fadipe Wasiu Ayobami
    ///     ---- Original Language: C#
    ///     ---- Current Version: v1.0.0.0.1
    ///     ---- Current Language: C#
    /// *****************************************************************************
    /// </remarks>
    /// <history>
    /// *****************************************************************************
    ///     --- Date First Created : 08 - 11 - 2017
    ///     --- Author: Fadipe Wasiu Ayobami
    ///     --- Date First Reviewed: 
    ///     --- Date Last Reviewed:
    /// *****************************************************************************
    /// </history>
    /// <usage>
    /// 
    /// -- Fadipe Wasiu Ayobami
    /// </usage>
    /// ----------------------------------------------------------------------------------------------
    ///
    ///
    public class RepositoryCommand<TEntity, TPrimaryKey> : IRepositoryCommand<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly APPContext _context;
        public RepositoryCommand(APPContext context)
        {
            this._context = context;
        }



        /// <summary>
        /// get the current Entity
        /// <returns><see cref="DbSet{TEntity}"/></returns>
        /// </summary>

        public virtual DbSet<TEntity> Table => _context.Set<TEntity>();

        public void Save()
        {
            _context.SaveChanges();
        }

        //public TPrimaryKey SaveChanges()
        //{
        //    return _context.SaveChanges()(TPrimaryKey);
        //}

        //public Task<TPrimaryKey> SaveChangesAsync()
        //{
        //    return _context.SaveChangesAsync();
        //}

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        public EntityEntry<TEntity> Insert(TEntity entity)
        {
            return Table.Add(entity);
        }

        public Task<EntityEntry<TEntity>> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Table.Add(entity));
        }



        public TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity).Entity;
            if (entity.IsTransient())
            {
                _context.SaveChanges();
            }

            return entity.Id;
        }

        public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var ent = await InsertAsync(entity);
            entity = ent.Entity;
            if (entity.IsTransient())
            {
                await _context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity).Entity;
            if (entity.IsTransient())
            {
                _context.SaveChanges();
            }

            return entity.Id;
        }

        public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            var ent = await InsertOrUpdateAsync(entity);
            entity = ent.Entity;
            if (entity.IsTransient())
            {
                await _context.SaveChangesAsync();
            }

            return entity.Id;
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            AttachIfNot(entity);
            var ent = _context.Entry(entity);
            ent.State = EntityState.Modified;
            return ent;
        }

        public Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            var ent = _context.Entry(entity);
            ent.State = EntityState.Modified;
            return Task.FromResult(ent);
        }

        public void Delete(TEntity entity)
        {
            Update(entity.Id, x => x.IsDeleted = true);
        }


        public TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Table.Find(id);
            updateAction(entity);
            return entity;
        }

        public void Delete(TPrimaryKey id)
        {
            Update(id, x => x.IsDeleted = true);
        }

        public Task DeleteAsync(TPrimaryKey id)
        {
            return Task.Run(() => Delete(id));

        }


        public async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await Table.FindAsync(id);
            await updateAction(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Func<TEntity, Task> updateAction)
        {
            var entity = await Table.FirstOrDefaultAsync(predicate);
            await updateAction(entity);
            return entity;
        }


        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }


        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        #region MyCommandRegion
        public async Task<EntityEntry<TEntity>> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            return !Table.Any(predicate) ? await InsertAsync(entity) : await UpdateAsync(entity);
        }


        public virtual EntityEntry<TEntity> InsertOrUpdate(TEntity entity)
        {
            return entity.IsTransient()
                ? Insert(entity)
                : Update(entity);
        }

        public virtual async Task<EntityEntry<TEntity>> InsertOrUpdateAsync(TEntity entity)
        {
            return entity.IsTransient()
                ? await InsertAsync(entity)
                : await UpdateAsync(entity);
        }



        #endregion
    }
}
