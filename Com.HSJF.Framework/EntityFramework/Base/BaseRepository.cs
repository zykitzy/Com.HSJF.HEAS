using Com.HSJF.Framework.EntityFramework.Context;
using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Com.HSJF.Framework.EntityFramework.Base
{
    public abstract class BaseRepository<T, D>
        where T : EntityModel
        where D : DbContext, new()
    {
        private DbContext _dbContext;
        public RepositoryFactory rf = new RepositoryFactory();

        /// <summary>
        /// 默认使用上下文
        /// </summary>
        public BaseRepository()
        {
            //获取的实当前线程内部的上下文实例，而且保证了线程内上下文实例唯一
            _dbContext = ContextFactory.EFContextFactory.GetCurrentDbContext<D>();
        }

        public DbContext GetCurrentContext()
        {
            return _dbContext;
        }

        #region 已废弃

        ///// <summary>
        ///// 指定使用上下文
        ///// </summary>
        ///// <param name="dbContext"></param>
        //public BaseRepository(DbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //    _dbContext.Configuration.AutoDetectChangesEnabled = false;
        //}

        #endregion 已废弃

        public virtual void Add(T entity)
        {
            //   _dbContext.
            _dbContext.Set<T>().Add(entity);
            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void AddRange(IEnumerable<T> entityList)
        {
            if (!entityList.Any()) return;
            foreach (var t in entityList)
            {
                _dbContext.Set<T>().Add(t);
            }
            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void Update(T entity)
        {
            var entry = _dbContext.Entry(entity);
            if (entry.State == EntityState.Unchanged)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            else if (entry.State == EntityState.Detached)
            {
                try
                {
                    _dbContext.Set<T>().Attach(entity);
                    _dbContext.Entry<T>(entity).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    T old = _dbContext.Set<T>().Find(typeof(T).GetProperty("ID").GetValue(entity));
                    _dbContext.Entry(old).CurrentValues.SetValues(entity);
                }
            }

            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void UpdateRange(IEnumerable<T> entityList)
        {
            foreach (var t in entityList)
            {
                T old = _dbContext.Set<T>().Find(typeof(T).GetProperty("ID").GetValue(t));
                if (old == null)
                {
                    Add(t);
                }
                else
                {
                    //    Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(t, model);
                    Update(t);
                }
            }

            //foreach (var t in entityList)
            //{
            //    Update(t);
            //}
            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void Delete(T entity)
        {
            var dbSet = _dbContext.Set<T>();
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void Delete(object key)
        {
            var entity = _dbContext.Set<T>().Find(key);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }
            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
        }

        public virtual void DeleteRange(IEnumerable<T> entityList)
        {
            var dbSet = _dbContext.Set<T>();
            if (entityList != null && entityList.Any())
            {
                dbSet.RemoveRange(entityList);
            }
        }

        public void SaveChanges()
        {
            var utcNow = DateTime.UtcNow;
            var entries = _dbContext.ChangeTracker.Entries().Where(ent =>
                    ent.State == EntityState.Added || ent.State == EntityState.Modified);

            #region 记录时间撮，暂时去除

            //foreach (var entry in entries)
            //{
            //    if (entry.Entity is ITimeTrace)
            //    {
            //        var entity = entry.Entity as ITimeTrace;
            //        if (entry.State == EntityState.Added)
            //        {
            //            entity.CreatedTime = utcNow;
            //        }
            //        entity.EditedTime = utcNow;
            //    }
            //}

            #endregion 记录时间撮，暂时去除

            _dbContext.ChangeTracker.DetectChanges();

            ContextFactory.EFContextFactory.SetTransactionContext(_dbContext);
            //dbc.ObjectContext.Connection.
        }

        //public void SaveChanges()
        //{
        //    IObjectContextAdapter dbc = _dbContext;

        //    _dbContext.SaveChanges();
        //    //dbc.ObjectContext.Connection.
        //}

        public virtual IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }
        public virtual IEnumerable<T> GetAllBySql(string sql)
        {
            // return _dbContext.Set<T>().Where(predicate);
            return _dbContext.Database.SqlQuery<T>(sql);
        }
        ///// <summary>
        ///// 获取数据时需要验证权限，验证时，更具配置的字段查询权限是否在传入的权限列表中
        ///// </summary>
        ///// <param name="datapermissions">拥有的权限</param>
        ///// <returns></returns>
        //public virtual IEnumerable<T> GetAllAuthorize(string[] datapermissions)
        //{
        //    var predicate = PredicateBuilder.True<T>();
        //    var attr = (DataAuthorizeAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DataAuthorizeAttribute));

        //    if (attr != null)
        //    {
        //        var aulist = attr.AuthorizeColumn.ToLower().Split(',');
        //        foreach (var pro in typeof(T).GetProperties())
        //        {
        //            if (aulist.Contains(pro.Name.ToLower()))
        //            {
        //                predicate = predicate.And(t => datapermissions.Contains(t.GetType().GetProperty(pro.Name).GetValue(t, null).ToString()));
        //            }
        //        }
        //    }
        //    return _dbContext.Set<T>().Where(predicate);
        //}

        //public virtual IEnumerable<T> GetAllAuthorizeAndSelf(string[] datapermissions, Expression<Func<T, bool>> username)
        //{
        //    var predicate = PredicateBuilder.True<T>();
        //    var attr = (DataAuthorizeAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DataAuthorizeAttribute));

        //    if (attr != null)
        //    {
        //        var aulist = attr.AuthorizeColumn.ToLower().Split(',');
        //        foreach (var pro in typeof(T).GetProperties())
        //        {
        //            if (aulist.Contains(pro.Name.ToLower()))
        //            {
        //                predicate = predicate.And(t => datapermissions.Contains(t.GetType().GetProperty(pro.Name).GetValue(t, null)));
        //            }
        //        }
        //        predicate = predicate.Or(username);
        //    }
        //    return _dbContext.Set<T>().Where(predicate);
        //}

        public virtual T Get(object key)
        {
            return _dbContext.Set<T>().Find(key);
        }

        ///// <summary>
        ///// 获取数据时需要验证权限，验证时，更具配置的字段查询权限是否在传入的权限列表中
        ///// </summary>
        ///// <param name="datapermissions">拥有的权限</param>
        ///// <returns></returns>
        //public virtual T GetAuthorize(object key, string[] datapermissions)
        //{
        //    var attr = (DataAuthorizeAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DataAuthorizeAttribute));
        //    var entity = _dbContext.Set<T>().Find(key);
        //    if (attr != null && entity != null)
        //    {
        //        var aulist = attr.AuthorizeColumn.ToLower().Split(',');
        //        foreach (var pro in typeof(T).GetProperties())
        //        {
        //            if (aulist.Contains(pro.Name.ToLower()))
        //            {
        //                if (!datapermissions.Contains(this.GetType().GetProperty(pro.Name).GetValue(entity, null)))
        //                {
        //                    return null;
        //                }
        //            }
        //        }
        //        return entity;
        //    }

        //    return null;
        //}

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        /// <summary>
        /// 提交所有Context 上的改动
        /// </summary>
        public void AcceptAllChange()
        {
            using (TransactionScope scop = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var list = ContextFactory.EFContextFactory.TransactionContextList;
                    if (list != null && list.Any())
                    {
                        foreach (var cont in list.Values)
                        {
                            cont.ChangeTracker.DetectChanges();
                            cont.SaveChanges();
                        }
                        scop.Complete();
                    }
                    ContextFactory.EFContextFactory.FinishTransaction();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void RollBackAllChange()
        {
            ContextFactory.EFContextFactory.RollBackAllChange();
        }

        public void AcceptAllChange(DbContext[] context)
        {
            using (TransactionScope scop = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (DbContext conte in context)
                {
                    conte.SaveChanges();
                }

                scop.Complete();

                foreach (DbContext conte in context)
                {
                    System.Data.Entity.Infrastructure.IObjectContextAdapter dbc = conte;
                    dbc.ObjectContext.AcceptAllChanges();
                }
            }
        }
    }
}