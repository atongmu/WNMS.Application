using Model;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_RTUInfoService : IBaseService
    {
        IEnumerable<DeviceAndRtuinfo> LoadDeviceAndRtuinfo(int StationID);
        IEnumerable<AlarmDeviceInfo> LoadAlarmDeviceInfo(int StationID);
        DeviceJK GetMongoJKData(int RTUID);
        DeviceJK GetMongoHistoryJKData(string year, string beginDate, string endDate, string collectName);
        List<DeviceJK> GetMoreMongoHistoryJKData(string year, string beginDate, string endDate, string collectName);
        IEnumerable<FLowReport> GetFlowReportData(string year, string beginDate, string endDate, string group, string project, string collectName);
        #region 通讯接入
        int DeleteDevHistoryInfo(string begindate, string enddate, int rtuid);
        IEnumerable<dynamic> GetSwsRtuInfoTable(int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount);
        IEnumerable<dynamic> GetRtu_StationTree(int stationid, string stationName);
        IEnumerable<dynamic> GetRtu_StationDevice(int stationid, int rtuid, string tablename, int f_itemid, string deviceids);
        int AddRtuInfo(SwsRtuinfo r, List<long> Deviceids);
        int EditeRtuInfo(SwsRtuinfo r, List<long> Deviceids);
        IEnumerable<dynamic> GetDeviceByRtuId(int rtuid, string tablename, int f_itemid);
        int DeleteRtuInfo(List<int> rtuids);
        #endregion
        #region 大屏能耗分析
        IEnumerable<dynamic> GetDeviceTree_Big(string stationName, int userid, bool isadmin);
        IEnumerable<dynamic> GetDeviceTree_BigFilter(string stationName, int userid, bool isadmin);
        IEnumerable<DmonthQuartZ01> GetTodayCompData(long DeviceID, string begindate, string enddate);
        IEnumerable<DdayQuartZ01> GetThisDayCompData(string DeviceID, string begindate, string enddate);
        IEnumerable<DetailFlowData> GetDetailFlowData(string year, string beginDate, string endDate, string group, string project, string collectName);
        #endregion
        #region GPS设备信息
        /// <summary>
        /// 查询GPS设备借还信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetGpsborrowingTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        /// <summary>
        /// 查询分区客服人员
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> LoadKF();
        //删除信息
        int DeleteGpsborrowing(List<int> ids);

        #endregion
    }
}
