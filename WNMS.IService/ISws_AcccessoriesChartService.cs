using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.IService
{
    public partial interface  ISws_AcccessoriesChartService:IBaseService
    {
        IEnumerable<WNMS.Model.CustomizedClass.StationDevice> LoadZtreeInfo(SysUser user, string stationName);
        IEnumerable<WNMS.Model.CustomizedClass.AccessEquInfo> LoadAccZtree(long id, byte type);
        IEnumerable<AccChartData> LoadAccChartData(long id);
        int ChartDataImport(List<SwsAcccessoriesChart> list);
        IEnumerable<AccessoriesDatas> ImportChartsData();
    }
}
