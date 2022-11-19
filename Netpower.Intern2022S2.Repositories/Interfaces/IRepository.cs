using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netpower.Intern2022S2.Repositories.Interfaces
{
    public interface IRepository<T> where T:class
    {
        DbSet<T> DbSet { get; }
        DbContext DbContext { get; set; }
        public Task<IEnumerable<T>> GetAllAsync();
        void Add(T entity);
        void Update(T entity);
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> GetAll();
        public bool Exists(Guid id);

    }
}
