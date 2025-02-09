﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Utility;

namespace WNMS.Service
{
    public class BaseService : IBaseService
    {
        #region Identity
        protected DbContext Context { get; private set; }
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="context"></param>
        public BaseService(DbContext context)
        {
            this.Context = context;
        }
        #endregion Identity

        #region Query
        public T Find<T>(int id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }
        public T Find<T>(long id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }
        public T Find<T>(params object[] keyValues) where T : class
        {
            return this.Context.Set<T>().Find(keyValues);
        }
        /// <summary>
        /// 不应该暴露给上端使用者，尽量少用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("尽量避免使用，using 带表达式目录树的代替")]
        public IQueryable<T> Set<T>() where T : class
        {
            return this.Context.Set<T>();
        }

        /// <summary>
        /// 这才是合理的做法，上端给条件，这里查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            if (funcWhere == null)
                return this.Context.Set<T>();
            else
                return this.Context.Set<T>().Where<T>(funcWhere);
        }

        public PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class
        {
            var list = this.Set<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }
        public PageResult<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true) where T : class
        {
            var list = this.Set<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }
        #endregion

        #region Insert
        /// <summary>
        /// 即使保存  不需要再Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : class
        {
            this.Context.Set<T>().Add(t);
            this.Commit();//写在这里  就不需要单独commit  不写就需要
            return t;
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().AddRange(tList);
            this.Commit();//一个链接  多个sql
            return tList;
        }
        #endregion

        #region Update
        /// <summary>
        /// 是没有实现查询，直接更新的,需要Attach和State
        /// 
        /// 如果是已经在context，只能再封装一个(在具体的service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public bool Update<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");

            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
            return true;
        }

        public bool Update<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
                this.Context.Entry<T>(t).State = EntityState.Modified;
            }
            this.Commit();
            return true;
        }

        #endregion

        #region Delete
        /// <summary>
        /// 先附加 再删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public bool Delete<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Attach(t);
            this.Context.Set<T>().Remove(t);
            this.Commit();
            return true;
        }

        /// <summary>
        /// 还可以增加非即时commit版本的，
        /// 做成protected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        public bool Delete<T>(int Id) where T : class
        {
            T t = this.Find<T>(Id);//也可以附加
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Remove(t);
            this.Commit();
            return true;
        }

        public bool Delete<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
            }
            this.Context.Set<T>().RemoveRange(tList);
            this.Commit();
            return true;
        }
        #endregion

        #region Other
        public void Commit()
        {
            this.Context.SaveChanges();
        }

        public IQueryable<T> ExcuteQuery<T>(string sql, Microsoft.Data.SqlClient.SqlParameter[] parameters) where T : class
        {
            //return this.Context.Database.SqlQuery<T>(sql, parameters).AsQueryable();
            var list = this.Context.Set<T>().FromSqlRaw<T>(sql, parameters);
            return list;
        }

        public PageResult<T> ExcuteQueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string sql, SqlParameter[] parameters, bool isAsc) where T : class, new()
        {
            IQueryable<T> list = Context.Database.SqlQuery<T>(sql, parameters).AsQueryable<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }
        public PageResult<T> ExcuteTwoQueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string functwoOrderby, string sql, SqlParameter[] parameters, bool isAsc) where T : class, new()
        {
            IQueryable<T> list = Context.Database.SqlQuery<T>(sql, parameters).AsQueryable<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            //if (isAsc)
            //{
            //    list = list.OrderBy(funcOrderby);
            //}
            //else
            //{
            //    list = list.OrderByDescending(funcOrderby);
            //}
            //if (!string.IsNullOrEmpty(functwoOrderby))
            //{
            //    list = list.OrderBy(functwoOrderby);
            //}
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }

        public void Excute<T>(string sql, SqlParameter[] parameters) where T : class
        {
            IDbContextTransaction trans = null;
            try
            {
                trans = this.Context.Database.BeginTransaction();
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                //this.Context.Database.ExecuteSqlCommand(sql, parameters);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
        }



        public virtual void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }


        #endregion
    }
}
