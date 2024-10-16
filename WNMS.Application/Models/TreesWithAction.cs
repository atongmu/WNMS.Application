using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Application.Models
{
    public class TreesWithAction
    {
        public readonly ISys_AreaService _sys_AreaService = null;


        public TreesWithAction(ISys_AreaService sys_AreaService
            )
        {
            _sys_AreaService = sys_AreaService;
        }
        public string TreesOfArea(System.Linq.Expressions.Expression<Func<WNMS.Model.DataModels.SysArea, bool>> whereLambda)
        {
            var areas = _sys_AreaService.Query(whereLambda);
            IEnumerable<TreeAction> area = areas.Select(t => new TreeAction
            {
                id = t.Id,
                pId = t.Parents,
                name = "<i class='fa fa-globe'></i>" + t.AreaName,
                @checked = false,
                //icon = "../../Content/zTree/css/zTreeStyle/area.png"

            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(area);
            return json;

        }

    }
}