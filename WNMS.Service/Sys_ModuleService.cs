using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sys_ModuleService : BaseService, IService.ISys_ModuleService
    {

        public int AddModuleEntity(SysModule module, List<SysModuleButton> modulebutton)
        {
            if (module != null)
            {
                this.Context.Attach(module);
                this.Context.Add<SysModule>(module);
            }
            if (modulebutton != null)
            {
                foreach (var item in modulebutton)
                {
                    this.Context.Attach(item);
                }
                this.Context.AddRange(modulebutton);
            }
            return this.Context.SaveChanges();
        }

        public int EditModuleEntity(SysModule module, List<SysModuleButton> modulebutton)
        {
            if (module != null)
            {
                this.Context.Set<SysModule>().Attach(module);
                this.Context.Update<SysModule>(module);
                List<SysModuleButton> buttonList = this.Query<SysModuleButton>(r => r.ModuleId == module.ModuleNum).ToList();
                if (buttonList != null)
                {
                    foreach (var item in buttonList)
                    {
                        this.Context.Set<SysModuleButton>().Attach(item);
                    }
                    this.Context.RemoveRange(buttonList);
                }

                if (modulebutton != null)
                {
                    foreach (var item in modulebutton)
                    {
                        this.Context.Set<SysModuleButton>().Attach(item);
                    }
                    this.Context.AddRange(modulebutton);
                }

                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        public int DeleteModule(int moduleId)
        {
            SysModule module = this.Find<SysModule>(moduleId);
            if (module == null)
            {
                throw new Exception("功能模块已经被删除");
            }
            else
            {
                List<int> ids = this.Query<SysModuleButton>(r => r.ModuleId == moduleId)?.Select(r => r.ModuleButtonId).ToList();
                ids.Add(moduleId);

                //删除用户权限中间表
                IQueryable<SysUserModule> userModule = this.Query<SysUserModule>(m => ids.Contains(m.ModuleId));
                if (userModule != null)
                {
                    foreach (var um in userModule)
                    {
                        this.Context.Attach(um);
                    }
                    this.Context.RemoveRange(userModule);
                }

                //删除角色权限中间表
                IQueryable<SysRoleModule> moduleRole = this.Query<SysRoleModule>(r => ids.Contains(r.ModuleId));
                if (moduleRole != null)
                {
                    foreach (var mr in moduleRole)
                    {
                        this.Context.Attach(mr);
                    }
                    this.Context.RemoveRange(moduleRole);
                }

                //删除按钮权限表
                IQueryable<SysModuleButton> moduleButton = this.Query<SysModuleButton>(b => b.ModuleId == moduleId);
                if (moduleButton != null)
                {
                    foreach (var mb in moduleButton)
                    {
                        this.Context.Attach(mb);
                    }
                    this.Context.RemoveRange(moduleRole);
                }

                //删除菜单权限
                this.Context.Attach(module);
                this.Context.Remove(module);
                return this.Context.SaveChanges();
            }
        }

        public IEnumerable<Model.CustomizedClass.CacheModules> UserModules(int userID)
        //public async Task<IEnumerable<Model.CustomizedClass.CacheModules>> UserModules(int userID)
        {
            var list = (from um in Context.Set<SysUserModule>()
                        join m in Context.Set<SysModule>() on um.ModuleId equals m.ModuleNum into userModulesTemp
                        from userModule in userModulesTemp.DefaultIfEmpty()
                        where um.UserId == userID && um.Type == 1
                        select new Model.CustomizedClass.CacheModules
                        {
                            HttpMethod = userModule.HttpMethod,
                            URL = userModule.Url
                        }).Union
                       (from um in Context.Set<SysUserModule>()
                        join mb in Context.Set<SysModuleButton>() on um.ModuleId equals mb.ModuleButtonId into btnModulesTemp
                        from btnModule in btnModulesTemp.DefaultIfEmpty()
                        where um.UserId == userID && um.Type == 2
                        select new Model.CustomizedClass.CacheModules
                        {
                            HttpMethod = btnModule.HttpMethod,
                            URL = btnModule.Url
                        });

            //return await list.ToListAsync();
            return list;
        }

        public IEnumerable<Model.CustomizedClass.CacheModules> RoleModules(int userID)
        //public async Task<IEnumerable<Model.CustomizedClass.CacheModules>> RoleModules(int userID)
        {
            var list = (from rm in Context.Set<SysRoleModule>()
                        join m in Context.Set<SysModule>() on rm.ModuleId equals m.ModuleNum into roleModulesTemp
                        from roleModule in roleModulesTemp.DefaultIfEmpty()
                        where (
                        from r in Context.Set<SysUserRole>()
                        where r.UserId == userID
                        select r.Role
                      ).Contains(rm.RoleId) && rm.Type == 1
                        select new Model.CustomizedClass.CacheModules
                        {
                            HttpMethod = roleModule.HttpMethod,
                            URL = roleModule.Url
                        }).Union
                       (from rm in Context.Set<SysRoleModule>()
                        join mb in Context.Set<SysModuleButton>() on rm.ModuleId equals mb.ModuleButtonId into btnModulesTemp
                        from btnModule in btnModulesTemp.DefaultIfEmpty()
                        where (
                        from r in Context.Set<SysUserRole>()
                        where r.UserId == userID
                        select r.Role
                      ).Contains(rm.RoleId) && rm.Type == 2
                        select new Model.CustomizedClass.CacheModules
                        {
                            HttpMethod = btnModule.HttpMethod,
                            URL = btnModule.Url
                        });

            return list;
            //return await list.ToListAsync();
        }

        public IEnumerable<SysModule> QueryUserModules(int userID)
        {
            var user = Context.Set<SysUser>().Where(u => u.UserId == userID).FirstOrDefault();

            IEnumerable<SysModule> list = null;

            if (user.IsAdmin)
            {
                list = from m in Context.Set<SysModule>()
                       .Where(m => m.IsEnable == true && m.IsMenu == true)
                       select m;
            }
            else
            {
                list = (from um in Context.Set<SysUserModule>()
                        join m in Context.Set<SysModule>() on um.ModuleId equals m.ModuleNum into temp
                        from module in temp.DefaultIfEmpty()
                        where um.UserId == userID
                              && um.Type == 1
                              && module.IsEnable == true
                              && module.IsMenu == true
                        select module).Union(
                    from rm in Context.Set<SysRoleModule>()
                    join m in Context.Set<SysModule>() on rm.ModuleId equals m.ModuleNum into temp
                    from module in temp.DefaultIfEmpty()
                    where (from r in Context.Set<SysUserRole>()
                           join role in Context.Set<SysRole>() on r.Role equals role.RoleId into roletemp
                           from roleModel in roletemp.DefaultIfEmpty()
                           where r.UserId == userID
                           && roleModel.IsEnable == true
                           select r.Role).Contains(rm.RoleId)
                           && rm.Type == 1
                           && module.IsEnable == true
                           && module.IsMenu == true
                    select module);
            }

            return list;
        }

        public IEnumerable<SysModuleButton> QueryUserButtons(int userID, string url, string method)
        {
            var user = Context.Set<SysUser>().Where(u => u.UserId == userID).FirstOrDefault();

            IEnumerable<SysModuleButton> list = null;

            if (user.IsAdmin)
            {
                list = Context.Set<SysModuleButton>().Where(
                    b => b.ModuleId ==
                    Context.Set<SysModule>().Where(
                        m => m.IsMenu == true
                        && m.IsEnable == true
                        && m.Url.ToLower() == url
                        && m.HttpMethod.ToLower() == method
                        ).FirstOrDefault().ModuleNum);
            }
            else
            {
                //list = Context.Set<Sys.ModuleButton>().Where(
                //       b => b.ModuleId ==
                //       Context.Set<SysModule>().Where(
                //           m => m.IsMenu == true
                //           && m.IsEnable == true
                //           && m.Url == url
                //           && m.HttpMethod == method
                //           ).FirstOrDefault().ModuleNum);

                list = from b in Context.Set<SysModuleButton>()
                       where b.ModuleId == ((from m in Context.Set<SysModule>()
                                             where m.IsMenu == true
                                             && m.IsEnable == true
                                             && m.Url == url
                                             && m.HttpMethod == method
                                             select m.ModuleNum).FirstOrDefault())
                && ((from ub in Context.Set<SysUserModule>()
                     where ub.Type == 2 && ub.UserId == userID
                     select ub.ModuleId
                        ).Union
                       (from rb in Context.Set<SysRoleModule>()
                        where rb.Type == 2 &&
                    (from ur in Context.Set<SysUserRole>()
                     where ur.UserId == userID
                     select ur.Role).Contains(rb.RoleId)
                        select rb.ModuleId
                    )).Contains(b.ModuleButtonId)
                       select b;
            }

            return list;
        }
    }
}
