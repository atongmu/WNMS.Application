using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Command
{
    public static class EnumParser
    {
        private static Hashtable cachedEnum = new Hashtable();

        /// <summary>
        /// 根据枚举类型得到该枚举对象的描述值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举对象的描述值</returns>
        public static string GetEnumDescription(Type enumType)
        {
            EnumDesc[] enumDescs = (EnumDesc[])enumType.GetCustomAttributes(typeof(EnumDesc), false);
            if (enumDescs.Length != 1)
                return string.Empty;
            return enumDescs[0].Description;
        }

        /// <summary>
        /// 根据枚举字段得到该字段的值
        /// </summary>
        /// <param name="enumField">枚举字段</param>
        /// <returns>字段的值</returns>
        public static object GetEnumFiledValue(object enumField)
        {
            EnumField[] enumFields = GetEnumFields(enumField.GetType());
            foreach (EnumField field in enumFields)
            {
                if (field.FieldInfo.Name == enumField.ToString())
                    return field.FieldValue;
            }
            return null;
        }

        /// <summary>
        /// 根据枚举类型和某个枚举字段的值得到该字段名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumFieldName">枚举字段名称</param>
        /// <returns>字段的值</returns>
        public static string GetEnumFiledValue(Type enumType, object enumFieldName)
        {
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                if (field.FieldInfo.Name.Equals(enumFieldName))
                    return field.FieldValue.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据枚举类型和某个枚举字段的值得到该字段名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumFieldValue">枚举字段的值</param>
        /// <returns>字段名称</returns>
        public static string GetEnumFiledName(Type enumType, object enumFieldValue)
        {
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                if (field.FieldValue.Equals(enumFieldValue))
                    return field.FieldInfo.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据枚举类型得到枚举项值的数组
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumGroup">枚举项所属的组</param>
        /// <returns>枚举项值的object数组</returns>
        public static object[] GetEnumFiledValues(Type enumType, string enumGroup)
        {
            ArrayList fieldValues = new ArrayList();
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                if (field.EnumGroups.Contains(enumGroup))
                    fieldValues.Add(field.FieldValue);
            }
            return fieldValues.ToArray();
        }

        /// <summary>
        /// 根据枚举类型得到枚举项值的数组
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举项值的object数组</returns>
        public static object[] GetEnumFiledValues(Type enumType)
        {
            ArrayList fieldValues = new ArrayList();
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                fieldValues.Add(field.FieldValue);
            }
            return fieldValues.ToArray();
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFiledList(Type enumType)
        {
            Dictionary<string, string> dicList = new Dictionary<string, string>();
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                dicList.Add(field.FieldInfo.Name.ToString() , field.FieldValue.ToString());
            }
            return dicList;
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFiledList(Type enumType, string enumGroup)
        {
            Dictionary<string, string> dicList = new Dictionary<string, string>();
            ArrayList fieldValues = new ArrayList();
            EnumField[] enumFields = GetEnumFields(enumType);
            foreach (EnumField field in enumFields)
            {
                if (field.EnumGroups.Contains(enumGroup))
                    dicList.Add(field.FieldInfo.Name.ToString(), field.FieldValue.ToString());
            }
            return dicList;
        }

        private static EnumField[] GetEnumFields(Type enumType)
        {
            EnumField[] enumFields = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList edAL = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumField), false);
                    if (eds.Length != 1)
                        continue;
                    ((EnumField)eds[0]).FieldInfo = fi;
                    edAL.Add(eds[0]);
                }

                cachedEnum.Add(enumType.FullName, (EnumField[])edAL.ToArray(typeof(EnumField)));
            }
            enumFields = (EnumField[])cachedEnum[enumType.FullName];

            return enumFields;
        }
    }
}
