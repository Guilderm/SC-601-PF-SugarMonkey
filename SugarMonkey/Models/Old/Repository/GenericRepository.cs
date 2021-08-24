using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SugarMonkey.DAL;

namespace SugarMonkey.Models.Old.Repository
{
    // This is used to Isolate the EntityFramework based Data Access Layer from the MVC Controller class

    public class GenericRepository<TBlEntity> : IRepository<TBlEntity> where TBlEntity : class
    {
        private readonly Online_ShoppingEntities _dbEntity;
        private readonly DbSet<TBlEntity> _dbSet;

        public GenericRepository(Online_ShoppingEntities dbEntity)
        {
            _dbEntity = dbEntity;
            _dbSet = _dbEntity.Set<TBlEntity>();
        }

        public IEnumerable<TBlEntity> GetAllRecords()
        {
            return _dbSet.ToList();
        }

        public IQueryable<TBlEntity> GetAllRecordsIQueryable()
        {
            return _dbSet;
        }

        public IEnumerable<TBlEntity> GetRecordsToShow(int pageNo, int pageSize, int currentPageNo,
            Expression<Func<TBlEntity, bool>> wherePredict, Expression<Func<TBlEntity, int>> orderByPredict)
        {
            if (wherePredict != null)
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();
            return _dbSet.OrderBy(orderByPredict).ToList();
        }

        public int GetAllRecordsCount()
        {
            return _dbSet.Count();
        }

        public void Add(TBlEntity entity)
        {
            _dbSet.Add(entity);
            _dbEntity.SaveChanges();
        }

        /// <summary>
        ///     Updates table entity passed to it
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TBlEntity entity)
        {
            _dbSet.Attach(entity);
            _dbEntity.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict,
            Action<TBlEntity> forEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(forEachPredict);
        }

        public TBlEntity GetFirstOrDefault(int recordId)
        {
            return _dbSet.Find(recordId);
        }

        public TBlEntity GetFirstOrDefaultByParameter(Expression<Func<TBlEntity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<TBlEntity> GetListByParameter(Expression<Func<TBlEntity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public void Remove(TBlEntity entity)
        {
            if (_dbEntity.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void RemoveByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict)
        {
            TBlEntity entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict)
        {
            List<TBlEntity> entity = _dbSet.Where(wherePredict).ToList();
            foreach (TBlEntity ent in entity)
            {
                Remove(ent);
            }
        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict,
            Action<TBlEntity> forEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(forEachPredict);
            _dbEntity.SaveChanges();
        }

        /// <summary>
        ///     Executes procedure in database and returns result
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TBlEntity> GetResultBySqlProcedure(string query, params object[] parameters)
        {
            if (parameters != null)
                return Enumerable.ToList<TBlEntity>(_dbEntity.Database.SqlQuery<TBlEntity>(query, parameters));
            return Enumerable.ToList<TBlEntity>(_dbEntity.Database.SqlQuery<TBlEntity>(query));
        }

        public void DeleteMarkByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict,
            Action<TBlEntity> forEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(forEachPredict);
            _dbEntity.SaveChanges();
        }

        /// <summary>
        ///     Returns result by where clause in descending order
        /// </summary>
        /// <param name="orderByPredict"></param>
        /// <returns></returns>
        public IQueryable<TBlEntity> OrderByDescending(Expression<Func<TBlEntity, int>> orderByPredict)
        {
            if (orderByPredict == null)
            {
                return _dbSet;
            }

            return _dbSet.OrderByDescending(orderByPredict);
        }
    }
}