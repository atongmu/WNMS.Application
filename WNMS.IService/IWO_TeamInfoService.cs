using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface IWO_TeamInfoService : IBaseService
    {
        IEnumerable<SysUser> GetUserByTeamIDOfPage(long teamid, int pageindex, int pagesize, ref int TotalCount);
        int SetTeamUser(WoTeamInfo t, List<WoTeamUser> list);

        IEnumerable<dynamic> GetTeamTreeNode();
        int DeleteTeamInfos(List<long> ids);

        IEnumerable<dynamic> GetTeamUserInfo(int teamid, string username, string sort, string order, int pageSize, int pageIndex, ref int totalcount);

        IEnumerable<dynamic> GetUnallotUser(int teamid);
        IEnumerable<dynamic> GetAllotUser(int teamid);
        IEnumerable<dynamic> GetUserTree(string searchtext, int teamid);
        IEnumerable<dynamic> SearchTreeTeam(string teamname);
        int AddTeamInfo(WoTeamInfo t, List<WoTeamUser> list);
        int EditeTeamInfo(WoTeamInfo t, List<WoTeamUser> list);
        IEnumerable<dynamic> GetUserByTeamID(int teamid);
    }
}
