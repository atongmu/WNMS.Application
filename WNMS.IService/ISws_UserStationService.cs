using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;

namespace WNMS.IService
{
    public partial interface ISws_UserStationService : IBaseService
    {
         IEnumerable<UserStationInfo> LoadUserStationInfo(int UserID);
         IEnumerable<UserStationAndDeviceInfo> LoadUserStationAndDeviceInfo(int UserID);
        IEnumerable<dynamic> LoadStationAndCameraInfo(int UserID);
    }
}
