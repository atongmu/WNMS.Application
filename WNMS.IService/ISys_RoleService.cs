using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISys_RoleService : IBaseService
    {
        //删除角色数据
        int DeleteRoles(List<int> ids, ref List<string> roleIdsList);
        //查询角色已有权限
        IEnumerable<SysModule> GetSaveActions(int RoleID);
        //分配权限
        int SetModule(List<SysRoleModule> list, int roleID);
        //根据部门查询所属用户
        IEnumerable<AllotUserRole> GetUserByDep(int id,int roleid);
        //添加客服角色的时候，gps表里添加一条记录
        int InsertPosition(long userid);
    }
}
