using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq.Expressions;
using WNMS.Utility;

namespace MongoDBHelper
{
    /// <summary>
    /// MongoDb 数据库操作类
    /// </summary>
    public class MongoDBHelper<T> where T : BaseEntity
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        private IMongoDatabase database;
        /// <summary>
        /// 连接字符串
        /// </summary>
        static string _conString = StaticConstraint.MongoDBConn;
        /// <summary>
        /// 数据库名
        /// </summary>
        static string _dbName = StaticConstraint.MongoDBName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conString">连接字符串</param>
        public MongoDBHelper(string year)
        {
            try
            {
                var url = new MongoUrl(_conString);
                MongoClientSettings mcs = MongoClientSettings.FromUrl(url);
                mcs.MaxConnectionLifeTime = TimeSpan.FromMilliseconds(1000);
                var client = new MongoClient(mcs);
                this.database = client.GetDatabase(_dbName + year);
            }
            catch (Exception ee)
            { }
        }
        /// <summary>
        /// 创建集合对象
        /// </summary>
        /// <param name="collName">集合名称</param>
        ///<returns>集合对象</returns>
        private IMongoCollection<T> GetColletion(String collName)
        {
            return database.GetCollection<T>(collName);
        }

        #region 增加
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="t">插入的对象</param>
        public void Insert(String collName, T t)
        {
            var coll = GetColletion(collName);
            coll.InsertOne(t);
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="ts">要插入的对象集合</param>
        public void InsertBath(String collName, IEnumerable<T> ts)
        {
            var coll = GetColletion(collName);
            coll.InsertMany(ts);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 按BsonDocument条件删除
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="document">文档</param>
        /// <returns></returns>
        //public Int64 Delete(String collName, QueryDocument document)
        //{
        //    var coll = GetColletion(collName);
        //    var result = coll.DeleteMany(document);
        //    return result.DeletedCount;
        //}
        /// <summary>
        /// 按json字符串删除
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public Int64 Delete(String collName, String json)
        {
            var coll = GetColletion(collName);
            var result = coll.DeleteMany(json);
            return result.DeletedCount;
        }
        /// <summary>
        /// 按条件表达式删除
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="predicate">条件表达式</param>
        /// <returns></returns>
        //public Int64 Delete(String collName, Expression<Func<T, Boolean>> predicate)
        //{
        //    var coll = GetColletion(collName);
        //    var result = coll.DeleteMany(predicate);
        //    return result.DeletedCount;
        //}
        /// <summary>
        /// 按检索条件删除
        /// 建议用Builders<T>构建复杂的查询条件
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public Int64 Delete(String collName, FilterDefinition<T> filter)
        {
            var coll = GetColletion(collName);
            var result = coll.DeleteMany(filter);
            return result.DeletedCount;
        }

        #endregion

        #region 修改
        /// <summary>
        /// 修改文档
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="filter">修改条件</param>
        /// <param name="update">修改结果</param>
        /// <param name="upsert">是否插入新文档（filter条件满足就更新，否则插入新文档）</param>
        /// <returns></returns>
        public Int64 Update(String collName, Expression<Func<T, Boolean>> filter, UpdateDefinition<T> update, Boolean upsert = false)
        {
            var coll = GetColletion(collName);
            var result = coll.UpdateMany(filter, update, new UpdateOptions { IsUpsert = upsert });
            return result.ModifiedCount;
        }
        /// <summary>
        /// 用新对象替换新文档-更新批量
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="filter">修改条件</param>
        /// <param name="t">新对象</param>
        /// <param name="upsert">是否插入新文档（filter条件满足就更新，否则true插入新文档）</param>
        /// <returns>修改影响文档数</returns>
        public Int64 UpdateT(String collName, FilterDefinition<T> filter, T t, Boolean upsert = false)
        {
            var coll = GetColletion(collName);
            BsonDocument document = t.ToBsonDocument<T>();
            document.Remove("_id");
            UpdateDocument update = new UpdateDocument("$set", document);
            var result = coll.UpdateMany(filter, update, new UpdateOptions { IsUpsert = upsert });
            return result.ModifiedCount;
        }
        /// <summary>
        /// 用新对象替换新文档-更新单个
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="filter">修改条件</param>
        /// <param name="t">新对象</param>
        /// <param name="upsert">是否插入新文档（filter条件满足就更新，否则true插入新文档）</param>
        /// <returns>修改影响文档数</returns>
        public Int64 UpdateOne(String collName, FilterDefinition<T> filter, T t, Boolean upsert = false)
        {
            var coll = GetColletion(collName);
            BsonDocument document = t.ToBsonDocument<T>();
            document.Remove("_id");
            UpdateDocument update = new UpdateDocument("$set", document);
            //var update = Builders<BsonDocument>.Update.Set("i", 110);
            var result = coll.UpdateOne(filter, update, new UpdateOptions { IsUpsert = upsert });
            return result.ModifiedCount;
        }

        /// <summary>
        /// 修改单个文档
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <param name="filter">修改条件</param>
        /// <param name="DataName">修改的字段名称</param>
        /// <param name="value">修改的值</param>
        /// <returns></returns>
        public Int64 UpDateSingle(string collName, FilterDefinition<T> filter, UpdateDefinition<T> update, Boolean upsert = false)
        {
            var coll = GetColletion(collName);
            var result = coll.UpdateOneAsync(filter, update).Result;
            return result.ModifiedCount;
        }
        #endregion

        #region 查询 

        /// <summary>
        /// 查询，复杂查询直接用Linq处理
        /// </summary>
        /// <param name="collName">集合名称</param>
        /// <returns>要查询的对象</returns>
        public IQueryable<T> GetQueryable(String collName)
        {
            var coll = GetColletion(collName);
            return coll.AsQueryable<T>();
        }
        /// <summary>
        /// 根据条件表达式返回可查询的记录源
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="sortPropertyName">排序表达式</param>
        /// <param name="isDescending">如果为true则为降序，否则为升序</param>
        /// <returns></returns>
        private IFindFluent<T, T> GetQueryable(String collName, FilterDefinition<T> query, string sortPropertyName, bool isDescending = true)
        {
            IMongoCollection<T> collection = GetColletion(collName);
            IFindFluent<T, T> queryable = collection.Find(query);
            var sort = isDescending ? Builders<T>.Sort.Descending(sortPropertyName) : Builders<T>.Sort.Ascending(sortPropertyName);
            return queryable.Sort(sort);
        }
        /// <summary>
        /// 根据条件表达式返回可查询的记录源
        /// </summary>
        /// <param name="match">查询条件</param>
        /// <param name="orderByProperty">排序表达式</param>
        /// <param name="isDescending">如果为true则为降序，否则为升序</param>
        /// <returns></returns>
        private IQueryable<T> GetQueryable<TKey>(String collName, Expression<Func<T, bool>> match, Expression<Func<T, TKey>> orderByProperty, bool isDescending = true)
        {
            IMongoCollection<T> collection = GetColletion(collName);
            IQueryable<T> query = collection.AsQueryable();

            if (match != null)
            {
                query = query.Where(match);
            }

            if (orderByProperty != null)
            {
                query = isDescending ? query.OrderByDescending(orderByProperty) : query.OrderBy(orderByProperty);
            }
            else
            {
                // query = query.OrderBy(sortPropertyName, isDescending);
            }
            return query;
        }
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="match">条件表达式</param>
        /// <param name="sortPropertyName">排序字段</param>
        /// <param name="isDescending">如果为true则为降序，否则为升序</param>
        /// <returns></returns>
        public IList<T> Find(String collName, Expression<Func<T, bool>> match, string sortPropertyName, bool isDescending = true)
        {
            return GetQueryable(collName, match, sortPropertyName, isDescending).ToList();
        }
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="query">条件表达式</param>
        /// <param name="sortPropertyName">排序字段</param>
        /// <param name="isDescending">如果为true则为降序，否则为升序</param>
        /// <returns></returns>
        public IList<T> Find(String collName, FilterDefinition<T> query, string sortPropertyName, bool isDescending = true)
        {
            return GetQueryable(collName, query, sortPropertyName, isDescending).ToList();
        }
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="match">条件表达式</param>
        /// <param name="orderByProperty">排序表达式</param>
        /// <param name="isDescending">如果为true则为降序，否则为升序</param>
        /// <returns></returns>
        public IList<T> Find<TKey>(String collName, Expression<Func<T, bool>> match, Expression<Func<T, TKey>> orderByProperty, bool isDescending = true)
        {
            return GetQueryable<TKey>(collName, match, orderByProperty, isDescending).ToList();
        }
        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <returns>存在则返回指定的第一个对象,否则返回默认值</returns>
        public T FindSingle(String collName, FilterDefinition<T> filter)
        {
            var coll = GetColletion(collName);
            return coll.Find(filter).FirstOrDefault();
        }
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="query">条件表达式</param>
        /// <param name="info">分页实体</param>
        /// <returns>指定对象的集合</returns>
        public IList<T> FindWithPager(String collName, FilterDefinition<T> query, PagerInfo info, string orderByProperty, bool isDescending = true)
        {
            int pageindex = (info.CurrenetPageIndex < 1) ? 1 : info.CurrenetPageIndex;
            int pageSize = (info.PageSize <= 0) ? 20 : info.PageSize;
            int excludedRows = (pageindex - 1) * pageSize;
            var find = GetQueryable(collName, query, orderByProperty, isDescending);
            info.RecordCount = (int)find.Count();
            return find.Skip(excludedRows).Limit(pageSize).ToList();
        }
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="match">条件表达式</param>
        /// <param name="info">分页实体</param>
        /// <returns>指定对象的集合</returns>
        //public IList<T> FindWithPager(String collName, Expression<Func<T, bool>> match, PagerInfo info, string orderByProperty, bool isDescending = true)
        //{
        //    int pageindex = (info.CurrenetPageIndex < 1) ? 1 : info.CurrenetPageIndex;
        //    int pageSize = (info.PageSize <= 0) ? 20 : info.PageSize;

        //    int excludedRows = (pageindex - 1) * pageSize;

        //    IQueryable<T> query = GetQueryable(collName,match, orderByProperty, isDescending);
        //    info.RecordCount = query.Count();

        //    return query.Skip(excludedRows).Take(pageSize).ToList();
        //}
        #endregion
        #region 查询指定字段
        /// <summary>
        /// 查询，返回指定字段
        /// </summary>
        ///  <param name="match">筛选条件</param>
        /// <param name="fields">要返回的字段</param>
        /// <returns>要查询的对象</returns>
        public IList<BsonDocument> FindByFields(String collName, FilterDefinition<T> match, FieldsDocument fields, string sortPropertyName, bool isDescending = true)
        {
            return GetQueryable(collName, match, fields, sortPropertyName, isDescending).ToList();
        }
        private List<BsonDocument> GetQueryable(String collName, FilterDefinition<T> query, FieldsDocument fields, string sortPropertyName, bool isDescending = true)
        {
            IMongoCollection<T> collection = GetColletion(collName);
            IFindFluent<T, T> queryable = collection.Find(query);
            var sort = isDescending ? Builders<T>.Sort.Descending(sortPropertyName) : Builders<T>.Sort.Ascending(sortPropertyName);
            //var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            if (sortPropertyName == "")
            {
                return queryable.Project(fields).ToList();
            }
            else
            {
                return queryable.Sort(sort).Project(fields).ToList();

            }

        }
        #endregion

        #region 聚合
        public IAsyncCursor<T> Aggregate(PipelineDefinition<T, T> stage, string collName)
        {
            var coll = GetColletion(collName);
            return coll.Aggregate(stage);
        }
        #endregion
    }

    /// <summary>
    /// 实体基类，方便生成_id
    /// </summary>
    public class BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        /// <summary>
        /// 给对象初值
        /// </summary>
        public BaseEntity()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
        }
    }
}