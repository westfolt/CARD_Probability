using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    static class Extentions
    {
        public static bool[] AddArray(this bool[] arr1, bool[] arr2)
        {
            bool[] temp = new bool[arr1.Length + arr2.Length];
            for (int i = 0; i < arr1.Length; i++)
            {
                temp[i] = arr1[i];
            }

            for (int i = 0, j = arr1.Length; i < arr2.Length; i++, j++)
            {
                temp[j] = arr2[i];
            }

            return temp;
        }

        //gets new array from given between index 1 and 2
        public static bool[] GetSubArray(this bool[] input, int index1, int index2)
        {
            if (index1 > input.Length || index1 < 0 || index2 > input.Length || index2 < 0 || index2 < index1)
                throw new IndexOutOfRangeException("Method in extensions class works wrong!");

            bool[] output = new bool[index2 - index1 + 1];
            for (int i = index1, j = 0; i <= index2; i++, j++)
            {
                output[j] = input[i];
            }

            return output;
        }

        public static bool[] Invert(this bool[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i])
                {
                    input[i] = false;
                }
                else
                {
                    input[i] = true;
                }
            }

            return input;
        }

        public static string MakeString(this string[] input, int index1, int index2)
        {
            if (index1 > input.Length || index1 < 0 || index2 > input.Length || index2 < 0 || index2 < index1)
                throw new IndexOutOfRangeException("Method in extensions class works wrong!");

            string temp = "";
            for (int i = index1; i <= index2; i++)
            {
                temp += input[i];
            }

            return temp;
        }

        public static string MakeString(this string[] input)
        {
            string temp = "";
            for (int i = 0; i < input.Length; i++)
            {
                temp += input[i];
            }

            return temp;
        }

        //extends array of Asterix type;
        public static Asterix[] Add(this Asterix[] firstArr, Asterix[] secondArr)
        {
            Asterix[] output = new Asterix[firstArr.Length + secondArr.Length];
            for (int i = 0; i < firstArr.Length; i++)
            {
                output[i] = firstArr[i];
            }

            for (int i = firstArr.Length, j = 0; i < output.Length; i++, j++)
            {
                output[i] = secondArr[j];
            }

            return output;
        }

        public static string[] GetSubArray(this string[] input, int index1, int index2)
        {
            if (index1 > input.Length || index1 < 0 || index2 > input.Length || index2 < 0 || index2 < index1)
                throw new IndexOutOfRangeException("Method in extensions class works wrong!");

            string[] output = new string[index2 - index1 + 1];
            for (int i = index1, j = 0; i <= index2; i++, j++)
            {
                output[j] = input[i];
            }

            return output;
        }
    }
}
