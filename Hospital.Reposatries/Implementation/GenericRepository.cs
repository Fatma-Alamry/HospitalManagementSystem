using Hospital.Reposatries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Reposatries.Implementation
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class

    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> DbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();

        }

        public void Add(T entity)
        {
            DbSet.Add(entity);

        }
        public async Task<T> AddAsync(T entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity) .State == EntityState.Detached)
            {
                DbSet.Attach(entity);   
            }

            DbSet.Remove(entity);

        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
            return entity;
        }
        private bool disposed=false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            foreach(var includeProperty in includeProperties.Split(new char[] {','} ,StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);

            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }

        public T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }


        public void Update(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;    
        }

        public async Task<T> UpdateAsync(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
        
    }
}
