using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
   public partial interface ISws_ConsumpSettingService:IBaseService
    {
        IEnumerable<dynamic> GetDeviceTree(bool isadmin, int userid, string searchtex, short type);
        int DeviceSetting(int userid, short type, List<SwsConsumpSetting> datalist);
        IEnumerable<dynamic> QueryNH_pre(string deviceids, string tablename, DateTime begindate, DateTime enddate);
        IEnumerable<dynamic> QueryNH_manufacter(string deviceids, DateTime begindate, DateTime enddate);
        IEnumerable<dynamic> QueryNH_station(string deviceids, DateTime begindate, DateTime enddate);
        IEnumerable<dynamic> QueryAreaDevice(string deviceids);
        IEnumerable<dynamic> GetDeviceid_init(int userid, bool isadmin);
        IEnumerable<dynamic> GetMaDeviceRate(int userid, bool isadmin, string begindate, string enddate);
        IEnumerable<dynamic> GetMilliLiftNH(int userid, bool isadmin, string begindate, string enddate);
        PageResult<ManuDeviceBase> GetMaDeviceBaseTable(Expression<Func<ManuDeviceBase, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true);
        int AddInfo(SwsMaDeBaseInfo b);
        int EditeInfo(SwsMaDeBaseInfo b);
        int DeleteInfo(List<int> ids);
        #region
        PageResult<GpsData> GetPersonLocus(string num, int pageindex, int pagesize, string begindate, string enddate);
        IEnumerable<dynamic> PersonLocusTree(string text);
        PageResult<LocusUser> LocusUserList(string userName,string searchTime, int pagesize, int pageindex, string sort, bool flag);
        #endregion
        #region 曲线汇总
        IEnumerable<dynamic> GetDeviceBySetting(int userid,int type);
        IEnumerable<dynamic> GetDevice_init(int userid, bool isadmin);
        IEnumerable<dynamic> GetDevice_initFilter(int userid, bool isadmin);
        IEnumerable<dynamic> QueryDeviceTree(string searchTxt, int userid, bool isadmin);
        IEnumerable<dynamic> GetMonthNHData(string deviceids, string begindate, string enddate);
        IEnumerable<dynamic> GetThisMonthNH(string deviceids, string begindate, string enddate);
        IEnumerable<dynamic> HistoryChartData(DateTime begindate, DateTime enddate, string project, string collectName);
        IEnumerable<dynamic> GetPressFlucNum(string year, string beginDate, string endDate, string match, string group, string project, string collectName);
        IEnumerable<dynamic> GetMaxWaterData(DateTime begindate, DateTime enddate, string deviceids);
        IEnumerable<dynamic> GetPressFlucNum_today(string project, string match, string group, string collectName, DateTime begindate, DateTime enddate);
        IEnumerable<dynamic> GetMilliLiftNH_base();
        #endregion
        #region 安防事件
        IEnumerable<dynamic> GetSerialNum(int userid);
        #endregion
        #region 移动端曲线
        IEnumerable<dynamic> HistroyChartData(string project, string sort, string group, string collectName, DateTime begindate, DateTime enddate);
        #endregion
        #region 人员轨迹调整2022.8.11
        PageResult<GpsData> GetPersonLocusData(string num, int pageindex, int pagesize, string begindate, string enddate);
        IEnumerable<dynamic> PersonLocusTreeData(string text);
        #endregion
    }
}
