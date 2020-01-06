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
        public double PrSSR { get; private set; }
        public long totalScansSSR { get; private set; }
        public long totalDetectionsSSR { get; private set; }
        public double PrPSR { get; private set; }
        public long totalScansPSR { get; private set; }
        public long totalDetectionsPSR { get; private set; }
        //private Key pathToCell;

        public RadarScreenCell(Key path)
        {
            PrSSR = 0;
            totalScansSSR = 0;
            totalDetectionsSSR = 0;
            PrPSR = 0;
            totalScansPSR = 0;
            totalDetectionsPSR = 0;
            //pathToCell = path;

            //EndAzimuth = endAzimuth;
            //EndRange = endRange;
        }
        public void ReCalculatePR()
        {
            if (totalScansSSR != 0)
                PrSSR = (double)totalDetectionsSSR / totalScansSSR;
        }
        public void Detection()
        {
            totalScansSSR++;
            totalDetectionsSSR++;
        }

        public void NoDetection()
        {
            totalScansSSR++;
        }
    }
}
