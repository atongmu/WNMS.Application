using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WNMS.Model.DataModels;

namespace WNMS.Application.Models.Modules
{
    public class ModuleHelper
    {
        private List<SysModule> _Modules = null;

        public ModuleHelper(IEnumerable<SysModule> modules)
        {
            _Modules = modules.ToList();
        }



        public List<WebModule> ConvertToWebModule()
        {
            List<WebModule> list = new List<WebModule>();

            var firstMenu = _Modules.Where(m => m.Pnum == 0).OrderBy(m => m.Sort);

            foreach (var item in firstMenu)
            {
                var model = new WebModule
                {
                    id = item.ModuleNum,
                    url = item.Url,
                    icon = item.Icon,
                    text = item.ModuleName,
                    targetType = item.Target == 2 ? "iframe-tab" : "blank"
                };

                var second = _Modules.Where(m => m.Pnum == item.ModuleNum).OrderBy(m => m.Sort).ToList();

                if (second != null)
                {
                    List<WebModule> clist = new List<WebModule>();

                    foreach (var sec in second)
                    {
                        var mm = new WebModule
                        {
                            id = sec.ModuleNum,
                            url = sec.Url,
                            icon = sec.Icon,
                            text = sec.ModuleName,
                            targetType = sec.Target == 2 ? "iframe-tab" : "blank"
                        };

                        clist.Add(mm);
                    }

                    model.children = clist;

                    list.Add(model);
                }

            }

            return list;
        }
    }

    public class WebModule
    {
        public int id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public string targetType { get; set; }
        public List<WebModule> children { get; set; }
    }
}
