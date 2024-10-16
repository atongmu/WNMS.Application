using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public enum Enum
    {
        [Description("部门类型")]
        部门类型 = 1,
        [Description("报警等级")]
        报警等级 = 3,
        [Description("设备类型")]
        设备类型 = 5,
        [Description("变频分类")]
        变频分类 = 6,
        [Description("设备品牌")]
        设备品牌 = 7,
        [Description("设备分区")]
        设备分区 = 8,
        [Description("泵房类型")]
        泵房类型 = 10,
        [Description("泵房内置类型")]
        泵房内置类型 = 11,
        [Description("泵房安装位置")]
        泵房安装位置 = 12,
        [Description("直饮水设备类型")]
        直饮水设备类型 = 14,
        [Description("资产类型")]
        资产类型 = 15,
        [Description("门禁品牌")]
        门禁品牌 = 17,
        [Description("保养周期")]
        保养周期 = 23,
        [Description("方案类型")]
        方案类型 = 29,
        [Description("工艺图类型")]
        工艺图类型 = 30
    }

    public enum EventLevelEnum
    {
        [Description("无报警")]
        无报警 = 0,
        [Description("紧急报警")]
        紧急报警 = 1,
        [Description("一般报警")]
        一般报警 = 2,
        [Description("提示性报警")]
        提示性报警 = 3
    }

    public enum PartitionEnum
    {
        [Description("低区")]
        低区 = 1,
        [Description("中区")]
        中区 = 2,
        [Description("高区")]
        高区 = 3,
        [Description("超高区")]
        超高区 = 4,
        [Description("超超高区")]
        超超高区 = 5
    }

    public enum StationTypeEnum
    {
        [Description("小区泵站")]
        小区泵站 = 1,
        [Description("水厂泵站")]
        水厂泵站 = 2,
        [Description("加压站")]
        加压站 = 3,
        [Description("水质检测")]
        水质检测 = 4
    }

    public enum PostitionsEnum
    {
        [Description("地下")]
        地下 = 1,
        [Description("地上")]
        地上 = 2
    }
    //附件分类
    public enum AttachmentClassify
    {
        [Description("泵房")]
        泵房 = 1

    }
    //附件类型
    public enum FileType
    {
        [Description("图片")]
        图片 = 1,
        [Description("文本文档")]
        文本文档 = 2,
        [Description("电子表格")]
        电子表格 = 3,
        [Description("压缩文件")]
        压缩文件 = 4,
        [Description("视频文件")]
        视频文件 = 5,
        [Description("其他")]
        其他 = 6
    }
    public enum Target
    {
        [Description("expand")]
        expand = 1,
        [Description("iframe")]
        iframe = 2,
        [Description("open")]
        open = 3
    }

    public enum CameraType
    {
        [Description("大华")]
        大华 = 0,
        [Description("海康")]
        海康 = 1,
        [Description("萤石")]
        萤石 = 2,
        [Description("乐橙")]
        乐橙 = 3,
        [Description("大华DSS")]
        大华DSS = 4,
        [Description("海康流媒体")]
        海康流媒体 = 5
    }

    public enum WeekDay
    {
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
        Sunday = 1,

    }

    public enum IncidentType
    {
        [Description("维保巡检")]
        维保巡检 = 0,
        [Description("维保保养")]
        维保保养 = 1,
        [Description("二供泵房维修")]
        二供泵房维修 = 2,
        [Description("其他")]
        其他 = 3
    }

    public enum IncidentTypePY
    {
        [Description("XJ")]
        XJ = 0,
        [Description("BY")]
        BY = 1,
        [Description("WX")]
        WX = 2
    }

    public enum IncidentSource
    {
        [Description("移动端")]
        移动端 = 0,
        [Description("pc端")]
        pc端 = 1,
        [Description("报警自动生成")]
        报警自动生成 = 2
    }

    public enum IncidentState
    {
        [Description("未处理")]
        未处理 = 0,
        [Description("已派发")]
        已派发 = 1,
        [Description("无效")]
        无效 = 2
    }

    public enum DisposeState
    {
        [Description("未处理")]
        未处理 = 1,
        [Description("处理中")]
        处理中 = 2,
        [Description("处理完成")]
        处理完成 = 3
    }

    public enum WoState
    {
        [Description("未分派")]
        未分派 = -1,
        [Description("待接收")]
        待接收 = 0,
        [Description("已接收")]
        已接收 = 1,
        [Description("已到场")]
        已到场 = 2,
        [Description("处理中")]
        处理中 = 3,
        [Description("已完工")]
        已完工 = 4,
        [Description("已审核")]
        已审核 = 5,
        [Description("已撤回")]
        已撤回 = 6,
        [Description("已退单")]
        已退单 = 7,
        [Description("移交")]
        移交 = 8,
        [Description("驳回")]
        驳回 = 9,
        [Description("其他")]
        其他 = 10
    }
    public enum WOOperationType
    {
        [Description("派发")]
        派发 = 0,
        [Description("维修接单")]
        维修接单 = 1,
        [Description("维修到场")]
        维修到场 = 2,
        [Description("维修处理")]
        维修处理 = 3,
        [Description("维修完工")]
        维修完工 = 4,
        [Description("审核")]
        审核 = 5,
        [Description("退单")]
        退单 = 6,
        [Description("移交")]
        移交 = 7,
        [Description("驳回")]
        驳回 = 8
    }

    public enum EmergencyDegree
    {
        [Description("一般")]
        一般 = 0,
        [Description("紧急")]
        紧急 = 1,
        [Description("非常紧急")]
        非常紧急 = 2
    }
    public enum WOExtensionReview
    {
        [Description("审核通过")]
        审核通过 = 1,
        [Description("审核未通过")]
        审核未通过 = 2,
        [Description("未审核")]
        未审核 = 3
    }


    public enum ProcessingLevel
    {
        [Description("两小时")]
        两小时 = 2,
        [Description("十二小时")]
        十二小时 = 12,
        [Description("二十四小时")]
        二十四小时 = 24,
        [Description("两天")]
        两天 = 48,
        [Description("三天")]
        三天 = 72,
        [Description("七天")]
        七天 = 162,
        [Description("约期")]
        约期 = 6
    }
    public enum WorkOrder_enum
    {
        [Description("巡检周期")]
        巡检周期 = 24,
        [Description("行走方式")]
        行走方式 = 25,
        [Description("反馈项类型")]
        反馈项类型 = 26
    }
    public enum WarnRule_enum
    {
        [Description("比较符号")]
        比较符号 = 27,
        [Description("关系符号")]
        关系符号 = 28,
    }
    public enum PlanType
    {
        [Description("计划")]
        计划 = 1,
        [Description("常规")]
        常规 = 2
    }

    public enum Weeks
    {
        Mon = 1,
        Tue = 2,
        Wed = 3,
        Thu = 4,
        Fri = 5,
        Sat = 6,
        Sun = 7
    }
    public enum Extra_enum
    {
        [Description("厂商")]
        厂商 = 7,
        [Description("设备型号")]
        设备型号 = 5,
        [Description("水泵品牌")]
        水泵品牌 = 31

    }
    public enum eventType_enum
    {
        [Description("视频丢失")]
        视频丢失 = 131329,
        [Description("视频遮挡")]
        视频遮挡 = 131330,
        [Description("移动侦测")]
        移动侦测 = 131331,
        [Description("场景变更")]
        场景变更 = 131612,
        [Description("区域入侵")]
        区域入侵 = 131588
    }
}
