using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class Sys_RoleService : BaseService, IService.ISys_RoleService
    {
        //删除角色信息  角色权限 判断用户是否使用 
        public int DeleteRoles(List<int> ids, ref List<string> roleIdsList)
        {
            //查询被占用的角色
            var useRoleIDs = this.Context.Set<SysUserRole>().Where(r => ids.Contains(r.Role)).Select(r => r.Role).ToList();
            var unUseRoleIDs = ids;
            if (useRoleIDs.Count > 0)//被占用的角色
            {
                unUseRoleIDs = ids.Except(useRoleIDs).ToList();
                var useRoles = this.Context.Set<SysRole>().Where(r => useRoleIDs.Contains(r.RoleId));
                foreach (var item in useRoles)
                {
                    roleIdsList.Add(item.RoleName);
                }
            }
            if (unUseRoleIDs.Count > 0)//未被占用的角色
            {
                //角色权限
                var roleModuleList = this.Context.Set<SysRoleModule>().Where(r => unUseRoleIDs.Contains(r.RoleId));
                var stationList = this.Context.Set<SysRole>().Where(r => unUseRoleIDs.Contains(r.RoleId));
                this.Context.Set<SysRoleModule>().RemoveRange(roleModuleList);
                this.Context.Set<SysRole>().RemoveRange(stationList);
            }
            return this.Context.SaveChanges();
        }


        //分配权限
        public int SetModule(List<SysRoleModule> list, int roleID)
        {
            IQueryable<SysRoleModule> rolem = this.Query<SysRoleModule>(r => r.RoleId == roleID);
            if (rolem != null)
            {
                foreach (var item in rolem)
                {
                    this.Context.Attach(item);
                }
                this.Context.RemoveRange(rolem);
                
            }

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    this.Context.Attach(item);
                }
                this.Context.AddRange(list);
            }

            return this.Context.SaveChanges();
        }

        //查询角色已存在权限
        public IEnumerable<SysModule> GetSaveActions(int RoleID)
        {
            string sql = "";
            //IEnumerable<SysModule> list = from roleAction in this.Query<SysRoleModule>(r => r.RoleId == RoleID)
            //                              join action in this.Query<SysModule>(r => true) on roleAction action
            //                              ;
            //IEnumerable<SysModule> list = from roleAction in Context.Set<SysRoleModule>()
            //                              where roleAction.RoleId == RoleID
            //                              join action in this.Context.Set<SysModule>() on roleAction equals action
            //                              select new SysModule
            //                              {

            //                              };

            IEnumerable<SysModule> list = null;
            return list;
        }
        public IEnumerable<AllotUserRole> GetUserByDep(int id,int roleid)
        {
            var allList = from s in Query<SysUser>(s => s.Department == id)
                          join u in Query<SysDepartMent>(u => true) on s.Department equals u.DepartmentId into su
                          from sut in su.DefaultIfEmpty()
                          join role in Query<SysUserRole>(r =>r.Role == roleid) on s.UserId equals role.UserId into sr
                          from sre in sr.DefaultIfEmpty()
                          select new AllotUserRole
                          {
                              Flag = sre.UserId == s.UserId ? true : false,
                              UserId = s.UserId,
                              UserName = s.Account,
                              UserNickName = s.NickName,
                              depName = sut.DptName
                          };
            return allList.Distinct().OrderByDescending(r=>r.Flag);
        } 
        #region 添加客服角色的时候，gps表里添加一条记录
        public int InsertPosition(long userid)
        {
            int year = DateTime.Now.Year;
            DateTime updateTime = DateTime.Now;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid", userid),
                new SqlParameter("@year", year), 
                new SqlParameter("@updateTime",updateTime)
            }; 
            string sqlinsert = @" INSERT INTO  [WNMS_PhoneGps" + @year + "].[dbo].[RealTimePosition] ([UserID] ,[UpdateTime] ,[Lng]  ,[Lat]) VALUES (@userid  ,@updateTime  ,0  ,0)";
            var queryinsert = this.Context.Database.InsertData(sqlinsert, sp);
            return queryinsert;
        }
        #endregion
    }
}
