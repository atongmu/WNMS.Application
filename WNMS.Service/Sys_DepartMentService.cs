using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;
namespace WNMS.Service
{
    public partial class Sys_DepartMentService : BaseService, ISys_DepartMentService
    {
        
        public IEnumerable<dynamic> QueryDepartmentTable(string dptname,int departEnum)
        {
            string sql = "";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@dptname","%"+dptname+"%"),
                new SqlParameter("@departEnum",departEnum)
            };
            if (dptname != "")
            {
                //              sql = @" select DepartmentID,0 as PID, DptName, Type, Manager, OuterPhone, InnerPhone, Email, Fax, d.Sort, Reamrk, ItemName from [dbo].[Sys_DepartMent] d 
                //left join [dbo].[Sys_DataItemDetail] t on d.Type=t.ItemValue
                //where F_ItemId=@departEnum and DptName like '%" + @dptname + "%'";
                sql = @" select DepartmentID,0 as PID, DptName, Type, Manager, OuterPhone, InnerPhone, Email, Fax, d.Sort, Reamrk, ItemName from [dbo].[Sys_DepartMent] d 
  left join (select * from  [dbo].[Sys_DataItemDetail] where  F_ItemId=@departEnum and  IsEnable=1 ) t on d.Type=t.ItemValue
  where  DptName like @dptname";
            }
            else {
                sql = @" select DepartmentID, PID, DptName, Type, Manager, OuterPhone, InnerPhone, Email, Fax, d.Sort, Reamrk, ItemName from [dbo].[Sys_DepartMent] d 
  left join(select * from  [dbo].[Sys_DataItemDetail] where  F_ItemId=@departEnum and  IsEnable=1)t on d.Type=t.ItemValue ";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        /// <summary>
        /// 获取子集和本身以外的数据集合
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public IEnumerable<SysDepartMent> QueyExtentSelfAndChirldren_Depart(int DepartmentID)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new  SqlParameter("@DepartmentID",DepartmentID)
            };
            string sql = @"with t as
    (
    select * from Sys_DepartMent where DepartmentID=@DepartmentID
    union all
    select d.* from Sys_DepartMent d join t as d2 on d.PID=d2.DepartmentID
    )
	 select d.* from Sys_DepartMent d
	 left join t on d.DepartmentID=t.DepartmentID where t.DepartmentID is null";
            var query = base.ExcuteQuery<SysDepartMent>(sql, sp);

            return query;
        }
        /// <summary>
        /// 获取本身以及子集数据集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<SysDepartMent> QuerySelfAndChirldren_Depart(string ids)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@ids",ids)
            };
            string sql = @"
	 with t as
    (
    select * from Sys_DepartMent where  CHARINDEX(','+LTRIM(DepartmentID)+',', ','+@ids+',')>0 "+
   @" union all
    select d.* from Sys_DepartMent d join t as d2 on d.PID=d2.DepartmentID
    )
	 select d.* from Sys_DepartMent d
	 left join t on d.DepartmentID=t.DepartmentID where t.DepartmentID is not null";
            var query = this.Context.Database.SqlQuery<SysDepartMent>(sql, sp);

            return query;

        }
    }
}
