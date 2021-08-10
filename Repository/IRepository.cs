using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SugarMonkey.Repository
{
    public interface IRepository<TBlEntity> where TBlEntity : class
    {
        IEnumerable<TBlEntity> GetAllRecords();
        IQueryable<TBlEntity> GetAllRecordsIQueryable();

        IEnumerable<TBlEntity> GetRecordsToShow(int pageNo, int pageSize, int currentPageNo,
            Expression<Func<TBlEntity, bool>> wherePredict, Expression<Func<TBlEntity, int>> orderByPredict);

        int GetAllRecordsCount();
        void Add(TBlEntity entity);
        void Update(TBlEntity entity);
        void UpdateByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict, Action<TBlEntity> forEachPredict);
        TBlEntity GetFirstOrDefault(int recordId);
        void Remove(TBlEntity entity);
        void RemoveByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict);
        void RemoveRangeByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict);

        void InactiveAndDeleteMarkByWhereClause(Expression<Func<TBlEntity, bool>> wherePredict,
            Action<TBlEntity> forEachPredict);

        TBlEntity GetFirstOrDefaultByParameter(Expression<Func<TBlEntity, bool>> wherePredict);
        IEnumerable<TBlEntity> GetListByParameter(Expression<Func<TBlEntity, bool>> wherePredict);
        IEnumerable<TBlEntity> GetResultBySqlProcedure(string query, params object[] parameters);
    }
}