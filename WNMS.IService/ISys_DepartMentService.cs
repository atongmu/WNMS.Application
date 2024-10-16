using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISys_DepartMentService : IBaseService
    {
         IEnumerable<dynamic> QueryDepartmentTable(string dptname, int departEnum);
         IEnumerable<SysDepartMent> QueyExtentSelfAndChirldren_Depart(int DepartmentID);
         IEnumerable<SysDepartMent> QuerySelfAndChirldren_Depart(string ids);
    }
}
