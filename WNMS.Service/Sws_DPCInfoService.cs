using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_DPCInfoService : BaseService, IService.ISws_DPCInfoService
    {
        //删除DPC信息
        public int DeleteDpcs(List<string> ids, ref List<string> dpcIdsList)
        {
            //查询被占用的DPC
            var userDpc = this.Context.Set<SwsRtuinfo>().Where(r => ids.Contains(r.DeviceType)).Select(r => r.DeviceType).ToList();
            var unuserDpc = ids;
            if (userDpc.Count > 0)
            {
                unuserDpc = ids.Except(userDpc).ToList();
                var usedpcs = this.Context.Set<SwsDpcinfo>().Where(r => userDpc.Contains(r.Dpcname));
                foreach (var item in usedpcs)
                {
                    dpcIdsList.Add(item.Dpcname);
                }
            }
            if (unuserDpc.Count > 0)
            {
                var dpclist = this.Context.Set<SwsDpcinfo>().Where(r => unuserDpc.Contains(r.Dpcname));
                this.Context.Set<SwsDpcinfo>().RemoveRange(dpclist);
            }
            return this.Context.SaveChanges();
        }
    }
}
