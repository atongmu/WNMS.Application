using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISws_GPSModuleService : IBaseService
    {
        int CreateGPSTable(SwsGpsmodule sgp, double lng, double lat);
        IEnumerable<UserGPS> GetGpsUser(string sid);
        List<UserPosition> GetGpsDevices(string SerialNumber);
        //手机gps定位
        List<UserPositionPhone> GetPhoneGpsDevices(long userid);
        string GetUserName(string SerialNumber, string UpdateTime);
        //手机gps定位
        string GetUserNamePhone(long userid, string UpdateTime);
        #region 移动端实时位置更新
        int UpdatePosition(long userid, double lng, double lat);
        #endregion
    }
}
