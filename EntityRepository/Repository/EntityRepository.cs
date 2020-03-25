using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityRepository.Context;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Interfaces;

namespace EntityRepository.Repository
{
    public class EntityRepository<T> : IRepositoryCore<T, int>
          where T : class, IEntity<int>
    {
        ICacheCoreRepository<T> _cache;
        protected DbContext _db;
        protected DbSet<T> _dbSet;
        ILoggerCoreRepository<T, int> _log;
        public EntityRepository(IDbContext context)
        {
            _dbSet = context.DataContext.Set<T>();
            _db = context.DataContext;
        }
        public EntityRepository(IDbContext context, ILoggerCoreRepository<T, int> log) : this(context)
        {
            _log = log;
        }
        public EntityRepository(IDbContext context, ICacheCoreRepository<T> cache) : this(context) { _cache = cache; }
        public EntityRepository(IDbContext context, ILoggerCoreRepository<T, int> log, ICacheCoreRepository<T> cache) : this(context)
        {
            _log = log;
            _cache = cache;
        }
        #region Add Methods 

        public virtual void Add(T model)
        {
            try
            {
                _cache?.Add("addMethod: " + model.Id.ToString(), model);
                _dbSet.Add(model);
                _db.SaveChanges();
                _log.AddDocument(model);
            }
            catch(Exception ext)
            {
                _log.CatchError(ext.Message, 0, model, ext,"Add", null);
            }
            
        }

        public virtual async Task AddAsync(T model)
        {
            Add(model);
        }

        public virtual void AddRange(List<T> models)
        {
            try
            {
                _cache?.AddRange(models);
                _dbSet.AddRange(models);
                _db.SaveChanges();
            }catch(Exception ext)
            {
                _log.CatchError(ext.Message, 0, models, ext, "Add Range", null);
                throw;
            }

        }

        public virtual async Task AddRangeAsync(List<T> models)
        {
            AddRange(models);
        }
        #endregion       
        #region Count
        public virtual long Count()
        {
            return _dbSet.Count();
        }

        public virtual long Count(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Count(expression);
        }

        public virtual long Count(string field, string value)
        {
            var props = typeof(T).GetProperty(field);
            var result = _dbSet.Count(m => props.GetValue(m, null) == value);
            return result;
        }
        #endregion
        #region Delete
        public virtual T Delete(T model)
        {
            try
            {
                if (model == null) return null;
                _cache?.Delete(model.Id.ToString());
                _dbSet.Remove(model);
                _db.SaveChanges();
                _log.Delete(model);
                return model;
            }
            catch (Exception ext)
            {
                _log.CatchError(ext.Message, 0, model, ext, "Delete", null);
                throw;
            }

        }

        public virtual async Task<T> DeleteAsync(T model)
        {
            return Delete(model);
        }

        public virtual T Delete(int id)
        {

            return Delete(Get(id));

        }

        public virtual bool DeleteMany(Expression<Func<T, bool>> expression)
        {
            return DeleteMany(Find(expression).ToList());

        }
        public virtual bool DeleteMany(List<T> models)
        {
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                _dbSet.RemoveRange(models);
                _log.DeleteMany(models);
                return true;
            }
            catch(Exception ext)
            {
                _log.CatchError(ext.Message, watch.ElapsedMilliseconds, models,ext, "DeleteMany", "");
                return false;
            }
            
        }
        public virtual async Task<bool> DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            return DeleteMany(expression);
        }
        #endregion
        #region Find
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            var result = _dbSet.Where(selector).OrderByDescending(m => m.Id).Skip(offset).Take(limit).ToList();
            return result;

        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> selector)
        {
            return _dbSet.Where(selector);

        }
        public virtual IEnumerable<T> Find(string field, string value)
        {
            var props = typeof(T).GetProperty(field);
            var result = _dbSet.Where(m => props.GetValue(m, null) == value);
            return result;
        }

        public virtual IEnumerable<T> Find(string field, string value, int offset, int limit)
        {
            var props = typeof(T).GetProperty(field);
            var result = _dbSet.Where(m => props.GetValue(m, null) == value).Skip(offset).Take(limit);
            return result;
        }

        public virtual IEnumerable<T> FindAll()
        {
            return _dbSet;
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync()
        {
            return FindAll();

        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> keySelector)
        {
            return Find(keySelector);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            var result = _dbSet.Where(selector).SkipLast(offset).TakeLast(limit);
            return result;

        }

        public virtual IEnumerable<T> FindReverse(Expression<Func<T, bool>> selector)
        {
            return _dbSet.Where(selector).OrderByDescending(m => m.Id);
        }

        public IEnumerable<T> FindReverse(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            return _dbSet.Where(selector).OrderByDescending(m => m.Id).Skip(offset).Take(limit);
        }
        public virtual IEnumerable<T> FindReverse(int offset, int limit)
        {
            return _dbSet.OrderByDescending(m => m.Id).Skip(offset).Take(limit);
        }

        public virtual IEnumerable<T> FindReverse(string key, string value, int offset, int limit)
        {
            return Find(key, value).OrderByDescending(m => m.Id).Skip(offset).Take(limit);
        }

        public virtual async Task<IEnumerable<T>> FindReverseAsync(int offset, int limit)
        {
            return FindReverse(offset, limit);
        }

        #region
        public virtual async Task<IEnumerable<T>> FindReverseAsync(string key, string value, int offset, int limit)
        {
            return FindReverse(key, value, offset, limit);
        }
        public virtual async Task<IEnumerable<T>> FindAsync(string field, string value)
        {
            return Find(field, value);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(string field, string value, int offset, int limit)
        {
            return Find(field, value, offset, limit);
        }
        #endregion
        #endregion
        #region Get Methods
        public virtual T Get(int id)
        {
            T item = null;
            item = _cache?.Find(id.ToString());
            if (item != null)
            {
                return item;
            }
            item = _dbSet.Find(id);
            return item;
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return Get(id);
        }

        public virtual T GetFirst(Expression<Func<T, bool>> selector)
        {
          
         var   result = _dbSet.FirstOrDefault(selector);
            return result;
        }
        public virtual T GetLast(Expression<Func<T, bool>> selector)
        {
            //var result = _cache?.FindFirst(selector);
            //if (result != null) return result;
           var  result = _dbSet.OrderByDescending(m => m.Id).FirstOrDefault(selector);
            return result;
        }
        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            return GetFirst(expression);
        }
        #endregion
        #region Type
        public Type GetGenericType()
        {
            return typeof(T);
        }
        #endregion
        #region Update
        public virtual void Update(T model)
        {
            _cache?.Update(model.Id.ToString(), model);
            _dbSet.Update(model);
            _db.SaveChanges();

        }

        public virtual async Task UpdateAsync(T model)
        {
            Update(model);
        }

        public virtual void UpdateMany(List<T> models)
        {
            _cache?.Update(models);
            _dbSet.UpdateRange(models);
            _db.SaveChanges();
        }

        public virtual async Task UpdateManyAsync(List<T> models)
        {
            UpdateMany(models);

        }


        #endregion
    }
}
