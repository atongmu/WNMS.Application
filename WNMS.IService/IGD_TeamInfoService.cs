using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;
namespace WNMS.IService
{
    public partial  interface IGD_TeamInfoService:IBaseService
    {
        IEnumerable<dynamic> GetTeamInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount);
        IEnumerable<dynamic> GetDepartUserNode(string searchText, int teamid, string userids);
        IEnumerable<dynamic> GetAllotUser(int teamid);
        int AddTeamInfo(GdTeamInfo t, List<int> userids);
        int EditeTeamInfo(GdTeamInfo t, List<int> userids);
        int DeleteTeamInfo(string teamids);
    }
}
