using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;
using Microsoft.Data.SqlClient;
using System.Linq;
using MongoDBHelper;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace WNMS.Service
{
    public partial class Sws_DeviceInfo02Service : BaseService, ISws_DeviceInfo02Service
    {
        #region 直饮水设备管理
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="funcWhere">查询条件</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="funcOrderby">排序列</param>
        /// <param name="isAsc">正序 倒序</param>
        /// <returns></returns>
        public PageResult<DeviceInfo02Info> LoadInfoList(Expression<Func<DeviceInfo02Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true)
        {
            SysUser user = this.Find<SysUser>(userID);

            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userID",userID)
            };
            string sql = @"SELECT   d.DeviceID, d.DeviceName, d.DeviceNum, d.Partition, d.StationID, d.DeviceType, d.Manufacturer,
                    d.ProductionDate,s.StationName,sd.ItemName as DeviceTypeName,d.ImageUrl,d.Gui,r.DeviceID as RtuNum,
                    sd2.ItemName as ManufacturerName,sd3.ItemName as  PartitionName
                    FROM sws_DeviceInfo02 d  
                    left join Sws_Station s on d.StationID = s.StationID
                    left join Sws_RTUInfo r on d.RTUID=r.RTUID
                    left join Sys_DataItemDetail sd on  d.DeviceType = sd.ItemValue and sd.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =14 and IsEnable=1)
                           
                    left join Sys_DataItemDetail sd2 on  d.Manufacturer = sd2.ItemValue and sd2.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =7 and IsEnable=1)

                    left join Sys_DataItemDetail sd3 on  d.Partition = sd3.ItemValue and sd3.F_ItemId in 
                    (select F_ItemId from Sys_DataItemDetail  where [F_ItemId] =8 and IsEnable=1)";

            if (!user.IsAdmin)     //非admin查询
            {
                sql += " where d.StationID in (select StationID from Sws_UserStation where UserID=@userID)";
            }

            PageResult<DeviceInfo02Info> presult = new PageResult<DeviceInfo02Info>();
            if (ispage)   //分页查询
            {
                presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            }
            else     //查询所有
            {
                List<DeviceInfo02Info> list = this.Context.Database.SqlQuery<DeviceInfo02Info>(sql, sqlparameter);
                presult.DataList = list;
            }
            return presult;
        }

        public IEnumerable<DeviceInfo02Info> LoadSwsDeviceInfo02Info(long EquipID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID)
             };

            var query = this.Context.Database.SqlQuery<DeviceInfo02Info>("exec GetSwsDeviceInfo02Info @EquipID", sp);
            return query;
        }

        //添加设备
        public int InsertData(SwsDeviceInfo02 info)
        {
            SwsStation station = this.Query<SwsStation>(r => r.StationId == info.StationId).FirstOrDefault();
            if (station != null && station.InType == 0)
            {
                station.InType = 2;
                this.Context.Attach(station);
                this.Context.Update<SwsStation>(station);
            }
            this.Context.Attach(info);
            this.Context.Add<SwsDeviceInfo02>(info);
            return this.Context.SaveChanges();
        }

        //删除设备
        public int DeleteDevice(List<SwsDeviceInfo02> deleteDetail)
        {
            foreach (var item in deleteDetail)
            {
                this.Context.Attach(item);
                var stationid = item.StationId;
                //查询设备表中 该泵站是否还存在
                var userstationlist = this.Context.Set<SwsDeviceInfo02>().Where(r => r.StationId == stationid && r.DeviceId != item.DeviceId).ToList();
                if (userstationlist.Count == 0)
                {
                    var stationinfo = this.Context.Find<SwsStation>(stationid);
                    if (stationinfo != null)
                    {
                        stationinfo.InType = 0;
                        this.Context.Attach(stationinfo);
                        this.Context.Update<SwsStation>(stationinfo);
                    }
                }

                //查询表中通讯id是否还存在
                var rtulist = this.Query<SwsDeviceInfo02>(r => r.Rtuid == item.Rtuid && r.DeviceId != item.DeviceId).AsNoTracking().ToList();
                if (rtulist.Count() == 0)
                {
                    SwsRtuinfo rtu = this.Find<SwsRtuinfo>(item.Rtuid);
                    if (rtu != null)
                    {
                        rtu.StationId = 0;
                        this.Context.Attach(rtu);
                        this.Context.Update<SwsRtuinfo>(rtu);
                    }
                }

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
                catch (Exception ex) { }

            }
            this.Context.Set<SwsDeviceInfo02>().RemoveRange(deleteDetail);
            return this.Context.SaveChanges();
        }

        //批量导入
        public int DeviceImport(List<SwsDeviceInfo02> list)
        {
            try
            {
                foreach (var item in list)
                {
                    var stationfo = this.Context.Set<SwsStation>().Where(r => r.StationId == item.StationId).FirstOrDefault();
                    if (stationfo != null && stationfo.InType == 0)
                    {
                        stationfo.InType = 2;
                        this.Context.Update<SwsStation>(stationfo);
                    }
                    this.Context.Add<SwsDeviceInfo02>(item);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        #endregion


        #region 直饮水运行日志
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
                new SqlParameter("@stationName","%" + stationName + "%"),
                new SqlParameter("@userID",userID)
            };
            if (user.IsAdmin)
            {
                sql = @"select d.DeviceId,d.DeviceName,s.StationId,s.StationName from Sws_DeviceInfo02 d left join Sws_Station s on d.StationID=s.StationID where s.StationId>0";
            }
            else
            {
                sql = @"select d.DeviceId,d.DeviceName,s.StationId,s.StationName from Sws_DeviceInfo02 d left join Sws_Station s on d.StationID=s.StationID
                        where d.StationID in (select StationID from Sws_UserStation where UserID=@userID) and s.StationId>0";
            }

            if (!string.IsNullOrEmpty(stationName))
            {
                sql = sql + @" and s.StationName like @stationName";
            }
            sql = sql + @" order by d.Partition";
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
        public int UpdateFocusOn(int id, bool focusOn, long userid)
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
                var tempInfoList = this.Context.Set<SwsTemplate>().Where(t => t.FocusOn == true && t.UserId == userid).ToList();
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
        #endregion

        #region 水质报表///CHARINDEX未测，由in转化成CHARINDEX
        public IEnumerable<WebWaterQuality> GetWaterData(string stationIds, string tablename, string beginDate, string endDate)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationIds",stationIds),
                new SqlParameter("@beginDate",beginDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@tablename",tablename)
        };
            string sql = @" with t as(SELECT StationID, UpdateTime, DataKey, DataValue FROM [dbo].[" + @tablename + "] where   CHARINDEX(','+LTRIM(StationID)+',',','+@stationIds+',')>0  and UpdateTime>=@beginDate " +
                @"and UpdateTime<@endDate and DataKey in ('PH_Aver','CL_Aver','Turbidity_Aver','ORP_Aver','Salinity_Aver','Oxygen_Aver','Conductivity_Aver'))," +
                @"r as(select StationID,UpdateTime,PH_Aver,CL_Aver,Turbidity_Aver,ORP_Aver,Salinity_Aver,Oxygen_Aver,Conductivity_Aver  from t PIVOT(max(DataValue) FOR DataKey 
                IN(PH_Aver,CL_Aver,Turbidity_Aver,ORP_Aver,Salinity_Aver,Oxygen_Aver,Conductivity_Aver)) as APVT)  select StationID as StationId, UpdateTime,PH_Aver as PHAver,CL_Aver as CLAver,Turbidity_Aver as TurbidityAver,ORP_Aver as ORPAver,
                Salinity_Aver as SalinityAver,Oxygen_Aver as OxygenAver,Conductivity_Aver as ConductivityAver from r  order by UpdateTime";
            var query = this.Context.Database.SqlQuery<WebWaterQuality>(sql, sp).ToList();
            return query;
        }
        #endregion

        #region 水质大屏
        /// <summary>
        /// 根据rtuID查询单个设备的
        /// </summary>
        /// <param name="rtuID">通讯ID</param>
        /// <returns></returns>
        public IEnumerable<Model.CustomizedClass.RtuJKInfo> GetJKWaterQuality(int rtuID)
        {
            MongoDBHelper<Model.CustomizedClass.RtuJKInfo> mongoHelper = new MongoDBHelper<Model.CustomizedClass.RtuJKInfo>(DateTime.Now.Year.ToString());

            //查询条件
            FilterDefinition<Model.CustomizedClass.RtuJKInfo> filter = "{ 'RTUID' : { '$eq' : " + rtuID + "} }";

            //数据查询
            var list = mongoHelper.Find("Sws_DeviceJKInfo", filter, "UpdateTime");
            return list;
        }

        /// <summary>
        /// 查询所有的直饮水设备的实时数据
        /// </summary>
        /// <param name="rtuIDs">直饮水设备RtuID拼接的字符串</param>
        /// <returns></returns>
        public IEnumerable<Model.CustomizedClass.RtuJKInfo> GetALLJKWaterQuality(string rtuIDs)
        {
            MongoDBHelper<Model.CustomizedClass.RtuJKInfo> mongoHelper = new MongoDBHelper<Model.CustomizedClass.RtuJKInfo>(DateTime.Now.Year.ToString());

            //查询条件
            FilterDefinition<Model.CustomizedClass.RtuJKInfo> filter = "{ 'RTUID' : { '$in' : [" + rtuIDs + "]} }";

            //数据查询
            var list = mongoHelper.Find("Sws_DeviceJKInfo", filter, "UpdateTime");
            return list;
        }

        /// <summary>
        /// 获取水质对比曲线
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WaterQualityDatas> QueryWaterQuality()
        {
            string time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@time",time)
            };

            string sql = @"select d.DeviceName,s.PH_Aver as PHAver,s.CL_Aver as CLAver,s.Turbidity_Aver as TurbidityAver from Sws_DeviceInfo02 d left join DDayQuartZ02 s on d.DeviceID=s.DeviceID
                        where s.UpdateTime=@time";

            var query = this.Context.Database.SqlQuery<WaterQualityDatas>(sql, sp);
            return query;
        }
        #endregion
    }
}
