using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.HSJF.Framework.EntityFramework.Context
{
    public class ContextFactory
    {
        public partial class EFContextFactory
        {

            static readonly string tranKey = "CurrentTransaction";


            /// <summary>
            /// 帮我们返回本次用户请求内的数据库上下文，如果当前线程内没有上下文，那么创建一个上下文，并保证
            /// 上下文是实例在每一次请求内唯一 
            /// 在EF4.0以前使用ObjectsContext对象
            /// </summary>
            /// <returns></returns>
            public static T GetCurrentDbContext<T>() where T : DbContext, new()
            {
                try
                {
                    if (HttpContext.Current != null)
                    {
                        var dbContextKey = GetDbContextKey<T>();
                        var dbContext = HttpContext.Current.Items[dbContextKey] as T;
                        if (dbContext == null)
                        {
                            lock (HttpContext.Current.Items.SyncRoot)
                            {
                                dbContext = HttpContext.Current.Items[dbContextKey] as T;
                                if (dbContext == null)
                                {
                                    dbContext = NewContext<T>();
                                }
                            }
                        }
                        return dbContext;
                    }
                    else
                    {
                        return NewContext<T>();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// 设置参与事务的context
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dbcontext"></param>
            public static void SetTransactionContext<T>(T dbcontext) where T : DbContext
            {
                try
                {
                    if (HttpContext.Current != null)
                    {

                        var contextList = HttpContext.Current.Items[tranKey] as Dictionary<string, DbContext>;
                        if (contextList == null)
                        {
                            lock (HttpContext.Current.Items.SyncRoot)
                            {

                                contextList = new Dictionary<string, DbContext>();
                                contextList.Add(dbcontext.GetType().FullName, dbcontext);
                                HttpContext.Current.Items[tranKey] = contextList;
                            }
                        }
                        else
                        {
                            //((List<DbContext>)HttpContext.Current.Items[tranKey]).Add(dbcontext);
                            if (!contextList.ContainsKey(dbcontext.GetType().FullName))
                            {
                                contextList.Add(dbcontext.GetType().FullName, dbcontext);
                                HttpContext.Current.Items[tranKey] = contextList;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            /// <summary>
            /// 获取参与事务的context
            /// </summary>
            public static Dictionary<string, DbContext> TransactionContextList
            {
                get
                {
                    if (HttpContext.Current != null)
                    {
                        // var tranKey = "CurrentTransaction";
                        var contextList = HttpContext.Current.Items[tranKey] as Dictionary<string, DbContext>;
                        return contextList;
                    }
                    return null;
                }
                //  set { }
            }

            /// <summary>
            /// 完成后取消已经进入事务池的事务
            /// </summary>
            public static void FinishTransaction()
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items[tranKey] != null)
                    {
                        HttpContext.Current.Items.Remove(tranKey);
                    }
                }
            }

            /// <summary>
            /// 放弃所有事务缓存，变相清楚所有事务
            /// </summary>
            public static void RollBackAllChange()
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items[tranKey] != null)
                    {
                        var list = HttpContext.Current.Items[tranKey] as Dictionary<string, DbContext>;
                        if (list.Any())
                        {
                            foreach (var str in list.Keys)
                            {
                                if (HttpContext.Current.Items.Contains(str))
                                {
                                    HttpContext.Current.Items.Remove(str);
                                }
                            }
                        }
                    }
                }
                FinishTransaction();
            }

            private static T NewContext<T>() where T : DbContext, new()
            {
                var contextType = typeof(T);
                var dbcontext = Activator.CreateInstance(contextType) as T;
                var dbContextKey = GetDbContextKey<T>();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[dbContextKey] = dbcontext;
                }

                return dbcontext;
            }

            private static string GetDbContextKey<T>()
            {
                return typeof(T).FullName;
            }
        }
    }
}
