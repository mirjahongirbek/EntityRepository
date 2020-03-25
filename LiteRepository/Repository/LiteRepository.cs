using LiteDB;
using LiteRepository.Context;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace LiteRepository.Repository
{
    public class LiteRepository<T> : IRepositoryCore<T, string>
        where T : class, IEntity<string>
    {
        private ICacheCoreRepository<T> _cache;
        private LiteCollection<T> _lite;

        public LiteRepository(ILiteContext context)
        {
            _lite = context.Database.GetCollection<T>(typeof(T).Name);
        }
        public LiteRepository(ILiteContext context, ICacheCoreRepository<T> cache)
        {
            _cache = cache;
        }

        public void Add(T model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ObjectId.NewObjectId().ToString();
            }
            _cache?.Add(model.Id, model);
            _lite.Insert(model);
        }

        public async Task AddAsync(T model)
        {
            Add(model);
        }

        public void AddRange(List<T> models)
        {
            _cache?.AddRange(models);
            _lite.InsertBulk(models);
        }

        public async Task AddRangeAsync(List<T> models)
        {
            AddRange(models);
        }

        public long Count()
        {
            return _lite.Count();
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            return _lite.LongCount(expression);
        }

        public long Count(string field, string value)
        {
            throw new NotImplementedException();
        }

        public T Delete(string id)
        {
            _cache?.Delete(id);
            var result = Get(id);
            if (result == null)
            {
                return null;
            }
            _lite.Delete(m => m.Id == id);
            return result;
        }

        public T Delete(T model)
        {
            if (model == null) return null;
            Delete(model.Id);
            return model;
        }

        public async Task<T> DeleteAsync(T model)
        {
           return Delete(model);
           
        }

        public bool DeleteMany(Expression<Func<T, bool>> expression)
        {
            _lite.Delete(expression);
            return true;
        }

        public async Task<bool> DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            return DeleteMany(expression);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            
           return Find(selector).Skip(offset).Take(limit);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> keySelector)
        {
           return _lite.Find(keySelector);
        }

        public IEnumerable<T> Find(string field, string value)
        {
            var props = typeof(T).GetProperty(field);
            return _lite.Find(m => props.GetValue(m, null) == value);
         }

        public IEnumerable<T> Find(string field, string value, int offset, int limit)
        {
           return Find(field, value).Skip(offset).Take(limit);
        }

        public IEnumerable<T> FindAll()
        {
           return _lite.FindAll();
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return FindAll();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> keySelector)
        {
           return Find(keySelector);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> selector, int offset, int limit)
        {
           return Find(selector, offset, limit);
        }

        public async Task<IEnumerable<T>> FindAsync(string field, string value)
        {
           return Find(field, value);
        }

        public async Task<IEnumerable<T>> FindAsync(string field, string value, int offset, int limit)
        {
            return Find(field, value, offset, limit);
        }

        public IEnumerable<T> FindReverse(Expression<Func<T, bool>> selector)
        {
         return   Find(selector).Reverse();
        }

        public IEnumerable<T> FindReverse(int offset, int limit)
        {
           return _lite.FindAll().Reverse().Skip(offset).Take(limit);
        }

        public IEnumerable<T> FindReverse(string key, string value, int offset, int limit)
        {
            return Find(key, value).Reverse().Skip(offset).Take(limit);
        }

        public IEnumerable<T> FindReverse(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> FindReverseAsync(int offset, int limit)
        {
            return FindReverse(offset, limit);
        }
        public async Task<IEnumerable<T>> FindReverseAsync(string key, string value, int offset, int limit)
        {
           return FindReverse(key, value, offset, limit);
        }

        public T Get(string id)
        {
           var model= _cache?.Find(id);
            if (model != null) return model;
           return _lite.FindById(id);

        }

        public async Task<T> GetAsync(string id)
        {
           return Get(id);
        }

        public T GetFirst(Expression<Func<T, bool>> expression)
        {
           //var model= _cache?.Find(expression);
           // if (model != null) return model;
           return _lite.FindOne(expression);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
           return GetFirst(expression);
        }

        public Type GetGenericType()
        {
           return typeof(T);
        }

        public T GetLast(Expression<Func<T, bool>> expression)
        {
            //var model = _cache?.Find(expression);
            //if (model != null) return model;
            return _lite.Find(expression).OrderByDescending(m=>m.Id).FirstOrDefault();
        }

        public void Update(T model)
        {
            _cache?.Update(model);
            _lite.Update(model);
        }

        public async Task UpdateAsync(T model)
        {
            Update(model);
        }

        public void UpdateMany(List<T> models)
        {
            _cache?.Update(models);
            _lite.Update(models);

        }

        public async Task UpdateManyAsync(List<T> models)
        {
            UpdateMany(models);
        }
    }
}
