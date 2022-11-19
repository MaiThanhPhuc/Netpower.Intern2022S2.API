using Microsoft.EntityFrameworkCore;
using Netpower.Intern2022S2.Entities.Models;
using Netpower.Intern2022S2.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository()
        {
        }

        public DbSet<T> DbSet
        {
            get
            {
                return DbContext.Set<T>();
            }
        }

        public DbContext DbContext { get; set ; }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            T existing = DbSet.Find(entity);
            if(existing!= null)
            {
                DbSet.Remove(existing);
            }
        }
       
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var data = await DbSet.ToListAsync();
            return data;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var user = await DbSet.FindAsync(id);
            if(user == null)
            {
                return null!;
            }
            return user;
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public bool Exists(Guid id)
        {
            return DbSet.Find(id) != null;
        }

        

        public IQueryable<T> GetAll()
        {
            return DbSet.AsNoTracking();
        }

       
    }
}
