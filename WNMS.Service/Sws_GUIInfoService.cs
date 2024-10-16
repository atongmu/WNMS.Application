using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_GUIInfoService:BaseService,ISws_GUIInfoService
    {
        /// <summary>
        /// 查询工艺图数据
        /// </summary>
        /// <param name="funcWhere">查询筛选条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sortName">排序字段</param>
        /// <param name="order">排序方式正序倒序</param>
        /// <returns></returns>
        public PageResult<GUIInfo> GetGUIData(Expression<Func<GUIInfo, bool>> funcWhere, int pageSize, int pageIndex, string sortName, string order)
        {
            string sql = @"(select g.* ,d.ItemName as DeviceTypeName from Sws_GUIInfo g  left join [dbo].[Sys_DataItemDetail] d on d.F_ItemID=5 
                    and g.EquipType=d.ItemValue  where g.DeviceType=1) union (select g.* ,d.ItemName as DeviceTypeName from Sws_GUIInfo g  
                    left join [dbo].[Sys_DataItemDetail] d on d.F_ItemID=14 and g.EquipType=d.ItemValue  where g.DeviceType=2)";
            Microsoft.Data.SqlClient.SqlParameter[] sqlparameter = new Microsoft.Data.SqlClient.SqlParameter[] { };

            bool flag = true;
            if (order == "desc") flag = false;
            if (string.IsNullOrWhiteSpace(sortName)) sortName = "GUIName";

            var guiDatas = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, sortName, sql, sqlparameter, flag);
            return guiDatas;
        }

        public int DeleteGUI(int guiId)
        {
            int num = 0;
            SwsGuiinfo gui = this.Find<SwsGuiinfo>(guiId);
            List<SwsDeviceInfo01> info01 = this.Query<SwsDeviceInfo01>(r => r.Gui == guiId).ToList();
            List<SwsDeviceInfo02> info02 = this.Query<SwsDeviceInfo02>(r => r.Gui == guiId).ToList();
            if (info01.Count() > 0 || info02.Count > 0)
            {
                num = 0;
            }
            else
            {
                if (this.Delete<SwsGuiinfo>(gui))
                {
                    //删除文件夹
                    try
                    {
                        string path = "..\\WNMS.Application\\wwwroot" + gui.ImageUrl;
                        FileAttributes attr = File.GetAttributes(path);
                        if (attr == FileAttributes.Directory)
                        {
                            Directory.Delete(path, true);
                        }
                        else
                        {
                            File.Delete(path);
                        }
                    }
                    catch (Exception ex) { }
                    num = 1;
                }            
                else
                {
                    num = -1;
                }
            }
            return num;
        }
    }
}
