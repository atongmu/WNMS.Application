using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;

namespace WNMS.Service
{
    public class JR_AreaService:BaseService,IJR_AreaService
    {
        public JR_AreaService(DbContext content) : base(content)
        {

        }
    }
}
