using Microsoft.EntityFrameworkCore;
using Rns.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rns.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private DbContext _context;
        private DbSet<TEntity> _entities;


        public EfRepository(DbContext context)
        {
            this._context = context;
        }

        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();

                return _entities;
            }
        }

        public IQueryable<TEntity> Table => Entities;

        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
            _context.SaveChanges();
        }

        public TEntity GetById(object id)
        {
            return Entities.Find(id);
        }


        public void Insert(TEntity entity)
        {
            entity.CreatedOn = DateTime.Now;

            Entities.Add(entity);
            _context.SaveChanges();
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach(var item in entities)
            {
                item.CreatedOn = DateTime.Now;
            }

            Entities.AddRange(entities);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
            _context.SaveChanges();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
            _context.SaveChanges();
        }

    }
}
