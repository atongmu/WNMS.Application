using Model;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;

namespace WNMS.IService
{
    public partial interface ISws_EventHistoryService : IBaseService
    {
        IEnumerable<AlarmDeviceInfo> LoadEventHistoryInfo(long EquipID);
        IEnumerable<dynamic> LoadEventHistoryInfoByPage(long EquipID, int startindex, int pageSize, ref int Totalcount);
        IEnumerable<EventDateCounts> LoadEventDateCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate);
        IEnumerable<EventMonthCounts> LoadEventMonthCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate);
        IEnumerable<EventYearCounts> LoadEventYearCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate);
        IEnumerable<EventLevelCounts> LoadEventLevelCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate);

        #region 报警历史数据
        IEnumerable<dynamic> QueryEventTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string sortName, string order, int userid, ref int Totalcount);
        #endregion
        #region 报警分析
        IEnumerable<EventRanking> GetRankingData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, ref int totalCount);
        IEnumerable<Eventmau> GetPinPaiData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int itemid);
        IEnumerable<Eventmau> GetDeviceTypeData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int itemid);
        IEnumerable<EventStation> GetEventTableData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, ref int totalCount);
        IEnumerable<EventStation> GetEventDetailData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex);
        IEnumerable<EventStation> GetDetailData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, int detailType, string content, ref int totalCount);
        /// <summary>
        /// 查询泵房报警排名
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rtuIDs"></param>
        /// <returns></returns>
        IEnumerable<StationEventRank> LoadStationRank(DateTime beginDate, DateTime endDate, string rtuIDs);
        #endregion
    }
}
