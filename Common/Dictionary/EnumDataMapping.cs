using System;
using Command;


namespace Command
{
    public static class SysDicEnum
    {
        #region example
        ///// <summary>
        ///// 是否有效的枚举
        ///// </summary>
        //[EnumDesc("是否有效")]
        //public enum UserLock
        //{
        //    /// <summary>
        //    /// 有效
        //    /// </summary>
        //    [EnumField("01", new string[] { "Type" })]
        //    有效,
        //    /// <summary>
        //    /// 锁定
        //    /// </summary>
        //    [EnumField("00", new string[] { "Type" })]
        //    锁定,
        //    /// <summary>
        //    /// 密码输错锁定
        //    /// </summary>
        //    [EnumField("02", new string[] { "Type" })]
        //    密码输错锁定,
        //    /// <summary>
        //    /// 管理员修改密码
        //    /// </summary>
        //    [EnumField("03")]
        //    管理员修改密码

        //}
        #endregion

        /// <summary>
        /// 是否有效
        /// </summary>
        [EnumDesc("是否有效")]
        public enum IsValid
        {
            /// <summary>
            /// 有效
            /// </summary>
            [EnumField("valid")]
            有效,

            /// <summary>
            /// 无效
            /// </summary>
            [EnumField("inValid")]
            无效,

            /// <summary>
            /// 删除
            /// </summary>
            [EnumField("deleted")]
            删除
        }

        /// <summary>
        /// 是否冻结
        /// </summary>
        [EnumDesc("是否冻结")]
        public enum IsFrozen
        {
            /// <summary>
            /// 已冻结
            /// </summary>
            [EnumField("frozen")]
            已冻结,

            /// <summary>
            /// 未冻结
            /// </summary>
            [EnumField("unFrozen")]
            未冻结
        }


        /// <summary>
        /// 设备分类
        /// </summary>
        [EnumDesc("设备分类")]
        public enum ClassType
        {
            /// <summary>
            /// 设备
            /// </summary>
            [EnumField("equipment")]
            设备,

            /// <summary>
            /// 配件
            /// </summary>
            [EnumField("parts")]
            配件
        }


        /// <summary>
        /// 设备状态
        /// </summary>
        [EnumDesc("设备状态")]
        public enum EquipmentState
        {
            /// <summary>
            /// 运行
            /// </summary>
            [EnumField("run")]
            运行,

            /// <summary>
            /// 停机
            /// </summary>
            [EnumField("stop")]
            停机,

            /// <summary>
            /// 维修
            /// </summary>
            [EnumField("repair")]
            维修,

            /// <summary>
            /// 损毁
            /// </summary>
            [EnumField("demage")]
            损毁,
            /// <summary>
            /// 在库
            /// </summary>
            [EnumField("storage")]
            在库
        }


        /// <summary>
        /// 是否停产
        /// </summary>
        [EnumDesc("是否停产")]
        public enum IsContinue
        {
            /// <summary>
            /// 已停产
            /// </summary>
            [EnumField("disContinue")]
            已停产,

            /// <summary>
            /// 未停产
            /// </summary>
            [EnumField("continue")]
            未停产
        }


        /// <summary>
        /// 是否审批
        /// </summary>
        [EnumDesc("是否审批")]
        public enum IsApproval
        {
            /// <summary>
            /// 已审批
            /// </summary>
            [EnumField("approval")]
            已审批,

            /// <summary>
            /// 待审批
            /// </summary>
            [EnumField("unApproval")]
            待审批
        }

        /// <summary>
        /// 是否修复
        /// </summary>
        [EnumDesc("是否修复")]
        public enum IsFixed
        {
            /// <summary>
            /// 已修复
            /// </summary>
            [EnumField("fixed")]
            已修复,

            /// <summary>
            /// 待修复
            /// </summary>
            [EnumField("unFixed")]
            待修复
        }


        /// <summary>
        /// 功能列表
        /// </summary>
        [EnumDesc("功能列表")]
        public enum FunctionList
        {
            /// <summary>
            /// 增加
            /// </summary>
            [EnumField("add")]
            增加,

            /// <summary>
            /// 删除
            /// </summary>
            [EnumField("delete")]
            删除,

            /// <summary>
            /// 修改
            /// </summary>
            [EnumField("modify")]
            修改,

            /// <summary>
            /// 导出
            /// </summary>
            [EnumField("outPut")]
            导出
        }

        /// <summary>
        /// 字典类型
        /// </summary>
        [EnumDesc("字典类型")]
        public enum DicClass
        {
            /// <summary>
            /// 设备类型
            /// </summary>
            [EnumField("Device")]
            设备类型,

            /// <summary>
            /// 参数类型
            /// </summary>
            [EnumField("Parame")]
            参数类型,

            ///// <summary>
            ///// 单位类型
            ///// </summary>
            //[EnumField("Unit")]
            //单位类型,

            ///// <summary>
            ///// 方法类型
            ///// </summary>
            //[EnumField("Method")]
            //方法类型,

            ///// <summary>
            ///// 测量仪器
            ///// </summary>
            //[EnumField("MeasurD")]
            //测量仪器,

            /// <summary>
            /// 设备配置
            /// </summary>
            [EnumField("DeviceConfig")]
            设备配置
        }


        /// <summary>
        /// 联系方式
        /// </summary>
        [EnumDesc("联系方式")]
        public enum ContactType
        {
            /// <summary>
            /// 手机方式
            /// </summary>
            [EnumField("1")]
            手机方式,

            /// <summary>
            /// 微信方式
            /// </summary>
            [EnumField("2")]
            微信方式
        }

        /// <summary>
        /// 站点类型
        /// </summary>
        [EnumDesc("站点类型")]
        public enum SiteType
        {
            /// <summary>
            /// 泵房
            /// </summary>
            [EnumField("stationPump")]
            泵房,

            /// <summary>
            /// 仓库
            /// </summary>
            [EnumField("stationHouse")]
            仓库
        }

        /// <summary>
        /// 流水号
        /// </summary>
        [EnumDesc("流水号")]
        public enum OrderNo
        {
            /// <summary>
            /// 收货单号
            /// </summary>
            [EnumField("Storage")]
            收货单号,

            /// <summary>
            /// 领用单号
            /// </summary>
            [EnumField("Allot")]
            领用单号,

            /// <summary>
            /// 调拨单号
            /// </summary>
            [EnumField("Take")]
            调拨单号,

            /// <summary>
            /// 盘点单号
            /// </summary>
            [EnumField("Check")]
            盘点单号,

            /// <summary>
            /// 采购单号
            /// </summary>
            [EnumField("Purchase")]
            采购单号,

            /// <summary>
            /// 设备档案编号
           /// </summary>
            [EnumField("Equipment")]
            档案编号
        }


        /// <summary>
        /// 领用分类
        /// </summary>
        [EnumDesc("领用分类")]
        public enum TakeType
        {
            /// <summary>
            /// 设备维修
            /// </summary>
            [EnumField("Fix")]
            设备维修,

            /// <summary>
            /// 损毁替换
            /// </summary>
            [EnumField("Damage")]
            损毁替换,

            /// <summary>
            /// 消耗补给
            /// </summary>
            [EnumField("Loss")]
            消耗补给,

            /// <summary>
            /// 其他
            /// </summary>
            [EnumField("Other")]
            其他
        }

        /// <summary>
        /// 单据状态
        /// </summary>
        [EnumDesc("单据状态")]
        public enum OrderState
        {
            /// <summary>
            /// 新建
            /// </summary>
            [EnumField("Create",new string[] { "approve" })]
            新建,

            /// <summary>
            /// 审批通过
            /// </summary>
            [EnumField("Adopted", new string[] { "approve" })]
            审批通过,

            /// <summary>
            /// 驳回
            /// </summary>
            [EnumField("Rejected", new string[] { "approve" })]
            驳回,

            /// <summary>
            /// 处理中
            /// </summary>
            [EnumField("InHandle")]
            处理中,

            /// <summary>
            /// 已完成
            /// </summary>
            [EnumField("Complate")]
            已完成
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        [EnumDesc("单据类型")]
        public enum OrderType
        {
            /// <summary>
            /// 采购单
            /// </summary>
            [EnumField("Purchase")]
            采购单,

            /// <summary>
            /// 盘点单
            /// </summary>
            [EnumField("Inventory")]
            盘点单
        }

        /// <summary>
        /// 设备状态
        /// </summary>
        [EnumDesc("操作类型")]
        public enum OpreateType
        {
            /// <summary>
            /// 安装
            /// </summary>
            [EnumField("install")]
            安装,

            /// <summary>
            /// 建档
            /// </summary>
            [EnumField("create")]
            建档,

            /// <summary>
            /// 运行
            /// </summary>
            [EnumField("run")]
            运行,

            /// <summary>
            /// 停机
            /// </summary>
            [EnumField("stop")]
            停机,

            /// <summary>
            /// 维修
            /// </summary>
            [EnumField("repair")]
            维修,

            /// <summary>
            /// 事故
            /// </summary>
            [EnumField("Accident")]
            事故,
            /// <summary>
            /// 报废
            /// </summary>
            [EnumField("scarp")]
            报废
        }

        [EnumDesc("器件类型")]
        public enum DevicetType
        {
            /// <summary>
            /// 机械件
            /// </summary>
            [EnumField("Mechanical")]
            机械件,

            /// <summary>
            /// 泵器件
            /// </summary>
            [EnumField("PumpDevice")]
            泵器件,

            /// <summary>
            /// 电器件
            /// </summary>
            [EnumField("Electrical")]
            电气件
        }
    }

}
