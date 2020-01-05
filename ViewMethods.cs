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
        public static void ShowSectorInfo(string sectorName, int flightLevel, out string Azimuth, out string Range, out string PrSSR)
        {
            Azimuth = "";
            Range = "";
            PrSSR = "0";
            string temp = "5";
            string[] data = sectorName.Split('_');
            try
            {
                Azimuth = $"Азимут {data[1]} - {data[2]} град";
                Range = $"Дальность {data[4]} - {data[5]} км";
                temp = (PPI.SelectByKey(MainWindow.GetKey(sectorName,flightLevel)).PrSSR).ToString("f4");
                PrSSR = $"PR SSR = {temp}";
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
    }
}
