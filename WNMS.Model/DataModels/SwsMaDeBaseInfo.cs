using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsMaDeBaseInfo
    {
        public int Manufacturer { get; set; }
        public int PumpBrand { get; set; }
        public double HighLift { get; set; }
        public double PumpEff { get; set; }
        public double MotorEff { get; set; }
        public int Id { get; set; }
        public double WaterFlow { get; set; }
        public double ValueData { get; set; }
    }
}
