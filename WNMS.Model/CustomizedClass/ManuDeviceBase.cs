using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class ManuDeviceBase
    {
        public string ManufacterName { get; set; }
        public string PumpBrandName { get; set; }
        public double HighLift { get; set; }
        public double PumpEff { get; set; }
        public double MotorEff { get; set; }
        public int Manufacturer { get; set; }
        public int DeviceType { get; set; }
        public int ID { get; set; }
        public double WaterFlow { get; set; }
        public double ValueData { get; set; }
    }
}
