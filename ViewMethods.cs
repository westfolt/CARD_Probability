using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace CARD_Probability
{
    static class ViewMethods
    {
        public static void ShowSectorInfo(string sectorName, int azState, int rgState, int flState, out string Azimuth, out string Range, out string PrSSR, out string SSRAdditionalInfo, out string PrPSR, out string PSRAdditionalInfo)
        {
            Azimuth = "";
            Range = "";
            PrSSR = "0";
            SSRAdditionalInfo = "NaN обн. из NaN скан.";
            PrPSR = "0";
            PSRAdditionalInfo = "NaN обн. из NaN скан.";
            RadarScreenCell temp = null;
            string[] data = sectorName.Split('_');
            try
            {
                Azimuth = $"Азимут {data[2]} - {data[3]} град";
                Range = $"Дальность {data[5]} - {data[6]} км";
                Key keyToCell = MainWindow.GetKey(sectorName, flState);
                temp = PPI.GetCell(azState, rgState, keyToCell.Azimuth, keyToCell.Range, keyToCell.Altitude);
                PrSSR = $"PR SSR = {temp.PrSSR.ToString("f4")}";
                SSRAdditionalInfo = $"{temp.totalDetectionsSSR} обн. из {temp.totalScansSSR} скан.";
                PrPSR = $"PR PSR = {temp.PrPSR.ToString("f4")}";
                PSRAdditionalInfo = $"{temp.totalDetectionsPSR} обн. из {temp.totalScansPSR} скан.";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Sector data error!!!");
            }
        }

        public static void ShowFlightLevel(string input, out int FlightLevel)
        {
            FlightLevel = 0;
            FlightLevel = Convert.ToInt32(input.Split('_')[1]) / 50;
        }

        public static void ShowAzimuthStep(string input, out int AzimuthStep)
        {
            AzimuthStep = 0;
            AzimuthStep = Convert.ToInt32(input.Split('_')[2]);
        }

        public static void ShowRangeStep(string input, out int RangeStep)
        {
            RangeStep = 0;
            RangeStep = Convert.ToInt32(input.Split('_')[2]);
        }

        public static void ShowPrToDisplayState(string input, out int PrToDisplayStep)
        {
            PrToDisplayStep = 1; //SSR by default
            string selected = (input.Split('_'))[2];
            if (selected == "PSR")
            {
                PrToDisplayStep = 0;
            }
            else
            {
                PrToDisplayStep = 1;
            }
        }
    }
}
