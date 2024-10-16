using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Service
{
    public partial class SwsCutOffMessageService : BaseService, IService.ISwsCutOffMessageService
    {
        public SwsCutOffMessageService(DbContext context) : base(context)
        {
        }
    }
}
