using System;
using System.Collections.Generic;
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
        protected DbContext _db;
        protected DbSet<T> _dbSet;
        public EntityRepository(IDbContext context)
        {
            _dbSet = context.DataContext.Set<T>();
        }
        #region Add Methods 

        public virtual void Add(T model)
        {
            _dbSet.FirstOrDefault();
        }

        public virtual Task AddAsync(T module)
        {
            throw new NotImplementedException();
        } 

        public virtual void AddRange(List<T> models)
        {
            throw new NotImplementedException();
        }

        public virtual Task AddRangeAsync(List<T> models)
        {
            throw new NotImplementedException();
        }
        #endregion


        public virtual IEnumerable<T> CallProcedure(string str)
        {
            throw new NotImplementedException();
        }

        public virtual T CalProcedure(string functinname, object[] item)
        {
            throw new NotImplementedException();
        }
        #region Count
        public virtual long Count()
        {
            throw new NotImplementedException();
        }

        public virtual long Count(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual long Count(string field, string value)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Delete
        public virtual T Delete(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> DeleteAsync(T model)
        {
            throw new NotImplementedException();
        }

        public virtual T Delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual bool DeleteMany(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Find
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> keySelector)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Find(string field, string value)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Find(string field, string value, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> keySelector)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> selector, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindAsync(string field, string value)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindAsync(string field, string value, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindReverse(System.Linq.Expressions.Expression<Func<T, bool>> selector)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindReverse(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> FindReverse(string key, string value, int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindReverseAsync(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> FindReverseAsync(string key, string value, int offset, int limit)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Get Methods
        public virtual T Get(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual T GetFirst(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetFirstAsync(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Type
        public Type GetGenericType()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Update
        public virtual void Update(T model)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(T model)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateMany(List<T> models)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateManyAsync(List<T> models)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
