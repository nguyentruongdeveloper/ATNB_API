using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class Repository<T> : IRepository<T> where T : class
    {


        public BookManagementDbContext _context;
        public IDbSet<T> dbset;
        public Repository(BookManagementDbContext context)
        {


            this._context = context;
            dbset = context.Set<T>();
        }

        public T GetById(int id)
        {
            return dbset.Find(id);
        }
        public int GetTotalRecord()
        {
            return dbset.Count();
        }


        public IQueryable<T> GetAll()
        {

            return dbset.AsQueryable();

        }

        public void Insert(T entity)
        {
            dbset.Add(entity);
        }


        public void Edit(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }


        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

    }
}
