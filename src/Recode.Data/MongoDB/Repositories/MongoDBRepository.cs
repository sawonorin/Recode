using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Recode.Data.EntityContract;
using Recode.Repository.MongoDBRepo;
using Recode.Repository;
using Recode.Data;
using Recode.Data.MongoDB.Configuration;
using Recode.Data.EntityBase;

namespace Recode.Repository.MongoDBRepo
{
    //https://codelp.com/create-a-crud-repository-using-mongodb-and-c/
    public class MongoDBRepository<TEntity, TPrimaryKey> : IMongoDBRepository<TEntity, TPrimaryKey> where TEntity : class, IMongoBaseEntity<TPrimaryKey>
    {
        public readonly IMongoDatabase _database;
        private readonly MongoDBSetting _databaseProvider;
        public MongoDBRepository(IOptions<MongoDBSetting> databaseProvider)
        {
            _databaseProvider = databaseProvider.Value;
            var client = new MongoClient(databaseProvider.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(databaseProvider.Value.DatabaseName);
        }

        public IMongoCollection<TEntity> Collection
        {
            get
            {
                return _database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        public  TEntity Insert(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
             await Collection.InsertOneAsync(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
           Collection.ReplaceOne( x => x.Id.Equals(entity.Id), entity,
           new UpdateOptions
           {
               IsUpsert = true
           });
           return entity;
        }

        public void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public void Delete(TPrimaryKey id)
        {
            Collection.DeleteOne(x => x.Id.Equals(id));
        }

        public  async Task DeleteAsync(TPrimaryKey id)
        {
            await Collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> DeleteUserById(ObjectId id)
        {
            var result = await Collection.DeleteOneAsync(x => x.Id.Equals(id));
            return result.DeletedCount != 0;
        }

        public async Task<long> DeleteAll()
        {
            var filter = new BsonDocument();
            var result = await Collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await Collection.AsQueryable().ToListAsync();
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return  Collection.Find(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }

        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public TEntity Get(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public Task<TEntity> GetAsync(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            return Collection.Find(filter).FirstOrDefaultAsync();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection
            .AsQueryable<TEntity>().SingleOrDefault(predicate.Compile());
                
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(Single(predicate));
        }

        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            return Collection.Find(filter).FirstOrDefaultAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection
           .AsQueryable<TEntity>()
               .FirstOrDefault(predicate.Compile());
        }

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LastOrDefault(predicate);
        }

        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LastOrDefault(predicate));
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public TEntity Load(TPrimaryKey id)
        {
            return Get(id);
        }


        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }
    
    }
}
