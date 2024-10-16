using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNMS.Model.CustomizedClass
{
    public class DeviceJK:BaseEntity
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }
        /// <summary>
        /// 获取通讯ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// 设备二级分类
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 所属分区
        /// </summary>
        public string Partition { get; set; }
        /// <summary>
        /// 获取设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 获取 模拟量
        /// </summary>
        public Dictionary<string, object> AnalogValues { get; set; }

        /// <summary>
        /// 获取 开关量
        /// </summary>
        public Dictionary<string, object> DigitalValues { get; set; }

        public List<Pump> lstPumps { get; set; }
        /// <summary>
        /// 进水压力
        /// </summary>
        public double? PressureIN { get; set; }

        /// <summary>
        /// 出水压力
        /// </summary>
        public double? PressureOut { get; set; }

        /// <summary>
        /// 设定压力
        /// </summary>
        public double? PressureSet { get; set; }
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double? InstantFlow { get; set; }
        /// <summary>
        /// 累计电量
        /// </summary>
        public double? TotalPower { get; set; }
        /// <summary>
        /// 累计流量
        /// </summary>
        public double? TotalFlow { get; set; }
       
    }

    /// <summary>
    /// 手机端首页显示使用
    /// </summary>
    public class DeviceJK_SY
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }
        /// <summary>
        /// 获取通讯ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// 设备二级分类
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 直饮水设备分类
        /// </summary>
        public string DeviceType_ZYS { get; set; }
        
        /// <summary>
        /// 所属分区
        /// </summary>
        public string Partition { get; set; }
        /// <summary>
        /// 获取设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public string UpdateTime { get; set; }
      
        /// <summary>
        /// 进水压力
        /// </summary>
        public double? PressureIN { get; set; }

        /// <summary>
        /// 出水压力
        /// </summary>
        public double? PressureOut { get; set; }

        /// <summary>
        /// 设定压力
        /// </summary>
        public double? PressureSet { get; set; }
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double? InstantFlow { get; set; }
      

    }

    /// <summary>
    /// 实时数据
    /// </summary>
    public class DeviceJK_SS
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }
        /// <summary>
        /// 获取通讯ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// 设备二级分类
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 所属分区
        /// </summary>
        public string Partition { get; set; }
        /// <summary>
        /// 获取设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        public List<Pump> lstPumps { get; set; }
        /// <summary>
        /// 进水压力
        /// </summary>
        public double? PressureIN { get; set; }

        /// <summary>
        /// 出水压力
        /// </summary>
        public double? PressureOut { get; set; }

        /// <summary>
        /// 设定压力
        /// </summary>
        public double? PressureSet { get; set; }
        /// <summary>
        /// 设定流量
        /// </summary>
        public double? SetFlow { get; set; }
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double? InstantFlow { get; set; }
        /// <summary>
        /// 累计电量
        /// </summary>
        public double? TotalPower { get; set; }
        /// <summary>
        /// 累计流量
        /// </summary>
        public double? TotalFlow { get; set; }
        /// <summary>
        /// 水箱液位
        /// </summary>
        public double? LiquidHight { get; set; }
        /// <summary>
        ///是否在线
        /// </summary>
        public string IsOnline { get; set; }
        /// <summary>
        ///动态绑定模拟量及开关量
        /// </summary>
        public List<dynamic> list = new List<dynamic>();
    }

    /// <summary>
    /// 直饮水实时数据
    /// </summary>
    public class DeviceJK_SS_ZYS
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }
        /// <summary>
        /// 获取通讯ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
        public string EquipmentType { get; set; }
        /// <summary>
        /// 设备二级分类
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 所属分区
        /// </summary>
        public string Partition { get; set; }
        /// <summary>
        /// 获取设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        public List<Pump> lstPumps { get; set; }
        /// <summary>
        /// 浊度
        /// </summary>
        public double Turbidity { get; set; }
        /// <summary>
        /// 余氯
        /// </summary>
        public double CL { get; set; }

        /// <summary>
        /// PH值
        /// </summary>
        public double PH { get; set; }

        /// <summary>
        /// ORP
        /// </summary>
        public double ORP { get; set; }
        /// <summary>
        /// 盐度
        /// </summary>
        public double Salinity { get; set; }
        /// <summary>
        /// 溶解氧
        /// </summary>
        public double Oxygen { get; set; }
        /// <summary>
        /// 电导率
        /// </summary>
        public double Conductivity { get; set; }

        /// <summary>
        /// 原水箱液位
        /// </summary>
        public double YLevel { get; set; }
        /// <summary>
        /// 净水箱液位
        /// </summary>
        public double JLevel { get; set; }
        /// <summary>
        /// 原水出水压力
        /// </summary>
        public double YOutPressure { get; set; }
        /// <summary>
        /// 净水设定压力
        /// </summary>
        public double JSetpressure { get; set; }

        /// <summary>
        /// 净水出水压力
        /// </summary>
        public double JOutPressure { get; set; }
        /// <summary>
        /// 硬度
        /// </summary>
        public double Hardness { get; set; }

        /// <summary>
        /// 高压泵状态
        /// </summary>
        public string GPumpState { get; set; }
        /// <summary>
        /// 清洗泵状态
        /// </summary>
        public string QPumpState { get; set; }
        /// <summary>
        /// 进水阀状态
        /// </summary>
        public string QValveState { get; set; }
        /// <summary>
        /// 旁通阀状态
        /// </summary>
        public string PValveState { get; set; }
        /// <summary>
        /// 高压阀状态
        /// </summary>
        public string GValveState { get; set; }
        /// <summary>
        /// 浓水阀状态 2009
        /// </summary>
        public string NValveState { get; set; }
        /// <summary>
        /// 回水阀状态
        /// </summary>
        public string HValveState { get; set; }

        /// <summary>
        /// 净水箱进水电磁阀状态2076
        /// </summary>
        public string JJValveState { get; set; }

        /// <summary>
        /// 膜前电磁阀状态 2051
        /// </summary>
        public string MQValveState { get; set; }

        /// <summary>
        /// 反洗进水电磁阀状态 2052
        /// </summary>
        public string FJValveState { get; set; }



        /// <summary>
        ///是否在线
        /// </summary>
        public string IsOnline { get; set; }

        /// <summary>
        /// 瞬时流量2040
        /// </summary>
        public double Ssll { get; set; }

        /// <summary>
        /// 累计流量
        /// </summary>
        public double Ljll { get; set; }

        /// <summary>
        /// 电导率超标5036
        /// </summary>
        public string ConHighPolice { get; set; }

        /// <summary>
        /// ph不合格 5037
        /// </summary>
        public string PHBHG { get; set; }
        /// <summary>
        /// 余氯超标5038
        /// </summary>
        public string YLCBBJ { get; set; }
        
        /// <summary>
        /// 浊度超标5039
        /// </summary>
        public string ZDCBBJ { get; set; }

        /// <summary>
        /// 溶氧超标5040
        /// </summary>
        public string RYCBBJ { get; set; }

        /// <summary>
        /// ORP超标5041
        /// </summary>
        public string ORPCBBJ { get; set; }

        /// <summary>
        /// 盐度超标5042
        /// </summary>
        public string YANDCBBJ { get; set; }

        /// <summary>
        /// CoD超标5043
        /// </summary>
        public string CODCBBJ { get; set; }

        /// <summary>
        /// 硬度超标5044
        /// </summary>
        public string YINGDCBBJ { get; set; }

        

    }


    public class DeviceJK01 : BaseEntity
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }
      
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 进水压力
        /// </summary>
        public double? PressureIN { get; set; }

        /// <summary>
        /// 出水压力
        /// </summary>
        public double? PressureOut { get; set; }

        /// <summary>
        /// 设定压力
        /// </summary>
        public double? PressureSet { get; set; }
        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double? InstantFlow { get; set; }
        /// <summary>
        /// 累计电量
        /// </summary>
        public double? TotalPower { get; set; }
        /// <summary>
        /// 累计流量
        /// </summary>
        public double? TotalFlow { get; set; }

        /// <summary>
        /// 浊度
        /// </summary>
        public double Turbidity { get; set; }

        /// <summary>
        /// 余氯
        /// </summary>
        public double CL { get; set; }

        /// <summary>
        /// PH值
        /// </summary>
        public double PH { get; set; }

    }

    public class DeviceJK02 : BaseEntity
    {
        /// <summary>
        /// 获取设备ID
        /// </summary>
        public long EquipID { get; set; }

        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 浊度
        /// </summary>
        public double Turbidity { get; set; }

        /// <summary>
        /// 余氯
        /// </summary>
        public double CL { get; set; }

        /// <summary>
        /// PH值
        /// </summary>
        public double PH { get; set; }

        /// <summary>
        /// ORP
        /// </summary>
        public double ORP { get; set; }
        /// <summary>
        /// 盐度
        /// </summary>
        public double Salinity { get; set; }
        /// <summary>
        /// 溶解氧
        /// </summary>
        public double Oxygen { get; set; }
        /// <summary>
        /// 电导率
        /// </summary>
        public double Conductivity { get; set; }

    }

 

}
