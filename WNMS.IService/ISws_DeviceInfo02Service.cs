using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface  ISws_DeviceInfo02Service:IBaseService
    {
        PageResult<DeviceInfo02Info> LoadInfoList(Expression<Func<DeviceInfo02Info, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true);
        IEnumerable<DeviceInfo02Info> LoadSwsDeviceInfo02Info(long EquipID);
        int DeviceImport(List<SwsDeviceInfo02> list);
        int InsertData(SwsDeviceInfo02 info);
        int DeleteDevice(List<SwsDeviceInfo02> deleteDetail);

        #region 直饮水运行日志
        IEnumerable<WNMS.Model.CustomizedClass.StationAndDevice> QueryZtreeInfo(SysUser user, string stationName);
        IEnumerable<Model.CustomizedClass.HistoryJKData> GetMongoHistoryData(string year, string collName, string beginDate, string endDate, string order, string sort, int pageIndex, int pageSize, ref int totalCount);
        IEnumerable<BsonDocument> GetCorrectData(string year, string collName, string beginDate, string endDate, string dataID);
        long UpdateCorrectData(string year, string collName, string id, string dataName, object value);
        int UpdateFocusOn(int id, bool focusOn, long userid);
        IEnumerable<HistoryJKData> GetExportHistoryData(string year, string collName, string beginDate, string endDate);
        #endregion

        #region 水质报表
        IEnumerable<WebWaterQuality> GetWaterData(string stationIds, string tablename, string beginDate, string endDate);
        #endregion

        #region 水质大屏
        public IEnumerable<Model.CustomizedClass.RtuJKInfo> GetJKWaterQuality(int rtuID);
        IEnumerable<Model.CustomizedClass.RtuJKInfo> GetALLJKWaterQuality(string rtuIDs);
        IEnumerable<WaterQualityDatas> QueryWaterQuality();
        #endregion
    }
}
