using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class View_DeviceInfoService : BaseService, IService.IView_DeviceInfoService
    {
        public View_DeviceInfoService(DbContext content) : base(content)
        {
        }
    }
}
