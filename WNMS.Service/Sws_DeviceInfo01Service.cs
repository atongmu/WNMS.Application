using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class Sws_DeviceInfo01Service : BaseService, IService.ISws_DeviceInfo01Service
    {
        public PageResult<WNMS.Model.CustomizedClass.DeviceInfo01Info> LoadInfoList(Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true)
        {
            string sql = @"SELECT   d.DeviceID, d.DeviceName, d.DeviceNum, d.Partition, d.StationID, d.DeviceType, d.Frequency, d.ImageURL, d.Manufacturer, d.RTUID,
                           d.PumpNum, d.PumpType, d.GUI, d.ImportDN, d.ExportDN, d.ManufactureDate, d.RTUID,s.StationName,sd.ItemName as DeviceTypeName,sd1.ItemName as FrequencyName,
                           sd2.ItemName as ManufacturerName,sd3.ItemName as  PartitionName
                           FROM sws_DeviceInfo01 d  
                           left join Sws_Station s on d.StationID = s.StationID
                           left join Sys_DataItemDetail sd on  d.DeviceType = sd.ItemValue and sd.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =5)
                           
                           left join Sys_DataItemDetail sd1 on  d.Frequency = sd1.ItemValue and sd1.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =6)
                           
                           left join Sys_DataItemDetail sd2 on  d.Manufacturer = sd2.ItemValue and sd2.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =7)

                           left join Sys_DataItemDetail sd3 on  d.Partition = sd3.ItemValue and sd3.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =8)";
            Microsoft.Data.SqlClient.SqlParameter[] sqlparameter = new Microsoft.Data.SqlClient.SqlParameter[] { };
            var dataList = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            return dataList;
        }
        public PageResult<WNMS.Model.CustomizedClass.DeviceInfo01Info> LoadorderInfoList(Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string functwoOrderby, bool isAsc = true)
        {
            string sql = @"SELECT   d.DeviceID, d.DeviceName, d.DeviceNum, d.Partition, d.StationID, d.DeviceType, d.Frequency, d.ImageURL, d.Manufacturer, d.RTUID,
                           d.PumpNum, d.PumpType, d.GUI, d.ImportDN, d.ExportDN, d.ManufactureDate, d.RTUID,s.StationName,sd.ItemName as DeviceTypeName,sd1.ItemName as FrequencyName,
                           sd2.ItemName as ManufacturerName,sd3.ItemName as  PartitionName,sr.DeviceID AS RDeviceID
                           FROM sws_DeviceInfo01 d  
                            left join Sws_RTUInfo sr on d.Rtuid = sr.Rtuid
                           left join Sws_Station s on d.StationID = s.StationID
                           left join Sys_DataItemDetail sd on  d.DeviceType = sd.ItemValue and sd.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =5)
                           
                           left join Sys_DataItemDetail sd1 on  d.Frequency = sd1.ItemValue and sd1.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =6)
                           
                           left join Sys_DataItemDetail sd2 on  d.Manufacturer = sd2.ItemValue and sd2.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =7)

                           left join Sys_DataItemDetail sd3 on  d.Partition = sd3.ItemValue and sd3.F_ItemId in 
                           (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =8) order by StationName,Partition";
            Microsoft.Data.SqlClient.SqlParameter[] sqlparameter = new Microsoft.Data.SqlClient.SqlParameter[] { };
            var dataList = this.ExcuteTwoQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, functwoOrderby, sql, sqlparameter, isAsc);
            return dataList;
        }

        /// <summary>
        /// 设备泵房树形
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public IEnumerable<WNMS.Model.CustomizedClass.StationAndDevice> QueryZtreeInfo(SysUser user, string stationName)
        {
            string sql = "";
            int userID = user.UserId;
            stationName = stationName ?? "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationName", "%" + stationName + "%"),
                new SqlParameter("@userID",userID)
            };
            if (user.IsAdmin)
            {
                sql = @"select d.DeviceId,d.RTUID,d.DeviceName,d.Partition,s.StationId,s.StationName from Sws_DeviceInfo01 d left join Sws_Station s on d.StationID=s.StationID where s.StationId>0";
            }
            else
            {
                sql = @"select d.DeviceId,d.RTUID,d.DeviceName,d.Partition,s.StationId,s.StationName from Sws_DeviceInfo01 d left join Sws_Station s on d.StationID=s.StationID
                        where d.StationID in (select StationID from Sws_UserStation where UserID=@userID) and s.StationId>0";
            }
            if (!string.IsNullOrEmpty(stationName))
            {
                sql += @" and s.StationName like @stationName";
            }
            sql += @" order by d.Partition";
            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.StationAndDevice>(sql, sp);
            return query;
        }
        public IEnumerable<DeviceInfo01Info> LoadSwsDeviceInfo01Info(long EquipID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID)
             };

            var query = this.Context.Database.SqlQuery<DeviceInfo01Info>("exec GetSwsDeviceInfo01Info @EquipID", sp);
            return query;
        }

        //批量导入
        //添加设备  更新泵房类型
        public int DeviceInsert(SwsDeviceInfo01 swsDeviceInfo01)
        {
            var stationinfo = this.Context.Find<SwsStation>(swsDeviceInfo01.StationId);
            if (stationinfo != null)
            {
                if (stationinfo.InType == 0)
                {
                    stationinfo.InType = 1;
                    this.Context.Update<SwsStation>(stationinfo);
                }
                this.Context.Add<SwsDeviceInfo01>(swsDeviceInfo01);
            }
            return this.Context.SaveChanges();
        }


        //添加、修改
        public int SetAreaInfo(long DeviceID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@DeviceID",DeviceID)
           };
            var query = this.Context.Database.InsertData("exec UpdAreaRTU  @DeviceID", sp);
            return query;
        }
        //批量删除设备
        public int DeleteDevice(List<SwsDeviceInfo01> deleteDetail,ref string deleteName)
        { 
            try
            {
                foreach (var item in deleteDetail)
                {
                    var stationid = item.StationId;
                    //查询设备表中 该泵站是否还存在
                    var userstationlist = this.Context.Set<SwsDeviceInfo01>().Where(r => r.StationId == stationid && r.DeviceId != item.DeviceId).ToList();
                    if (userstationlist.Count == 0)
                    {
                        var stationinfo = this.Context.Find<SwsStation>(stationid);
                        stationinfo.InType = 0;
                        this.Context.Update<SwsStation>(stationinfo);
                    }
                    //查询该设备下的通讯是否还在用,不存在则更新通讯的所属泵房为0
                    var rtudevice = this.Context.Set<SwsDeviceInfo01>().Where(r => r.Rtuid == item.Rtuid && r.DeviceId != item.DeviceId).ToList();
                    if (rtudevice.Count == 0)
                    {
                        var rtuinfo = this.Context.Set<SwsRtuinfo>().Where(r => r.Rtuid == item.Rtuid).FirstOrDefault();
                        if (rtuinfo != null)
                        {
                            rtuinfo.StationId = 0;
                            this.Context.Update<SwsRtuinfo>(rtuinfo);
                        }
                    }
                    this.Context.Set<SwsDeviceInfo01>().RemoveRange(item);
                    deleteName += item.DeviceName + ",";
                    //删除文件夹
                    try
                    {
                        //发布之后地址可能需要修改
                        DirectoryInfo dir = new DirectoryInfo("../WNMS.Application/wwwroot/UploadImg");
                        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                        foreach (FileSystemInfo i in fileinfo)
                        {
                            if (i.Name == item.DeviceId.ToString())
                            {
                                if (i is DirectoryInfo)            //判断是否文件夹
                                {
                                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                                    subdir.Delete(true);          //删除子目录和文件
                                }
                                else
                                {
                                    //如果 使用了 streamreader 在删除前 必须先关闭流 ，否则无法删除 sr.close();
                                    System.IO.File.Delete(i.FullName);      //删除指定文件
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    this.Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }


        public int DelAreaInfo(string id)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@DeviceIDs",id)
           };
            var query = this.Context.Database.InsertData("exec DelAreaRTU  @DeviceIDs", sp);
            return query;
        }
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int DeviceImport(List<SwsDeviceInfo01> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var stationfo = this.Context.Set<SwsStation>().Where(r => r.StationId == item.StationId).FirstOrDefault();
                    if (stationfo != null && stationfo.InType == 0)
                    {
                        stationfo.InType = 1;
                        this.Context.Update<SwsStation>(stationfo);
                    }
                    this.Context.Add<SwsDeviceInfo01>(item);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 设备泵房树形
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public IEnumerable<WNMS.Model.CustomizedClass.StationAndDevice> QueryZtreeInfoold(SysUser user, string stationName)
        {
            string sql = "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] { };
            if (user.IsAdmin == true)
            {
                if (!string.IsNullOrEmpty(stationName))
                {
                    splist.Add(new SqlParameter("@stationName", stationName));
                }
                sp = splist.ToArray();
                sql = @"select DISTINCT ss.StationID,ss.StationName,sd.DeviceID,sd.StationID as DeviceStationId from Sws_Station ss 
                         left join Sws_DeviceInfo01 sd on ss.StationID = sd.StationID
                         left join Sws_UserStation su on ss.StationID = su.StationID
                         where ss.StationID  in (select sd1.StationID from Sws_DeviceInfo01 sd1)";
            }
            else
            {

            }
            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.StationAndDevice>(sql, sp);
            return query;
        }
        /// <summary>
        /// 设备历史数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="collName">集合名称</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Model.CustomizedClass.HistoryJKData> GetMongoHistoryData(string year, string collName, string beginDate, string endDate, string order, string sort, int pageIndex, int pageSize, ref int totalCount)
        {
            MongoDBHelper<Model.CustomizedClass.HistoryJKData> mongoHelper = new MongoDBHelper<Model.CustomizedClass.HistoryJKData>(year);

            //查询条件
            FilterDefinition<Model.CustomizedClass.HistoryJKData> filter = "{ 'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') } }";
            //排序
            bool isDescending = true;
            if (sort == "asc")
            {
                isDescending = false;
            }
            //分页
            PagerInfo info = new PagerInfo
            {
                CurrenetPageIndex = pageIndex,
                PageSize = pageSize
            };

            //数据查询
            var list = mongoHelper.FindWithPager(collName, filter, info, order, isDescending);
            totalCount = info.RecordCount;
            return list;
        }
        /// <summary>
        /// 矫正数据，数据获取指定字段数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="collName">集合名称</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dataID">所取的值的dataID</param>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetCorrectData(string year, string collName, string beginDate, string endDate, string dataID)
        {
            MongoDBHelper<HistoryJKData> mongoHelper = new MongoDBHelper<HistoryJKData>(year);

            //查询条件
            FilterDefinition<HistoryJKData> filter = "{ 'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') } }";

            string f = "AnalogValues." + dataID;

            //获取的指定字段
            FieldsDocument fildes = new FieldsDocument();
            fildes.Add("UpdateTime", 1);
            fildes.Add(f, 1);

            //数据查询
            var list = mongoHelper.FindByFields(collName, filter, fildes, "UpdateTime");
            return list;
        }
        /// <summary>
        /// 矫正数据（更新数据）
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="collName">集合名称（设备EquipmentID）</param>
        /// <param name="dateTime">对应一条数据的更新时间</param>
        /// <param name="hdata">要更新的数据</param>
        /// <returns></returns>
        public long UpdateCorrectData(string year, string collName, string id, string dataName, object value)
        {
            MongoDBHelper<HistoryJKData> mongoHelper = new MongoDBHelper<HistoryJKData>(year);
            ObjectId a = new ObjectId(id);
            var filter = Builders<HistoryJKData>.Filter.Eq("_id", a);

            var updated = Builders<HistoryJKData>.Update.Set(dataName, value);

            long num = mongoHelper.UpDateSingle(collName, filter, updated);
            return num;
        }
        //修改关注
        public int UpdateFocusOn(int id, bool focusOn, long userid, int type)
        {
            var tempInfo = this.Context.Find<SwsTemplate>(id);
            if (focusOn == true)
            {
                tempInfo.FocusOn = false;
                this.Context.Update(tempInfo);
            }
            else
            {
                tempInfo.FocusOn = true;
                this.Context.Update(tempInfo);
                //查找是否存在关注的模板
                var tempInfoList = this.Context.Set<SwsTemplate>().Where(t => t.FocusOn == true && t.UserId == userid && t.Classify == 1 && t.DeviceType == type).ToList();
                if (tempInfoList.Count > 0)
                {
                    foreach (var item in tempInfoList)
                    {
                        item.FocusOn = false;
                        this.Context.Update(item);
                    }
                }

            }
            return this.Context.SaveChanges();
        }

        #region 数据监测模块
        public IEnumerable<dynamic> GetOverTimeHiatrory(string Time, int Num, double interval, string Dev_BeginDate, int DataID, bool? IsCumulation, string rtuId)
        {
            DateTime beginDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime Date = Convert.ToDateTime(Dev_BeginDate);

            string group = "";
            string project = "";
            //var interval = 1000 * 60 * 60;
            if (Time == "day")
            {
                endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(1);
                beginDate = Date.AddDays(1 - Num);

                if (IsCumulation == true)
                {
                    beginDate = beginDate.AddHours(-1);
                    group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                         "" + interval + "]}]},'maxdata':{$max:'$AnalogValues." + DataID + "'}, 'mindata':{ $first:'$AnalogValues." + DataID + "'}}}";

                    project = @"{$project: {'_id': 0,'data':'$mindata','datetime': { $add:[new Date(-28800000),'$_id']}}}";
                }
                else
                {
                    group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                        "" + interval + "]}]},'data':{$avg:'$AnalogValues." + DataID + "'}}}";

                    project = @"{$project: {'_id': 0,'data':1,'datetime': { $add: [new Date(-28800000), '$_id']}, 'group':{$dateToString:{'format':'%Y-%m-%d','date':{$add: [new Date(-28800000), '$_id']}}}}}";
                }
            }
            if (Time == "week")
            {
                var zhouji = Date.DayOfWeek;
                var num = (int)System.Enum.Parse(typeof(WeekDay), zhouji.ToString());
                if (num == 1)
                {
                    num = 8;
                }
                beginDate = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd}", Date.AddDays(2 - num - (Num - 1) * 7)));

                if (IsCumulation == true)
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(2);
                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'maxdata':{$max:'$AnalogValues." + DataID + "'}, 'mindata':{ $first:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':'$mindata', 'datetime':'$_id'}}";
                }
                else
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(1);

                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'data':{$avg:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':1,'datetime':'$_id'}}";
                }
            }
            if (Time == "month")
            {

                beginDate = Convert.ToDateTime(Date.ToString("yyyy-MM") + "-01").AddMonths(1 - Num);
                if (IsCumulation == true)
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(2);
                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'maxdata':{$max:'$AnalogValues." + DataID + "'}, 'mindata':{ $first:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':'$mindata','datetime':'$_id'}}";
                }
                else
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(1);
                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'data':{$avg:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':1,'datetime':'$_id'}}";

                }
            }
            if (Time == "year")
            {
                beginDate = Convert.ToDateTime(Date.ToString("yyyy") + "-01-01").AddYears(1 - Num);
                if (IsCumulation == true)
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddMonths(2);
                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m','date':'$UpdateTime'}}, 'mindata':{ $first:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':'$mindata','datetime':'$_id'}}";
                }
                else
                {
                    endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddMonths(1);
                    group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m','date':'$UpdateTime'}},'data':{$avg:'$AnalogValues." + DataID + "'}}}";
                    project = @"{$project:{'_id':0,'data':1,'datetime':'$_id'}}";

                }
            }
            string match = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + beginDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + endDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')} }}";
            string sort = "{'$sort': {'datetime': 1}}";
            PipelineStageDefinition<dynamic, dynamic> pmatch = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> pgroup = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> pproject = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> psort = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(pmatch);
            stages.Add(pgroup);
            stages.Add(pproject);
            stages.Add(psort);

            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            var client = new MongoClient(_conntionString);//连接 
            var database = client.GetDatabase(_dbName + beginDate.Year.ToString());
            var collection = database.GetCollection<dynamic>(rtuId);
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var data = collection.Aggregate(pipeline).ToList();
            if (beginDate.Year != endDate.Year)
            {
                var beginYear_flag = beginDate.Year + 1;
                while (beginYear_flag <= endDate.Year)
                {
                    database = client.GetDatabase(_dbName + beginYear_flag.ToString());
                    collection = database.GetCollection<dynamic>(rtuId);
                    var dataappend = collection.Aggregate(pipeline).ToList();
                    if (dataappend.Count > 0)
                    {
                        data = data.Concat(dataappend).ToList();
                    }
                    beginYear_flag = beginYear_flag + 1;
                }

            }
            return data;
        }

        enum WeekDay
        {
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7,
            Sunday = 1,
        }

        /// <summary>
        /// 历史曲线，获取叠加点数据
        /// </summary>
        /// <param name="year">年份  </param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="group">分组</param>
        /// <param name="project">重新定义字段的字符串</param>
        /// <param name="collectName">集合名称</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetHistoryChartData(string year, string beginDate, string endDate, string group, string project, string collectName)
        {
            //定义mongodb
            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            //string connectstring = _conntionString + "/" + _dbName ;
            var client = new MongoClient(_conntionString);
            var database = client.GetDatabase(_dbName + year);

            //定义参数
            string match = "{$match:{'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') }}}";
            string sort = "{$sort:{'Time':1}}";
            PipelineStageDefinition<dynamic, dynamic> matchjson = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> projectjson = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> groupjson = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> sortjson = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(groupjson);
            stages.Add(projectjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var result = database.GetCollection<dynamic>(collectName).Aggregate(pipeline);
            return result.ToList();
        }
        #endregion

        #region 运行概况模块数据获取
        public PageResult<Device01Data> GetSituationData(Expression<Func<Device01Data, bool>> funcWhere, string datebaseName, SysUser user, string beginDate, string endDate, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userID",user.UserId),
                new SqlParameter("@beginDate",beginDate),
                new SqlParameter("@endDate",endDate)
            };

            string sql = @"with t as (select StationID, count(ID) as Num  from [dbo].[Sws_EventHistory] e left join Sws_RTUInfo r on e.RTUID=r.RTUID group by StationID),
                            g as (select StationID,Sum(Case when DataKey='EnergyCon' then DataValue else 0 end) as EnergyCon, Sum(Case when DataKey='FlowCon' 
                            then DataValue else 0 end) as FlowCon  from [dbo].[" + datebaseName + "] where UpdateTime >@beginDate and UpdateTime <=@endDate  group by StationID)" +
                            @"select s.StationID,s.StationName, ROUND(g.EnergyCon,2) as EnergyCon,ROUND(g.FlowCon,2) as FlowCon,t.Num as Num 
                            from Sws_Station s left join  g on g.StationID=s.StationID left join t on s.StationID=t.StationID where s.Intype=1";
            if (!user.IsAdmin)     //非admin查询
            {
                sql += " and s.StationID in (select StationID from Sws_UserStation where UserID=@userID)";
            }
            var presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);

            return presult;
        }

        #endregion
        /// <summary>
        /// 历史数据导出数据获取
        /// </summary>
        /// <param name="year">年份，用于数据库名称</param>
        /// <param name="collName">集合名称</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IEnumerable<HistoryJKData> GetExportHistoryData(string year, string collName, string beginDate, string endDate)
        {
            MongoDBHelper<HistoryJKData> mongoHelper = new MongoDBHelper<HistoryJKData>(year);

            //查询条件
            FilterDefinition<HistoryJKData> filter = "{ 'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') } }";

            //数据查询
            var list = mongoHelper.Find(collName, filter, "UpdateTime");
            return list;
        }
        //运行分析
        public IEnumerable<dynamic> QueryStationTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string sortName, string order, string userid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@beginTime",beginTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@type",type),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@sortName",sortName),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@userid",userid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryNewRunEvaluation @IsAdmin,@beginTime,@endTime,@type,@startindex,@pageSize,@sortName,@orderItems,@userid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[9].Value);
            return query;
        }
        //运行分析设备
        public IEnumerable<dynamic> QueryDeviceStationTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string tableName, string sortName, string order, string userid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@beginTime",beginTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@tableName",tableName),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@sortName",sortName),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@userid",userid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryDeviceRunEvaluation @IsAdmin,@beginTime,@endTime,@tableName,@startindex,@pageSize,@sortName,@orderItems,@userid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[9].Value);
            return query;
        }
        //运行设备分析同比排名
        public IEnumerable<dynamic> QueryDevicePreTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string tableName, string tbeginTime, string tendTime, string userid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@beginTime",beginTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@tableName",tableName),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@tbeginTime",tbeginTime),
                new SqlParameter("@tendTime",tendTime),
                new SqlParameter("@userid",userid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryDevicePreEvaluation @IsAdmin,@beginTime,@endTime,@tableName,@startindex,@pageSize,@tbeginTime,@tendTime,@userid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[9].Value);
            return query;
        }
        //运行分析同比排名
        public IEnumerable<dynamic> QueryPreTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string tbeginTime, string tendTime, int userid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@beginTime",beginTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@type",type),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@tbeginTime",tbeginTime),
                new SqlParameter("@tendTime",tendTime),
                new SqlParameter("@userid",userid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryPreEvaluationNew @IsAdmin,@beginTime,@endTime,@type,@startindex,@pageSize,@tbeginTime,@tendTime,@userid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[9].Value);
            return query;
        }
        //查询能耗统计 
        public IEnumerable<ConsumptionChart> GetConsumptionSum(bool isadmin, int userid, string beginTime, string endTime, string type)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid),
            new SqlParameter("@beginTime",beginTime),
            new SqlParameter("@endTime",endTime)
            };
            string sql = "";
            if (isadmin == true)
            {
                if (type == "1")
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from DayQuartZ  d  
                         
                        where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime";
                }
                else if (type == "4")
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from HourQuartZ  d  
                        
                        where d.UpdateTime >=@beginTime and  d.UpdateTime <= @endTime";
                }
                else
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from MonthQuartZ  d  
                        
                        where d.UpdateTime >=@beginTime and  d.UpdateTime <= @endTime";
                }

            }
            else
            {
                if (type == "1")
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from DayQuartZ  d  
                        left join Sws_UserStation s on d.StationID = s.StationID and s.UserID =@userid where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime";
                }
                else if (type == "4")
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from HourQuartZ  d  
                        left join Sws_UserStation s on d.StationID = s.StationID and s.UserID =@userid where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime";
                }
                else
                {
                    sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from MonthQuartZ  d  
                        left join Sws_UserStation s on d.StationID = s.StationID and s.UserID =@userid where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime";
                }

            }
            var query = this.Context.Database.SqlQuery<ConsumptionChart>(sql, sp);
            return query;
        }
        //查询设备能耗统计新 
        public IEnumerable<ConsumptionChart> GetDeviceConsumptionSumNew(bool isadmin, string userid, string beginTime, string endTime, string tableName)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid),
            new SqlParameter("@beginTime",beginTime),
            new SqlParameter("@endTime",endTime)
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from " + tableName + "  d " +

                        "where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime";
            }
            else
            {
                sql = @"select SUM(EnergyCon)AS EnergyCon,sum(FlowCon) as FlowCon from " + tableName + "  d  " +
                     "where d.UpdateTime >=@beginTime and  d.UpdateTime <=@endTime " +
                     "and d.deviceid in (select deviceid from Sws_DeviceInfo01 where stationid in (select stationid from Sws_UserStation where userid = @userid))";
            }
            var query = this.Context.Database.SqlQuery<ConsumptionChart>(sql, sp);
            return query;
        }

        //查询能耗统计新 
        public IEnumerable<ConsumptionChart> GetConsumptionSumNew(bool isadmin, int userid, string beginTime, string endTime, string type)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid),
            new SqlParameter("@beginTime",beginTime),
            new SqlParameter("@endTime",endTime)
            };
            string sql = "";
            if (isadmin == true)
            {
                if (type == "3")
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[MonthQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             " ),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +
                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n  ";
                }
                else if (type == "4")
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[HourQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             " ),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +
                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n ";
                }
                else
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[DayQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             " ),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +
                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n ";
                }

            }
            else
            {
                if (type == "3")
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[MonthQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             " left join Sws_UserStation su on s.StationID = su.StationID where  su.UserID = @userid),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +
                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n ";
                }
                else if (type == "4")
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[HourQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             " left join Sws_UserStation su on s.StationID = su.StationID where  su.UserID = @userid),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +
                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n ";
                }
                else
                {
                    sql = @"with t as(SELECT  s.StationName,  s.StationID, d.UpdateTime, d.DataKey, d.DataValue
                             from [dbo].[Sws_Station] s  left join 
                              [dbo].[DayQuartZ01] d on s.StationID = d.StationID and UpdateTime >=@beginTime and  UpdateTime <=@endTime and DataKey in ('EnergyCon','FlowCon')" +
                             "  left join Sws_UserStation su on s.StationID = su.StationID where  su.UserID = @userid),  r as(" +
                              "select StationName StationID,UpdateTime,EnergyCon,FlowCon  from t PIVOT(max(DataValue) FOR DataKey IN(EnergyCon,FlowCon)) as APVT)," +
                              " m as( select *,case when  FlowCon is null or FlowCon=0 or  EnergyCon  is null then null" +
                            " else (EnergyCon/FlowCon) end tonwater from r)," +
                           " n as( select  StationID,sum(isnull(EnergyCon,0)) as EnergyCon," +
                            "sum(isnull(FlowCon, 0)) as FlowCon  from m group by StationID)" +

                       "select sum(isnull(EnergyCon,0)) as EnergyCon,sum(isnull(FlowCon, 0)) as FlowCon from n ";
                }
            }
            var query = this.Context.Database.SqlQuery<ConsumptionChart>(sql, sp);
            return query;
        }

        #region  模板左侧泵房信息
        //模板左侧泵房信息
        public IEnumerable<dynamic> GetStationTree(int userid, bool isadmin, string stationName)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid),
            new SqlParameter("@stationName","%" + stationName + "%")
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"select distinct s.StationID,StationName,InType from [dbo].[Sws_Station] s 
                        left join Sws_UserStation su on s.StationID = su.StationID  where 1=1 and   s.InType <> 0  ";
                if (!string.IsNullOrEmpty(stationName))
                {
                    sql += " and StationName like @stationName";
                }
            }
            else
            {
                sql = @"select distinct s.StationID,StationName,InType from [dbo].[Sws_Station] s 
                        left join Sws_UserStation su on s.StationID = su.StationID  where 1=1 and   s.InType <> 0 and su.UserID = @userid ";
                if (!string.IsNullOrEmpty(stationName))
                {
                    sql += " and StationName like @stationName";
                }
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //模板右侧设备列表
        public IEnumerable<dynamic> GetStationDevice(int stationid, int tempid, string tablename, int f_itemid, string deviceids)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@tempid",tempid),
                new SqlParameter("@tablename",tablename),
                new SqlParameter("@f_itemid",f_itemid),
                new SqlParameter("@deviceids",deviceids)
           };

            string sql = @"select d.DeviceID,d.DeviceName,d.RTUID,ItemName as partionname,case when g.DeviceID is not null then 1 else  0 end hasCheck from [dbo].[" + @tablename + "] d " +
  @"left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) t on d.Partition=t.ItemValue
left join (select * from  [dbo].[Sws_DeviceTemplate] where TemplateID=@tempid) m on m.DeviceID=d.DeviceID
left join (select * from [dbo].[" + @tablename + "] where CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',')>0 ) g on d.DeviceID=g.DeviceID " +
   @"where d.StationID=@stationid";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }

        //模板泵房列表
        public IEnumerable<dynamic> GetStationInfo( int tempid, string deviceids)
        {
            SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@tempid",tempid), 
                new SqlParameter("@deviceids",deviceids)
           };

            string sql = @"select d.StationID ,d.StationName,case when g.StationID is not null then 1 else  0 end hasCheck from Sws_Station d
                           left join (select * from  [dbo].[Sws_DeviceTemplate] where TemplateID=@tempid) m on m.DeviceID=d.StationID
                           left join (select * from [dbo].[Sws_Station] where CHARINDEX(','+LTRIM(StationID)+',',','+@deviceids+',')>0 ) g on d.StationID=g.StationID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion

        #region 查询阀门信息
        public IEnumerable<WNMS.Model.CustomizedClass.FmInfo> QueryFaInfo(int sid, int type)
        {
            string sql = "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@sid",sid)
            };

            sql = @" select v.*,d.DeviceName,d.Partition,D.RtuId from Sws_ValveWith01 v  
                     left join Sws_DeviceInfo0" + type + "  d on v.deviceid = d.deviceid " +
                     " left join Sws_Station ss on d.stationid = ss.stationid " +
                     " where ss.stationid = @sid ";
            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.FmInfo>(sql, sp);
            return query;
        }
        public IEnumerable<WNMS.Model.CustomizedClass.FmInfo> QueryZFaInfo(int sid)
        {
            string sql = "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@sid",sid)
            };

            sql = @" select v.*,d.DeviceName,d.Partition,D.RtuId from Sws_ValveWith01 v 
                     left join Sws_DeviceInfo02 d on v.deviceid = d.deviceid
                     left join Sws_Station ss on d.stationid = ss.stationid
                     where ss.stationid = @sid";
            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.FmInfo>(sql, sp);
            return query;
        }
        #endregion

        #region  全览
        //全览报警设备
        public IEnumerable<dynamic> QueryEventDevice()
        {
            string sql = "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] {

            };

            sql = @"with t as  (select sd1.RTUID from Sws_DeviceInfo01 sd1  
                    union all select sd2.RTUID from Sws_DeviceInfo02 sd2 )
                    select distinct RTUID from t where RTUID in (select rtuid from Sws_EventInfo)";
            var query = this.Context.Database.SqlQuery<dynamic>(sql, sp);
            return query;
        }
        //设备增长

        public IEnumerable<dynamic> loadDeviceUp(SysUser user, DateTime? startTime, DateTime? endTime)
        {
            string sql = string.Empty;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startTime",startTime),
                 new SqlParameter("@endTime",endTime),
                  new SqlParameter("@UserId",user.UserId)
            };
            if (user.IsAdmin == true)
            {
                sql = @"with t as ((select sd1.DeviceName, sd1.CreateTime from Sws_DeviceInfo01 sd1    where sd1.CreateTime >@startTime and sd1.CreateTime <@endTime )"
                         + @"union all (select sd2.DeviceName, sd2.CreateTime from Sws_DeviceInfo02 sd2   where sd2.CreateTime >@startTime and sd2.CreateTime <@endTime ))"
                         + @"select count(*) as CountRtu,month(t.CreateTime)   as  CreateTime from t group by   month(t.CreateTime)";
            }
            else
            {
                sql = @"with t as ((select sd1.DeviceName, sd1.CreateTime from Sws_DeviceInfo01 sd1  left join Sws_UserStation su on sd1.StationID = su.StationID where su.Userid =@UserId and sd1.CreateTime >@startTime and sd1.CreateTime <@endTime)" +
                         " union all (select sd2.DeviceName, sd2.CreateTime from Sws_DeviceInfo02 sd2 left join Sws_UserStation su on sd2.StationID = su.StationID where su.Userid =@UserId and sd2.CreateTime >@startTime and sd2.CreateTime <@endTime))" +
                         " select count(*) as CountRtu,month(t.CreateTime)   as  CreateTime from t  group by   month(t.CreateTime)";
            }
            //var EventInfoList = this.Context.Database.SqlQuery<RtuCount>(sql,sp);
            var EventInfoList = this.Context.Database.SqlQuery_Dic(sql, sp);
            return EventInfoList;
        }
        #endregion
        #region 瞬时流量
        public IEnumerable<dynamic> GetOverTimeHiatroryAll(string Time, int Num, double interval, string Dev_BeginDate, int DataID, bool? IsCumulation, string rtuId, string[] sstr)
        {
            DateTime beginDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime Date = Convert.ToDateTime(Dev_BeginDate);

            string group = "";
            string project = "";
            //var interval = 1000 * 60 * 60;
            if (Time == "day")
            {
                endDate = Convert.ToDateTime(Date.ToString("yyyy-MM-dd")).AddDays(1);
                beginDate = Date.AddDays(1 - Num);

                if (IsCumulation == true)//智能泵房
                {
                    group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                         "" + interval + "]}]},'data1':{$avg:'$AnalogValues." + sstr[0] + "'}}}";

                    project = @"{$project: {'_id': 0,'data1':{$ifNull:['$data1', 0.0]},'datetime': { $add: [new Date(-28800000), '$_id']}, 'group':{$dateToString:{'format':'%Y-%m-%d','date':{$add: [new Date(-28800000), '$_id']}}}}}";
                }
                else//其他分区
                {
                    group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                        "" + interval + "]}]},'data1':{$avg:'$AnalogValues." + sstr[0] + "'},'data2':{$avg:'$AnalogValues." + sstr[1] + "'},'data3':{$avg:'$AnalogValues." + sstr[2] + "'},'data4':{$avg:'$AnalogValues." + sstr[3] + "'}}}";

                    //project = @"{$project: {'_id': 0,'data1':{$ifNull:['$data1', 0.0]},'data2':{$ifNull:['$data2', 0.0]},'data3':{$ifNull:['$data3', 0.0]},'data4':{$ifNull:['$data4', 0.0]},'datetime': { $add: [new Date(-28800000), '$_id']}, 'group':{$dateToString:{'format':'%Y-%m-%d','date':{$add: [new Date(-28800000), '$_id']}}}}}";
                    project = @"{$project: {'_id': 0,'data1':{$add:[{$ifNull:['$data1', 0.0]},{$ifNull:['$data2', 0.0]},{$ifNull:['$data3', 0.0]},{$ifNull:['$data4', 0.0]}]},'datetime': { $add: [new Date(-28800000), '$_id']}, 'group':{$dateToString:{'format':'%Y-%m-%d','date':{$add: [new Date(-28800000), '$_id']}}}}}";

                }
            }

            string match = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + beginDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + endDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')} }}";
            string sort = "{'$sort': {'datetime': 1}}";
            PipelineStageDefinition<dynamic, dynamic> pmatch = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> pgroup = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> pproject = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> psort = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(pmatch);
            stages.Add(pgroup);
            stages.Add(pproject);
            stages.Add(psort);

            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            var client = new MongoClient(_conntionString);//连接 
            var database = client.GetDatabase(_dbName + beginDate.Year.ToString());
            var collection = database.GetCollection<dynamic>(rtuId);
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var data = collection.Aggregate(pipeline).ToList();
            if (beginDate.Year != endDate.Year)
            {
                database = client.GetDatabase(_dbName + endDate.Year.ToString());
                collection = database.GetCollection<dynamic>(rtuId);
                var dataappend = collection.Aggregate(pipeline).ToList();
                if (dataappend.Count > 0)
                {
                    data = data.Concat(dataappend).ToList();
                }
            }
            return data;
        }
        #endregion

        #region K线图
        public IEnumerable<dynamic> GetDeviceJKData(DateTime beginDate, DateTime endDate, int DataID, string rtuId)
        {
            ///
            //DateTime beginDate = new DateTime();
            //DateTime endDate = new DateTime();
            //DateTime Date = Convert.ToDateTime(Dev_BeginDate);

            string group = "";
            string project = "";
            group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'firstdata':{$first:'$AnalogValues." + DataID + "'}, 'lastdata':{$last:'$AnalogValues." + DataID + "'}, 'maxdata':{$max:'$AnalogValues." + DataID + "'}, 'mindata':{ $min:'$AnalogValues." + DataID + "'}}}";
            project = @"{$project:{'_id':0,'firstdata':1,'lastdata':1,'maxdata':1,'mindata':1,'datetime':'$_id'}}";

            string match = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + beginDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + endDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')} }}";
            string sort = "{'$sort': {'datetime': 1}}";
            PipelineStageDefinition<dynamic, dynamic> pmatch = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> pgroup = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> pproject = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> psort = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(pmatch);
            stages.Add(pgroup);
            stages.Add(pproject);
            stages.Add(psort);

            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            var client = new MongoClient(_conntionString);//连接 
            var database = client.GetDatabase(_dbName + beginDate.Year.ToString());
            var collection = database.GetCollection<dynamic>(rtuId);
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var data = collection.Aggregate(pipeline).ToList();
            if (beginDate.Year != endDate.Year)
            {
                var beginYear_flag = beginDate.Year + 1;
                while (beginYear_flag <= endDate.Year)
                {
                    database = client.GetDatabase(_dbName + beginYear_flag.ToString());
                    collection = database.GetCollection<dynamic>(rtuId);
                    var dataappend = collection.Aggregate(pipeline).ToList();
                    if (dataappend.Count > 0)
                    {
                        data = data.Concat(dataappend).ToList();
                    }
                    beginYear_flag = beginYear_flag + 1;
                }

            }
            return data;
        }
        #endregion


        public IEnumerable<dynamic> GetWaterAns(double interval, string BeginDate, string EndDate, int DataID, bool? IsCumulation, string rtuId)
        {
            DateTime beginDate = Convert.ToDateTime(BeginDate);
            DateTime endDate = Convert.ToDateTime(EndDate);
            TimeSpan cha = endDate - beginDate;

            string group = "";
            string project = "";
            //var interval = 1000 * 60 * 60;
            if (cha.TotalHours <= 24)
            {



                group = "{$group:{'_id':{$subtract:[{$subtract:['$UpdateTime',new Date('1970-01-01')]},{$mod:[{$subtract:['$UpdateTime',new Date('1970-01-01')]}," +
                    "" + interval + "]}]},'data':{$avg:'$AnalogValues." + DataID + "'}}}";

                project = @"{$project: {'_id': 0,'data':1,'datetime': { $add: [new Date(-28800000), '$_id']}, 'group':{$dateToString:{'format':'%Y-%m-%d','date':{$add: [new Date(-28800000), '$_id']}}}}}";

            }
            if (cha.TotalHours > 24)
            {



                group = "{$group:{'_id':{$dateToString:{'format':'%Y-%m-%d','date':'$UpdateTime'}},'data':{$avg:'$AnalogValues." + DataID + "'}}}";
                project = @"{$project:{'_id':0,'data':1,'datetime':'$_id'}}";

            }

            string match = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + beginDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + endDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')} }}";
            string sort = "{'$sort': {'datetime': 1}}";
            PipelineStageDefinition<dynamic, dynamic> pmatch = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> pgroup = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> pproject = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> psort = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(pmatch);
            stages.Add(pgroup);
            stages.Add(pproject);
            stages.Add(psort);

            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            var client = new MongoClient(_conntionString);//连接 
            var database = client.GetDatabase(_dbName + beginDate.Year.ToString());
            var collection = database.GetCollection<dynamic>(rtuId);
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var data = collection.Aggregate(pipeline).ToList();
            if (beginDate.Year != endDate.Year)
            {
                var beginYear_flag = beginDate.Year + 1;
                while (beginYear_flag <= endDate.Year)
                {
                    database = client.GetDatabase(_dbName + beginYear_flag.ToString());
                    collection = database.GetCollection<dynamic>(rtuId);
                    var dataappend = collection.Aggregate(pipeline).ToList();
                    if (dataappend.Count > 0)
                    {
                        data = data.Concat(dataappend).ToList();
                    }
                    beginYear_flag = beginYear_flag + 1;
                }

            }
            return data;
        }

    }

}
