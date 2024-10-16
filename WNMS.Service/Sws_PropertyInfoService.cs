using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
namespace WNMS.Service
{
    public partial class Sws_PropertyInfoService : BaseService, ISws_PropertyInfoService
    {
        public IEnumerable<dynamic> QueryPropertyTable(bool IsAdmin, int pageindex, int pagesize, int ptypeID,  string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
               
                new SqlParameter("@ptypeID",ptypeID),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryPropertyTable @IsAdmin,@ptypeID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }
    }
}
