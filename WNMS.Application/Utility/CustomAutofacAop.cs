﻿using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WNMS.Application.Utility
{
    public class CustomAutofacAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"invocation.Methond={invocation.Method}");
            Console.WriteLine($"invocation.Arguments={string.Join(",", invocation.Arguments)}");

            invocation.Proceed(); //继续执行
            Console.WriteLine($"方法{invocation.Method}执行完成了");
        }
    }

    public interface IA
    {
        void Show(int id, string name);
    }

    [Intercept(typeof(CustomAutofacAop))]
    public class A : IA
    {
        public void Show(int id, string name)
        {
            Console.WriteLine($"This is {id} _ {name}");
        }
    }
}
