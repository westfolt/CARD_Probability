using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace CARD_Probability
{
    //хранит ключ для словаря путей
    public struct Key //1st Azimuth(clockwise from 0), 2nd Range(from close), 3rd Altitude(from lowest)
    {
        public int Azimuth { get; private set; }
        public int Range { get; private set; }
        public int Altitude { get; private set; }

        public Key(int azimuth, int range, int altitude)
        {
            Azimuth = azimuth;
            Range = range;
            Altitude = altitude;
        }

        public override bool Equals(object obj)
        {
            if (obj is Key)
            {
                Key temp = (Key) obj;
                return Azimuth.Equals(temp.Azimuth) && Range.Equals(temp.Range) && Altitude.Equals(temp.Altitude);
            }

            return false;
        }
    }
}
