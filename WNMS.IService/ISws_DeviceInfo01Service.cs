using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_DeviceInfo01Service : IBaseService
    {
        PageResult<WNMS.Model.CustomizedClass.DeviceInfo01Info> LoadInfoList(Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true);
        PageResult<WNMS.Model.CustomizedClass.DeviceInfo01Info> LoadorderInfoList(Expression<Func<WNMS.Model.CustomizedClass.DeviceInfo01Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, string functwoOrderby, bool isAsc = true);

        IEnumerable<DeviceInfo01Info> LoadSwsDeviceInfo01Info(long EquipID);
        //批量导入
        int DeviceImport(List<SwsDeviceInfo01> list);
        //获取泵房树形
        IEnumerable<WNMS.Model.CustomizedClass.StationAndDevice> QueryZtreeInfo(SysUser user, string stationName);
        //获取设备历史数据
        IEnumerable<Model.CustomizedClass.HistoryJKData> GetMongoHistoryData(string year, string collName, string beginDate, string endDate, string order, string sort, int pageIndex, int pageSize, ref int totalCount);
        //获取矫正数据
        IEnumerable<BsonDocument> GetCorrectData(string year, string collName, string beginDate, string endDate, string dataID);
        //矫正
        long UpdateCorrectData(string year, string collName, string dateTime, string dataName, object value);
        //添加设备 更新泵房类型
        int DeviceInsert(SwsDeviceInfo01 swsDeviceInfo01);

        int SetAreaInfo(long DeviceID);
        //删除设备
        int DeleteDevice(List<SwsDeviceInfo01> deleteDetail, ref string deleteName);
        int DelAreaInfo(string id);
        //修改关注
        int UpdateFocusOn(int id, bool focusOn, long userid, int type);

        #region 数据监测
        IEnumerable<dynamic> GetOverTimeHiatrory(string Time, int Num, double interval, string Dev_BeginDate, int DataID, bool? IsCumulation, string rtuId);
        IEnumerable<dynamic> GetHistoryChartData(string year, string beginDate, string endDate, string group, string project, string collectName);
        #endregion

        #region 运行概况
        PageResult<Device01Data> GetSituationData(Expression<Func<Device01Data, bool>> funcWhere, string datebaseName, SysUser user, string beginDate, string endDate, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true);
        #endregion


        //导出历史数据
        IEnumerable<HistoryJKData> GetExportHistoryData(string year, string collName, string beginDate, string endDate);
        //运行分析
        IEnumerable<dynamic> QueryStationTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string sortName, string order, string userid, ref int Totalcount);
        //设备运行分析
        IEnumerable<dynamic> QueryDeviceStationTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string tableName, string sortName, string order, string userid, ref int Totalcount);
        //同比排名
        IEnumerable<dynamic> QueryPreTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string tbeginTime, string tendTime, int userid, ref int Totalcount);
        IEnumerable<dynamic> QueryDevicePreTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string tableName, string tbeginTime, string tendTime, string userid, ref int Totalcount);
        //查询能耗数量
        IEnumerable<ConsumptionChart> GetConsumptionSum(bool isadmin, int userid, string beginTime, string endTime, string type);
        IEnumerable<ConsumptionChart> GetDeviceConsumptionSumNew(bool isadmin, string userid, string beginTime, string endTime, string tableName);
        //查询能耗统计新 
        IEnumerable<ConsumptionChart> GetConsumptionSumNew(bool isadmin, int userid, string beginTime, string endTime, string type);
        #region 模板左侧泵房树形
        IEnumerable<dynamic> GetStationTree(int userid, bool isadmin, string stationName);
        IEnumerable<dynamic> GetStationDevice(int stationid, int tempid, string tablename, int f_itemid, string deviceids);
        //模板泵房列表
        IEnumerable<dynamic> GetStationInfo(int tempid, string deviceids);
        #endregion
        //查询阀门信息
        IEnumerable<WNMS.Model.CustomizedClass.FmInfo> QueryFaInfo(int sid, int type);
        IEnumerable<WNMS.Model.CustomizedClass.FmInfo> QueryZFaInfo(int sid);
        //全览报警设备
        IEnumerable<dynamic> QueryEventDevice();
        //设备增长
        IEnumerable<dynamic> loadDeviceUp(SysUser user, DateTime? startTime, DateTime? endTime);

        IEnumerable<dynamic> GetOverTimeHiatroryAll(string Time, int Num, double interval, string Dev_BeginDate, int DataID, bool? IsCumulation, string rtuId, string[] sstr);

        #region K线图
        IEnumerable<dynamic> GetDeviceJKData(DateTime beginDate, DateTime endDate, int DataID, string rtuId);
        #endregion

        IEnumerable<dynamic> GetWaterAns(double interval, string BeginDate, string EndDate, int DataID, bool? IsCumulation, string rtuId);
    }
}
