using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace CARD_Probability
{
    class RadarScreenCell
    {
        //    public int EndRange { get; }
        //    public int EndAzimuth { get; }
        public double PrSSR { get; set; }
        private long totalScans;
        private long totalDetections;
        private Key pathToCell;

        public RadarScreenCell(Key path)
        {
            PrSSR = 0;
            totalScans = 0;
            totalDetections = 0;
            pathToCell = path;

            //EndAzimuth = endAzimuth;
            //EndRange = endRange;
        }
        public void ReCalculatePR()
        {
            if (totalScans != 0)
                PrSSR = (double)totalDetections / totalScans;
        }
        public void Detection()
        {
            totalScans++;
            totalDetections++;
        }

        public void NoDetection()
        {
            totalScans++;
        }
    }
}
