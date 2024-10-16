
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace WNMS.Utility
{
    public static class EntityFrameworkCoreExtensions
    {

        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            //链接状态判断
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            var cmd = conn.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            command.Parameters.Clear();
            conn.Close();

            return dt;
        }

        public static int InsertData(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            int data = 1;
            try
            {
                var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
                var reader = command.ExecuteReader();
                reader.Close();
                command.Parameters.Clear();
                conn.Close();
            }
            catch
            {
                data = 0;
            }
            return data;
        }

        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            var dt = SqlQuery(facade, sql, parameters);

            return dt.ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var propertyInfos = typeof(T).GetProperties();
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                list.Add(t);
            }
            return list;
        }

        public static IEnumerable<dynamic> SqlQuery_Dic(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            using (var dataReader = command.ExecuteReader())
            {

                while (dataReader.Read())
                {
                    var dataRow = GetDataRow(dataReader);
                    yield return dataRow;
                }
            }
            command.Parameters.Clear();
            conn.Close();
        }

        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}
