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
    public partial class Sws_AccessoriesService : BaseService,ISws_AccessoriesService
    {
        public IEnumerable<dynamic> QueryAccessoriesTable(  int pageindex, int pagesize, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
 
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAccessoriesTable @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //删除 
        public int DeleteAccessories(List<string> ids)
        {
            SqlParameter[] sp = new SqlParameter[] {
                 
            };
            
            foreach (var mid in ids)
            {
                var query = this.Context.Database.SqlQuery<SwsAccessories>(" select 1 from [dbo].[Sws_Accessories] a LEFT JOIN dbo.Sws_AccessoriesEqu c ON a.id=c.AccessoriesID WHERE a.ID='"+mid+"'  HAVING COUNT(c.id)>= 1", sp).ToList();
                if (query != null &&query.Any())
                {
                    return -1;
                }

                var ids_Info = this.Context.Set<SwsAccessories>().Find(mid);
                this.Context.Set<SwsAccessories>().RemoveRange(ids_Info);
            }
           
            return this.Context.SaveChanges();
        }


        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AccessoriesImport(List<SwsAccessories> list)
        {
            try
            {
                foreach (var item in list)
                {
                    this.Context.Add<SwsAccessories>(item);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
    }
}
