﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WNMS.IService
{
    public interface IBaseService : IDisposable//是为了释放Context
    {
        #region Query
        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(int id) where T : class;
        T Find<T>(long id) where T : class;

        /// <summary>
        /// 提供对单表的查询
        /// </summary>
        /// <returns>IQueryable类型集合</returns>
        [Obsolete("尽量避免使用，using 带表达式目录树的 代替")]
        IQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class;
        PageResult<T> QueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true) where T : class;
        #endregion

        #region Add
        /// <summary>
        /// 新增数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        /// <returns>返回带主键的实体</returns>
        T Insert<T>(T t) where T : class;

        /// <summary>
        /// 新增数据，即时Commit
        /// 多条sql 一个连接，事务插入
        /// </summary>
        /// <param name="tList"></param>
        IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region Update
        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        bool Update<T>(T t) where T : class;

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        bool Update<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region Delete
        /// <summary>
        /// 根据主键删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        bool Delete<T>(int Id) where T : class;

        /// <su+mary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        bool Delete<T>(T t) where T : class;

        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        bool Delete<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region Other
        /// <summary>
        /// 立即保存全部修改
        /// 把增/删的savechange给放到这里，是为了保证事务的
        /// </summary>
        void Commit();

        /// <summary>
        /// 执行sql 返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<T> ExcuteQuery<T>(string sql, Microsoft.Data.SqlClient.SqlParameter[] parameters) where T : class;

        /// <summary>
        /// 执行sql 分页 返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PageResult<T> ExcuteQueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string sql, SqlParameter[] parameters, bool isAsc) where T : class, new();
        /// <summary>
        /// 执行sql，无返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        void Excute<T>(string sql, SqlParameter[] parameters) where T : class;
        /// <summary>
        /// 执行sql 分页 返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="functwoOrderby"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PageResult<T> ExcuteTwoQueryPage<T>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string functwoOrderby, string sql, SqlParameter[] parameters, bool isAsc) where T : class, new();
        #endregion
    }
}
