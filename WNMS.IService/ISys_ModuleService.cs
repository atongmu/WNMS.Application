using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface ISys_ModuleService : IBaseService
    {
        int AddModuleEntity(SysModule module, List<SysModuleButton> modulebutton);
        int EditModuleEntity(SysModule module, List<SysModuleButton> modulebutton);
        int DeleteModule(int moduleId);
        IEnumerable<Model.CustomizedClass.CacheModules> UserModules(int userID);
        //Task<IEnumerable<Model.CustomizedClass.CacheModules>> UserModules(int userID);
        IEnumerable<Model.CustomizedClass.CacheModules> RoleModules(int userID);
        //Task<IEnumerable<Model.CustomizedClass.CacheModules>> RoleModules(int userID);

        IEnumerable<SysModule> QueryUserModules(int userID);

        IEnumerable<SysModuleButton> QueryUserButtons(int userID, string url, string method);
    }
}
