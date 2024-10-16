using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_StationService : IBaseService
    {
        IEnumerable<dynamic> QueryStationTable(bool IsAdmin, int pageindex, int pagesize, int F_itemid, int f_StationInstall, string order, string filter, ref int Totalcount);
        IEnumerable<dynamic> LoadStationInfo(int StationID);
        int AddStation(SwsStation ss, List<Attachment> attachlist, bool isadmin, int userid);
        int EditeStation(SwsStation ss, List<Attachment> attachlist, int classify);
        int DeleteStation(string RootPath, List<int> stationIDs, int classify, ref string useStationName, ref string deleteStationName);
        int AddStationList(List<SwsStation> slist, bool isadmin, int userid);

        #region 泵房分布
        IEnumerable<dynamic> GetMapStationInfo(string stationName, int stationtype, int userID);
        IEnumerable<StationType> GetStationType(int userID);
        IEnumerable<dynamic> GetStationByID(int stationID);
        IEnumerable<DeviceInfo01Info> GetDevice01ByID(int stationID);
        IEnumerable<DeviceInfo02Info> GetDevice02ByID(int stationID);
        IEnumerable<dynamic> GetPropertyInfoByID(int stationID);
        #endregion

        #region 泵房监控(供水泵房)
        IEnumerable<dynamic> GetAlarmStion(bool isadmin, int userID, int stationtype);
        IEnumerable<dynamic> GetAlarmStion_New(bool isadmin, int userID, int stationtype, string onlineRtu);
        IEnumerable<dynamic> GetAlarmZStion_New(bool isadmin, int userID, int stationtype, string onlineRtu);
        IEnumerable<dynamic> GetRtuIDByUserID(int userid, int stationtype);
        IEnumerable<dynamic> GetSationNumMany(string alarmSationids, string onlineRtuid, string filter, int userid, bool isadmin, int stationtype);
        IEnumerable<dynamic> GetStaionDataByPage(int stationtype, string type, string onlineRtuid, string alarmSids, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount);
        IEnumerable<dynamic> GetStaionDataByPage_simple(int stationtype, string type, string onlineRtuid, string alarmSids, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount);
        IEnumerable<Eventinfo> GetEventinfos(int devicetype, List<int> rtuids);
        #endregion

        #region 泵房监控（直饮水泵房）
       
        
        
        IEnumerable<dynamic> GetAlarmZStion(bool isadmin, int userID, int stationtype);
        IEnumerable<dynamic> GetZRtuIDByUserID(int userid, int stationtype);
        #endregion
        #region 泵房监控 表变量
        IEnumerable<dynamic> GetSationNumMany_tvp(DataTable tvpDt, string filter, int userid, bool isadmin, int stationtype);
        IEnumerable<dynamic> GetSationNumMany_StationJK(DataTable tvpDt, string filter, int userid, bool isadmin, int stationtype);
        IEnumerable<dynamic> GetStaionDataByPage_tvp(DataTable tvpDt, int stationtype, string type, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount);
        IEnumerable<dynamic> GetStaionDataByPage_simple_tvp(DataTable tvpDt, int stationtype, string type, string filter, int pageIndex, int pageSize, bool IsAdmin, long UserID, ref int totalcount);
        #endregion

        #region 数据监测
        IEnumerable<StationData> GetStationInfo(int userID, string stationName);
        #endregion
        #region 单吨能耗
        IEnumerable<dynamic> GetStation_consumption(bool isadmin, int userid, string stationName, int innertype);
        IEnumerable<dynamic> GetConsumpBy_Day(string begindate, string enddate, int stationID, string tablename);
        IEnumerable<dynamic> GetConsumpBy_ThisMonth(string begindate, string enddate, int stationID);
        IEnumerable<dynamic> GetConsumpBy_MY(string begindate, string enddate, int stationID, string tablename);
        #region 以设备为单位
        IEnumerable<dynamic> GetConDataByDeviceID(string begindate, string enddate, string deviceids, string tablename);
        IEnumerable<dynamic> GetDeviceNameOfCon(int stationid);
        IEnumerable<dynamic> GetConDataByDeviceID_thisMonth(string begindate, string enddate, string deviceids);
        #endregion
        #endregion
        #region 夜间流量
        IEnumerable<dynamic> GetNightFlow_Day(int innertype, string stationids, int pageindex, int pagesize, string pastbegin, string pastend, string thisbegin, string thisend, string dates, ref int totalcount);
        IEnumerable<dynamic> ExportNightFlow_Day(int innertype, string stationids, string pastbegin, string pastend, string thisbegin, string thisend, string dates);
        IEnumerable<dynamic> GetNightFlow_Month(int innertype, string stationids, int pageindex, int pagesize, string time1, string time2, string time3, int minhour, int maxhour, string dates, ref int totalcount);
        IEnumerable<dynamic> GetNightFlow_Year(int innertype, string stationids, int pageindex, int pagesize, string time1, string time2, string time3, int minhour, int maxhour, string dates, ref int totalcount);
        IEnumerable<dynamic> ExportNightFlow_Month(int innertype, string stationids, string time1, string time2, string time3, int minhour, int maxhour, string dates);
        IEnumerable<dynamic> ExportNightFlow_Year(int innertype, string stationids, string time1, string time2, string time3, int minhour, int maxhour, string dates);

        #endregion
        #region 直饮水数据监测
        IEnumerable<dynamic> GetZDataMonitoringTree(int F_ItemId, int userid, bool isadmin, string searchtext);
        #endregion
        #region 地图监测数据 泵房数量
        IEnumerable<dynamic> GetSationNumMap(string alarmSationids, string onlineRtuid, int userid, bool isadmin);
        //报警泵房ID
        public IEnumerable<dynamic> GetAlarmStionMap(bool isadmin, int userID);
        //根据用户id来查询rtuid
        IEnumerable<dynamic> GetRtuIDByUserIDMap(int userid);
        //查询机组数量 
        IEnumerable<dynamic> GetDeviceNumMap(bool isadmin, int userID);
        //查询泵房监控数据地图
        IEnumerable<dynamic> GetStaionDataMap(string type, string onlineRtuid, string alarmSids, bool IsAdmin, long UserID,string stationName);
        //查询泵房下的RTUID
        IEnumerable<dynamic> GetStationRtuid(int id);
        //查询泵房设备信息
        IEnumerable<dynamic> GetStaionById(string type, int stationId);
        //新数据查询方法
        IEnumerable<dynamic> GetStaionDataByMap_tvp(DataTable tvpDt, int stationtype, string type, string filter, bool IsAdmin, long UserID);
        #endregion

        #region 直饮水泵房数据
        //获取直饮水泵房以及设备数据地图
        IEnumerable<dynamic> GetZStaionDataMap(string type, string onlineRtuid, string alarmSids, bool IsAdmin, long UserID, string stationName);
        //查询泵房设备信息
        IEnumerable<dynamic> GetZStaionById(string type, int stationId);
        #endregion
        #region 查询二供泵房和直饮水泵房的报警
        //查询二供泵房和直饮水泵房的报警
        IEnumerable<dynamic> GetAllAlarmStionMap(bool isadmin, int userID);
        #endregion

        #region
        //获取泵房以及设备数据地图 不区分直饮水 二供
        IEnumerable<dynamic> GetStaionAllDataMap(string onlineRtuid, string alarmSids, bool IsAdmin, long UserID, string stationName);
        #endregion
        #region 泵房接入
        IEnumerable<dynamic> QueryStationAccessTable(int pageindex, int pagesize, int F_itemid, string order, string filter, ref int Totalcount);
        IEnumerable<dynamic> GetAccessByStationid(int pageindex, int pagesize, int stationid, int brand_itemid, string search, ref int Totalcount);
        int SetStationAccess_Door(string doorids, int stationid);
        IEnumerable<dynamic> GetDeviceByStationID(int stationid, string tablename);
        int CancleRtu_Access(string Deviceid, int Intype);
        int SetDeviceRtu_Access(int rtuid, string deviceID, int intype);
        #endregion

        #region 查询泵房工艺图地址
        IEnumerable<StationGUI> GetGUIImg(int sid);
        //获取某泵房的分区和通讯id
        IEnumerable<dynamic> GetStationOfRtu(int sttaionid);
        #endregion

        #region 实时追踪
        IEnumerable<GpsData> GetGPSMarkerData(string userid);
        IEnumerable<GpsData> GetGPSData(string userid, string beginDate, string endDate);
        #endregion
    }

}
