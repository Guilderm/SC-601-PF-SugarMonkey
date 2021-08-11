using System;
using SugarMonkey.DAL;

namespace SugarMonkey.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private readonly Online_ShoppingEntities DBEntity = new Online_ShoppingEntities();

        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
            return new GenericRepository<Tbl_EntityType>(DBEntity);
        }

        public void SaveChanges()
        {
            DBEntity.SaveChanges();
        }


        #region Disposing the Unit of work context ...

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DBEntity.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}