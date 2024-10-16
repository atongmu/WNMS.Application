using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{

    public partial class Sys_EarlyWarningPlanService : BaseService, ISys_EarlyWarningPlanService
    {
        public IEnumerable<dynamic> QueryPlansTable(int pageindex, int pagesize, string order, string filter, ref int Totalcount)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
               
            };
            string sql = "";
   
                sb.Append(@" SELECT se.ID,se.Title,se.Contents,se.Solution,se.Type,se.IsEnable,s.ItemName FROM dbo.Sys_EarlyWarningPlan se  left join [dbo].[Sys_DataItemDetail] s on s.F_ItemId=29 and s.ItemValue=Type where ");
          
            sql = sb.ToString() +filter;
            
            var query = this.Context.Database.SqlQuery<EarlyWarningPlan>(sql, sp).ToList();

            Totalcount = query.Count;
            return query;
        }
        //删除 
        public int DeletePlans(List<string> ids)
        {
            SqlParameter[] sp = new SqlParameter[] {

            };

            foreach (var mid in ids)
            {
                //var query = this.Context.Database.SqlQuery<SysEarlyWarningPlan>(" select 1 from [dbo].[Sws_Accessories] a LEFT JOIN dbo.Sws_AccessoriesEqu c ON a.id=c.AccessoriesID WHERE a.ID='" + mid + "'  HAVING COUNT(c.id)>= 1", sp).ToList();
                //if (query != null && query.Any())
                //{
                //    return -1;
                //}
                long id = Convert.ToInt32(mid);
                var ids_Info = this.Context.Set<SysEarlyWarningPlan>().Find(id );
                this.Context.Set<SysEarlyWarningPlan>().RemoveRange(ids_Info);
            }

            return this.Context.SaveChanges();
        }

        //添加保养及更新器件表
        public int AddPlan(SysEarlyWarningPlan sysEarlyWarningPlan)
        {
       
                this.Context.Update(sysEarlyWarningPlan);
         
          
 
            return this.Context.SaveChanges();
        }
        /// <summary>
        /// 查询报警方案
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IEnumerable<dynamic> QueryEventPlans(string filter)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
            };
            string sql = ""; 
            sb.Append(@" SELECT ID,Title,Contents,Solution,Type,s.ItemName FROM dbo.Sys_EarlyWarningPlan  left join [dbo].[Sys_DataItemDetail] s on s.F_ItemId=29 and s.ItemValue=Type where ");
            sql = sb.ToString() + filter; 
            var query = this.Context.Database.SqlQuery<EarlyWarningPlan>(sql, sp).ToList(); 
            return query;
        }
    }

}
