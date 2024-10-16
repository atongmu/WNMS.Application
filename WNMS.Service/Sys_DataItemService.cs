using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Model.DataModels;
using Microsoft.Data.SqlClient;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class Sys_DataItemService : BaseService, ISys_DataItemService
    {
        /// <summary>
        /// 获取子集和本身以外的数据集合
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        public IEnumerable<SysDataItem> QueyExtentSelfAndChirldren(int itemid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@itemid",itemid)
            };
            string sql = @"with t as
    (
    select * from Sys_DataItem where ItemID=@itemid
    union all
    select d.* from Sys_DataItem d join t as d2 on d.PID=d2.ItemID
    )
	 select d.* from Sys_DataItem d
	 left join t on d.ItemID=t.ItemID where t.ItemID is null";
            var query = this.Context.Database.SqlQuery<SysDataItem>(sql, sp);
            
            return query;
        }
        /// <summary>
        /// 获取本身以及子集数据集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<SysDataItem> QuerySelfAndChirldren(string ids)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@ids",ids)
            };
            string sql = @"with t as
    (
    select * from Sys_DataItem where  CHARINDEX(','+LTRIM(ItemID)+',',','+@ids+',')>0"+
    @"
    union all
    select d.* from Sys_DataItem d join t as d2 on d.PID=d2.ItemID
    )
	 select d.* from Sys_DataItem d
	 left join t on d.ItemID=t.ItemID where t.ItemID is not null";
            var query = this.Context.Database.SqlQuery<SysDataItem>(sql, sp);

            return query;

        }
    }
}
