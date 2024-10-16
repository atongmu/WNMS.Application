using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SimpleFactory
    {
        //
        public static object _SyncHelper = new object();
        //使用反射+配置
        //private static string DllName = ConfigInfo.ConfigInfo.ComTypeConfig;
        //private static string DllName = "DPC.ComServer";
        private static Dictionary<string, string> dicBaseParameters = new Dictionary<string, string>();
      
        public static string GetParameters(string strParameters)
        {
            string iValue = string.Empty;
            if (!dicBaseParameters.ContainsKey(strParameters))
            {
                lock (_SyncHelper)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    if (!dicBaseParameters.ContainsKey(strParameters))
                    {
                        Assembly assembly = Assembly.LoadFile(path + "\\Common.dll");
                        string DllName = "Common.PartitionInfo";
                        Type tp = assembly.GetType(DllName);

                        //Type[] types = assembly.GetTypes();
                        object obj = new object();
                        //获取所有的属性列表
                        PropertyInfo[] PropertyList = tp.GetProperties();
                        foreach (var prop in PropertyList)
                        {
                            if (prop.IsDefined(typeof(DisplayNameAttribute), true)) // 先判断
                            {
                                object[] aAttributeArry = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                                foreach (DisplayNameAttribute attribute in aAttributeArry)
                                {
                                    //如果是当前选择的数据节点
                                    if (attribute.DisplayName == strParameters)
                                    {
                                        //反射获取描述信息
                                        if (prop.IsDefined(typeof(DefaultValueAttribute), true)) // 先判断
                                        {
                                            object[] aAttributeArrydes = prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                                            foreach (DefaultValueAttribute attributedes in aAttributeArrydes)
                                            {
                                                iValue = attributedes.Value.ToString();
                                                dicBaseParameters.Add(strParameters, iValue);
                                                break;
                                            }
                                        }
                                    }
                                }


                             
                            }

                        }

                    }

                }
            }

            if (!dicBaseParameters.ContainsKey(strParameters))
            {
                return iValue;
            }
            else
            {
                return dicBaseParameters[strParameters];
            }

        }
    }
}
