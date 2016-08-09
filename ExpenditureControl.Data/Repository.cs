using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;

namespace ExpenditureControl.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // do zastosowania nlogger
        //protected readonly Common.Logging.ILog Log = Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ISession NHSession { get; private set; }

        public Repository(ISession session)
        {
            NHSession = session;
        }

        public T Add(T entity)
        {
            NHSession.Save(entity);
            return entity;
        }

        public bool Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                NHSession.Save(item);
            }
            return true;
        }

        public T Update(T entity)
        {
            NHSession.Update(entity);
            return entity;
        }

        public bool Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                NHSession.Update(entity);
            }
            return true;
        }

        public T Merge(T entity)
        {
            NHSession.Merge(entity);
            return entity;
        }

        public bool Merge(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                NHSession.Merge(entity);
            }
            return true;
        }


        public bool Delete(T entity)
        {
            NHSession.Delete(entity);
            return true;
        }

        public bool Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                NHSession.Delete(entity);
            }
            return true;
        }

        public IQueryable<T> All()
        {
            return NHSession.Query<T>();
        }

        public T GetById(long id)
        {
            return NHSession.Get<T>(id);
        }

        public T GetBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }



        public bool HasOpenTransaction()
        {
            return NHSession.Transaction != null && NHSession.Transaction.IsActive && !NHSession.Transaction.WasCommitted && !NHSession.Transaction.WasRolledBack;
        }
        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return NHSession.BeginTransaction(isolationLevel);
        }
        public void CommitTransaction()
        {
            try
            {
                if (HasOpenTransaction())
                {
                    NHSession.Transaction.Commit();
                }
            }
            catch (HibernateException ex)
            {
                RollbackTransaction();
                throw new ApplicationException("something went wrong with an NH transaction", ex);
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                if (HasOpenTransaction())
                    NHSession.Transaction.Rollback();
            }
            finally
            {
                if (NHSession != null && NHSession.IsOpen)
                {
                    //NHSession.Flush();
                    // NHSession.Close();
                }
            }
        }
        public void CommitChanges()
        {
            if (HasOpenTransaction())
            {
                CommitTransaction();
            }
            else
            {
                // If there's no transaction, just flush the changes
                NHSession.Flush();
            }
        }

    }
}
