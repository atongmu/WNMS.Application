using Model;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;

namespace Common.WebApiActions
{
  public static class WebApiActions
    {
        /// <summary>
        /// 计算数据总和
        /// </summary>
        /// <param name="deviceJK"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        /// 
        public static double GetSumData(DeviceJK deviceJK, List<int> dataid)
        {
            double SumData = 0;
            foreach (var item in dataid)
            {
                double data = deviceJK.AnalogValues.ContainsKey(item.ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[item.ToString()]).ToString()),2) : 0;
                SumData = SumData + data;
            }


            return SumData;
        }

        /// <summary>
        /// 获取水质信息
        /// </summary>
        /// <param name="deviceJK"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        /// 
        public static WaterQuality GetWaterQualityData(DeviceJK deviceJK, List<int> dataid)
        {
            WaterQuality waterQuality = new WaterQuality();

            double data1 = deviceJK.AnalogValues.ContainsKey(dataid[0].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[0].ToString()]).ToString()),2) : 0;
            double data2 = deviceJK.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
            double data3 = deviceJK.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
            double data4 = deviceJK.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
            double data5 = deviceJK.AnalogValues.ContainsKey(dataid[4].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[4].ToString()]).ToString()), 2) : 0;
            double data6 = deviceJK.AnalogValues.ContainsKey(dataid[5].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[5].ToString()]).ToString()), 2) : 0;
            waterQuality.PH = data1;
            waterQuality.CL = data2;
            waterQuality.Turbidity = data3;
            waterQuality.Temperature = data4;
            waterQuality.Humidity = data5;
            waterQuality.Noise = data6;


            return waterQuality;
        }

        /// <summary>
        /// 设备动画---无负压泵数据处理方式
        /// </summary>
        /// <param name="deviceJK"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        /// 
        public static Pump GetDataByDataIDMongo(DeviceJK deviceJK, List<int> dataid)
        {
            Pump pump = new Pump();
            #region 方式
            bool PState = deviceJK.DigitalValues.ContainsKey(dataid[0].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[0].ToString()]).ToString()) : false;
            pump.PState = "";
            pump.PState = PState == true ? "手动" : "自动";
            #endregion
            #region 状态
             bool PFault = deviceJK.DigitalValues.ContainsKey(dataid[3].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[3].ToString()]).ToString()) : false;
            bool DeviceJKInfoB = deviceJK.DigitalValues.ContainsKey(dataid[4].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[4].ToString()]).ToString()) : false;
            bool DeviceJKInfoH = deviceJK.DigitalValues.ContainsKey(dataid[5].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[5].ToString()]).ToString()) : false;
            bool DeviceJKInfoG = deviceJK.DigitalValues.ContainsKey(dataid[6].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[6].ToString()]).ToString()) : false;
            pump.Electric = deviceJK.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[2].ToString()]).ToString()),2) : 0;
            pump.Frequency = deviceJK.AnalogValues.ContainsKey(dataid[1].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[1].ToString()]).ToString()), 2) : 0;
            if (pump.PState == "自动")
            {

                if (PFault == true)
                {
                    pump.PFault = "故障";
                    pump.Frequency = 0;
                }
                else if (DeviceJKInfoB == true)
                {
                    pump.PFault = "变频";
                }
                else if (DeviceJKInfoH == true)
                {
                    pump.PFault = "恒频";
                }
                else if (DeviceJKInfoG == true)
                {
                    pump.PFault = "工频";
                    pump.Frequency = 50;
                }
                else
                {
                    pump.PFault = "停止";
                    pump.Frequency = 0;
                }
            }
            else if (pump.PState == "手动")
            {
                if (PFault == true)
                {
                    pump.PFault = "故障";
                    pump.Frequency = 0;
                }
                else if (pump.Frequency != 0)
                {
                    pump.PFault = "变频";
                }
                else
                {
                    pump.PFault = "停止";
                    pump.Frequency = 0;
                }
            }
            #endregion
            return pump;
        }

        /// <summary>
        /// 设备动画---直饮水数据处理方式
        /// </summary>
        /// <param name="deviceJK"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public static Pump GetYDataMongo(DeviceJK deviceJK, List<int> dataid)
        {
            Pump pump = new Pump();
            if (dataid[0] == 2000)
            {
                pump.PumpName = "1#原水泵";
            }
            if (dataid[0] == 2001)
            {
                pump.PumpName = "2#原水泵";
            }
            if (dataid[0] == 2002)
            {
                pump.PumpName = "1#净水泵";
            }
            if (dataid[0] == 2003)
            {
                pump.PumpName = "2#净水泵";
            }
            if (dataid[0] == 2004)
            {
                pump.PumpName = "高压泵";
            }
            if (dataid[0] == 2005)
            {
                pump.PumpName = "清洗泵";
            }
            bool GDeviceJKInfo = deviceJK.DigitalValues.ContainsKey(dataid[1].ToString()) ? bool.Parse((deviceJK.DigitalValues[dataid[1].ToString()]).ToString()) : false;
            double ZDeviceJKInfo = deviceJK.AnalogValues.ContainsKey(dataid[0].ToString()) ? double.Parse((deviceJK.AnalogValues[dataid[0].ToString()]).ToString()) : 0;
            pump.PFault = "停止";
            pump.Frequency = 50;
            pump.PState = "无";

            if (dataid.Count == 3)
            {
                double Electric = deviceJK.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                pump.Electric = Electric;
            }

            if (dataid.Count == 4)
            {
                double Fre = deviceJK.AnalogValues.ContainsKey(dataid[2].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[2].ToString()]).ToString()), 2) : 0;
                pump.Frequency = Fre;

                double Electric = deviceJK.AnalogValues.ContainsKey(dataid[3].ToString()) ? Math.Round(double.Parse((deviceJK.AnalogValues[dataid[3].ToString()]).ToString()), 2) : 0;
                pump.Electric = Electric;
            }
            if (GDeviceJKInfo == true)
            {
                pump.PFault = "故障";
            }
            else if (ZDeviceJKInfo == 1)
            {
                pump.PFault = "正常";
            }
            else if (ZDeviceJKInfo == 0)
            {
                pump.Frequency = 0;
                pump.PFault = "停止";
            }
            else
            {
                pump.Frequency = 0;
                pump.PFault = "停止";
            }

            return pump;
        }


    }

}
