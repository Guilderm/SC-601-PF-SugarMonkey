using System;
using SugarMonkey.DAL;

namespace SugarMonkey.Models.Old.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private readonly Online_ShoppingEntities _dbEntity = new Online_ShoppingEntities();

        public IRepository<TBlEntityType> GetRepositoryInstance<TBlEntityType>() where TBlEntityType : class
        {
            return new GenericRepository<TBlEntityType>(_dbEntity);
        }

        public void SaveChanges()
        {
            _dbEntity.SaveChanges();
        }


        #region Disposing the Unit of work context ...

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbEntity.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}