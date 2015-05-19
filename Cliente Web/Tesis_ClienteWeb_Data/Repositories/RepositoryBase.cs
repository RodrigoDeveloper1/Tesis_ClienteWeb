using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Tesis_ClienteWeb_Data.Repositories
{
    public class RepositoryBase<T> where T : class
    {
        internal Context _dataContext;
        internal DbSet<T> _dbset;

        public RepositoryBase(Context context)
        {
            this._dataContext = context;
            this._dbset = context.Set<T>();

        }

        public virtual List<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }
        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> resultado = _dbset.AsQueryable();

            return resultado;
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.AsQueryable().Where(where);
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault<T>();
        }
        public virtual void Modify(T entity)
        {
            //Solo para debugging
            //Rodrigo Uzcátegui 20-12-14
            var entry = _dataContext.Entry(entity); 
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Update(T entity, int id)
        {
            if (entity == null)
                throw new ArgumentException("Cannot update a null entity.");

            var entry = _dataContext.Entry(entity);

            /* 
             * Referencia: http://msdn.microsoft.com/en-us/library/system.data.entitystate(v=vs.110).aspx
             * The object exists but is not being tracked. An entity is in this state immediately after it has 
             * been created and before it is added to the object context. An entity is also in this state after 
             * it has been removed from the context by calling the Detach method or if it is loaded by using a 
             * NoTracking MergeOption. There is no ObjectStateEntry instance associated with objects in the 
             * Detached state.
             */
            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = _dbset.Find(id); // Need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = _dataContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
        }
        public virtual void Update(T entity, string id)
        {
            if (entity == null)
                throw new ArgumentException("Cannot update a null entity.");

            var entry = _dataContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = _dbset.Find(id); // Need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = _dataContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
        }
        public virtual void Attach(T entity)
        {
            _dbset.Attach(entity);
        }
    }
}