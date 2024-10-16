using Model;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_EventInfoService : IBaseService
    {
        IEnumerable<AlarmDeviceInfo> LoadCurrentEventInfo();
        #region 报警监控
        IEnumerable<dynamic> QueryEventInfo_JK(int pageindex, int pagesize, string filter, string countfilter, int userID, bool IsAdmin, int f_itemID, int f_itemIDStationType, ref int Totalcount, ref int AttendNum, ref int ValueAlarmNum, ref int CommiNum, ref int DataNum);
        IEnumerable<dynamic> QueryEventHandleInfo(DateTime eventtime, int rtuid, int eventsouce);
        IEnumerable<dynamic> GetAlarmList(int userid, bool isadmin, string filter, int pageindex, int pagesize, int itemid_partion, ref int TotalCount);
        int OperateReadAlarm(string eventid);
        int GetAlarmCount(int userid, bool isadmin);
        int OperateReadAlarm_All(int userid, bool isadmin);
        #endregion
        /// <summary>
        /// 工艺图获取实时报警信息
        /// </summary>
        /// <param name="rtuids"></param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadEventList(string rtuids);
        /// <summary>
        /// 查询报警泵房信息
        /// </summary>
        /// <param name="rtuIDs">设备ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetPumData(List<int> rtuIDs);
        /// <summary>
        /// 查询泵房报警
        /// </summary>
        /// <param name="rtuids">设备ID</param>
        /// <returns></returns>
        IEnumerable<dynamic> LoadEventsInfo(string rtuids);
        #region 泵房监控 报警查询 自动生成工单
        IEnumerable<dynamic> QueryEventByRtuID(string rtuid, int intype);
        int CreatOrder(int evid, int rtuid, int userid, string message, byte eventLevel, ref long woid);
        #endregion
        int HandlerEventInfo(int? evid,SwsEventHandle e, string alarmMessage, byte? eventLevel);
        int DeleteEvents(int eventId, DateTime eventTime, short eventSource, int rtuId);
    }
}
