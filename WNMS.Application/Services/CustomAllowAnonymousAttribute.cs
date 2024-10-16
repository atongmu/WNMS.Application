using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Application.Services
{
    /// <summary>
    /// 跟以前一样，可以支持匿名的
    /// </summary>
    public class CustomAllowAnonymousAttribute : Attribute, IAllowAnonymous
    {
    }
}
