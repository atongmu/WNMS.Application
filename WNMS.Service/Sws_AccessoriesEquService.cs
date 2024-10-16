using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_AccessoriesEquService:BaseService,ISws_AccessoriesEquService
    {
        public IEnumerable<dynamic> QueryAccessoryEquTable(bool IsAdmin,int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAcessoryEquTable @IsAdmin,@UserID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }
        public IEnumerable<dynamic> GetAccessoryTable(String conponentid, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@conponentid",conponentid),
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
                 };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAccessoryTable @conponentid,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        public IEnumerable<dynamic> GetAcce_DeviceTable( bool IsAdmin,int UserID, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
                 };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAcc_DeviceTable @IsAdmin,@UserID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }
        //添加
        public int AddAccessoryEqu(SwsAccessoriesEqu ae, SwsAccessories a)
        {
            try
            {
               
                a.Inventory = a.Inventory - ae.Quantity;
                this.Context.Set<SwsAccessories>().Update(a);
                this.Context.Set<SwsAccessoriesEqu>().AddRange(ae);
                return this.Context.SaveChanges();
            }
            catch (Exception e) {
                var aa = e;
                return this.Context.SaveChanges();
            }
            
        }
        //修改
        public int EditeAccessoryEqu(SwsAccessoriesEqu ae_new, SwsAccessories a,int ae_oldQuantity)
        {
            this.Context.Set<SwsAccessoriesEqu>().Update(ae_new);
            if (ae_new.Quantity != ae_oldQuantity)
            {
                var accessory_Quantity = a.Inventory + ae_oldQuantity - ae_new.Quantity;
                a.Inventory = accessory_Quantity;
                this.Context.Set<SwsAccessories>().Update(a);
            }
            return this.Context.SaveChanges();
        }
        public int DeleteAccessoryEqu(List<long> A_Equids)
        {
            var A_Equlist = this.Context.Set<SwsAccessoriesEqu>().Where(r => A_Equids.Contains(r.Id)).ToList();
            this.Context.Set<SwsAccessoriesEqu>().RemoveRange(A_Equlist);
            //删除保养记录
            var Maintenances = this.Context.Set<SwsAccessoriesMaintenance>().Where(r => A_Equids.Contains(r.AccessoriesDetailId)).ToList();
            this.Context.Set<SwsAccessoriesMaintenance>().RemoveRange(Maintenances);
            return this.Context.SaveChanges();
        }
        #region 器件地图
        public IEnumerable<dynamic> GetStationByAccessory(bool IsAdmin, int UserID,string searchText)
        {
            string sql = "";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@searchText","%"+ searchText + "%")
                 };
            if (IsAdmin == true)
            {
                sql = @" with t as (select DeviceID,StationID,1 as EquType  from [dbo].[Sws_DeviceInfo01]
  union  select DeviceID,StationID,2 as EquType  from [dbo].[Sws_DeviceInfo02]),
  s1 as(
  select StationID,case when m.AccessoriesID is not null then 1 else 0 end state from t
  left join [dbo].[Sws_AccessoriesEqu] aq on t.DeviceID=aq.EquipmentID and t.EquType=aq.EquType
  left join [dbo].[Sws_Accessories] aa on aa.ID=aq.AccessoriesID
  left join [dbo].[Sws_AccessoriesRtMaintenance] m on aq.ID=m.AccessoriesID
  where aq.ID is not null and EndDate is null and aa.ID is not null),
  s as(
  select StationID,sum(state) as state from s1  group by StationID)
  select st.StationID,StationName,Lng,Lat,state from s
  left join [dbo].[Sws_Station] st on s.StationID=st.StationID
  where st.StationID is not null";
            }
            else
            {
                sql = @"with t as (select DeviceID,StationID,1 as EquType  from [dbo].[Sws_DeviceInfo01]
  union  select DeviceID,StationID,2 as EquType  from [dbo].[Sws_DeviceInfo02]),
  s1 as(
  select t.StationID,case when m.AccessoriesID is not null then 1 else 0 end state from t
  left join [dbo].[Sws_AccessoriesEqu] aq on t.DeviceID=aq.EquipmentID and t.EquType=aq.EquType
  left join [dbo].[Sws_UserStation] u on t.StationID=u.StationID
  left join [dbo].[Sws_Accessories] aa on aa.ID=aq.AccessoriesID
  left join [dbo].[Sws_AccessoriesRtMaintenance] m on aq.ID=m.AccessoriesID
  where aq.ID is not null and u.UserID=@UserID and EndDate is null and aa.ID is not null),
  s as(
  select StationID,sum(state) as state from s1  group by StationID)
  select st.StationID,StationName,Lng,Lat,state from s
  left join [dbo].[Sws_Station] st on s.StationID=st.StationID
  where st.StationID is not null";
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                sql += "  and StationName like @searchText";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        public IEnumerable<dynamic> GetAccessBySId(int stationid,int f_itemid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@f_itemid",f_itemid) 
                 };
  //          string sql = @"   with t as (select DeviceName,StationID,DeviceID, 1 as EquType,Partition  from [dbo].[Sws_DeviceInfo01] where StationID=@stationid
  //union select DeviceName,StationID,DeviceID, 2 as EquType,Partition  from [dbo].[Sws_DeviceInfo02] where StationID=@stationid)
  //select Name,AccessoriesID,quantity,AccessoriesNo,ItemName,Lat,Lng from [dbo].[Sws_AccessoriesEqu] aq
  //left join t on aq.EquipmentID=t.DeviceID and aq.EquType=t.EquType
  //left join [dbo].[Sws_Accessories] a on aq.AccessoriesID=a.ID
  //left join [dbo].[Sws_Station] s on t.StationID=s.StationID
  //left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) d on t.Partition=d.ItemValue
  //where t.StationID is not null and EndDate is null";
            string sql = @"    with t as (select DeviceName,StationID,DeviceID, 1 as EquType,Partition  from [dbo].[Sws_DeviceInfo01]  where StationID=@stationid
  union select DeviceName,StationID,DeviceID, 2 as EquType,Partition  from [dbo].[Sws_DeviceInfo02]  where StationID=@stationid),
  r as(
   select  AccessoriesID,quantity,AccessoriesNo,StationID,EquipmentID,ItemName from [dbo].[Sws_AccessoriesEqu] aq
    left join t on aq.EquipmentID=t.DeviceID and aq.EquType=t.EquType
	left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) d on t.Partition=d.ItemValue
    left join [dbo].[Sws_Accessories] aa on aq.AccessoriesID=aa.ID
    where t.StationID is not null and EndDate is null and aa.ID is not null),
	g as(
	select AccessoriesID,sum(quantity) as quantity,AccessoriesNo,StationID,EquipmentID,ItemName from r  group by 
	AccessoriesID,AccessoriesNo,StationID,EquipmentID,ItemName)
	select g.*,Name,Lat,Lng,Type from g
  left join [dbo].[Sws_Accessories] a on g.AccessoriesID=a.ID
  left join [dbo].[Sws_Station] s on g.StationID=s.StationID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
    }

}
