using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    static class NumberConverter
    {
        public static int HexToDecimal(string value)
        {
            return Convert.ToInt32(value, 16);
        }

        public static bool[] HexToBool(string value)
        {
            string temp = Convert.ToString(Convert.ToInt32(value, 16), 2);
            while (temp.Length < value.Length * 4) //на выходе нужно получить восьмизначные двоичные числа
            {
                temp = temp.Insert(0, "0");
            }
            bool[] result = new bool[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                result[i] = temp[i] == '1' ? true : false;
            }
            return result;
        }
        //reload for arrays
        public static bool[] HexToBool(string[] value)
        {
            bool[] output = HexToBool(value[0]);
            if (value.Length < 2)
                return output;
            for (int i = 1; i < value.Length; i++)
            {
                output = output.AddArray(HexToBool(value[i]));
            }

            return output;
        }
        public static int BoolArrayToDecimal(bool[] input)
        {
            string temp = "";
            foreach (bool b in input)
            {
                if (b)
                    temp += "1";
                else
                {
                    temp += "0";
                }
            }

            return Convert.ToInt32(temp, 2);
        }
        public static string BoolArrayToString(bool[] input)
        {
            string output = "";
            foreach (bool b in input)
            {
                switch (b)
                {
                    case true:
                        output += "1";
                        break;
                    case false:
                        output += "0";
                        break;
                }
            }

            return output;
        }

    }
}
