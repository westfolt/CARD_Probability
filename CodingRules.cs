using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    static class CodingRules
    {
        private static Dictionary<int, string> IA5dictionary = new Dictionary<int, string>();

        static CodingRules()
        {
            IA5dictionary.Add(1, "A");
            IA5dictionary.Add(2, "B");
            IA5dictionary.Add(3, "C");
            IA5dictionary.Add(4, "D");
            IA5dictionary.Add(5, "E");
            IA5dictionary.Add(6, "F");
            IA5dictionary.Add(7, "G");
            IA5dictionary.Add(8, "H");
            IA5dictionary.Add(9, "I");
            IA5dictionary.Add(10, "J");
            IA5dictionary.Add(11, "K");
            IA5dictionary.Add(12, "L");
            IA5dictionary.Add(13, "M");
            IA5dictionary.Add(14, "N");
            IA5dictionary.Add(15, "O");
            IA5dictionary.Add(16, "P");
            IA5dictionary.Add(17, "Q");
            IA5dictionary.Add(18, "R");
            IA5dictionary.Add(19, "S");
            IA5dictionary.Add(20, "T");
            IA5dictionary.Add(21, "U");
            IA5dictionary.Add(22, "V");
            IA5dictionary.Add(23, "W");
            IA5dictionary.Add(24, "X");
            IA5dictionary.Add(25, "Y");
            IA5dictionary.Add(26, "Z");
            IA5dictionary.Add(32, "");//space???
            IA5dictionary.Add(48, "0");
            IA5dictionary.Add(49, "1");
            IA5dictionary.Add(50, "2");
            IA5dictionary.Add(51, "3");
            IA5dictionary.Add(52, "4");
            IA5dictionary.Add(53, "5");
            IA5dictionary.Add(54, "6");
            IA5dictionary.Add(55, "7");
            IA5dictionary.Add(56, "8");
            IA5dictionary.Add(57, "9");
        }

        public static string decodeIA5(int symbol)
        {
            if (!IA5dictionary.ContainsKey(symbol))
            {
                //throw new ArgumentException("No such value in IA-5 dictionary!");
                return "@";//затычка на случай прихода битого ID от ВС
            }

            string s = "";
            IA5dictionary.TryGetValue(symbol, out s);
            return s;
        }
        //преобразовывает отрицательные двоичные числа, используется дополнительный код с добавлением единицы
        public static double decodeNegativeBinary(bool[] input)
        {
            input = input.Invert();
            int temp = NumberConverter.BoolArrayToDecimal(input);

            return (temp + 1) / (double)-128;
        }
    }
}
