using System;
using System.Collections;
using System.Reflection;


namespace Command
{
    internal sealed class EnumField : Attribute
    {
        private object fieldValue;
        private bool bind;
        private FieldInfo fieldInfo;
        private IList enumGroups;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldValue">枚举项值</param>
        /// <param name="bind">是否要绑定到下拉列表控件</param>
        /// <param name="enumGroups">枚举项所属的组</param>
        internal EnumField(object fieldValue, bool bind, string[] enumGroups)
        {
            this.fieldValue = fieldValue;
            this.bind = bind;
            this.enumGroups = ArrayList.Adapter(enumGroups);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldValue">枚举项值</param>
        /// <param name="bind">是否要绑定到下拉列表控件</param>
        internal EnumField(object fieldValue, bool bind)
                : this(fieldValue, bind, new string[] { })
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldValue">枚举项值</param>
        /// <param name="enumGroups">需要绑定的类型</param>
        internal EnumField(object fieldValue, string[] enumGroups)
                : this(fieldValue, true, enumGroups)
        {
        }

        /// <summary>
        /// 构造函数，默认需要绑定
        /// </summary>
        /// <param name="fieldValue">枚举项值</param>
        internal EnumField(object fieldValue)
                : this(fieldValue, true, new string[] { })
        {
        }

        /// <summary>
        /// 枚举项值
        /// </summary>
        internal object FieldValue
        {
            get { return this.fieldValue; }
        }

        internal bool Bind
        {
            get { return this.bind; }
        }

        internal FieldInfo FieldInfo
        {
            get { return this.fieldInfo; }
            set { this.fieldInfo = value; }
        }

        internal IList EnumGroups
        {
            get { return this.enumGroups; }
        }

    }
}
