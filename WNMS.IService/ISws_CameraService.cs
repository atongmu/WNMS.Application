using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface  ISws_CameraService:IBaseService
    {
        PageResult<CameraInfo> LoadInfoList(Expression<Func<CameraInfo, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true);
        //添加摄像头
        int AddCameraEntity(SwsCamera camera);
        int EditCameraEntity(SwsCamera cameram,int id);
        int EditCameraEntitys(List<SwsCamera> cameralist, int id);
        int DeleteCameraEntity(List<int> ids);
        #region 视频接入
        IEnumerable<CameraInfo> LoadCameraInfo(string condition, int userID);
        #endregion

        #region 获取视频列表
        IEnumerable<dynamic> GetCameraStationData(bool isadmin, int userid, string name, int innertype);
        #endregion
    }
}
