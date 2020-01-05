using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    static class FileProcessor
    {
        public static List<Asterix> Process(string[] fileStream, out int cat21Counter, out int cat34Counter, out int cat48Counter, out int cat8Counter,
            out int catOtherCounter)
        {
            int pointer = 0;
            List<Asterix> decodedData = new List<Asterix>();
            Cat34Decoder decoder34 = new Cat34Decoder();
            Cat48Decoder decoder48 = new Cat48Decoder();
            //переменные для проверки
            cat21Counter = 0;
            cat34Counter = 0;
            cat48Counter = 0;
            cat8Counter = 0;
            catOtherCounter = 0;
            do
            {
                int category = NumberConverter.HexToDecimal(fileStream[pointer]);
                int length = NumberConverter.HexToDecimal(fileStream[pointer + 1] + fileStream[pointer + 2]);
                switch (category)
                {
                    case 21:
                        cat21Counter++;
                        pointer += length;
                        category = 0;
                        length = 0;
                        break;
                    case 34:
                        cat34Counter++;
                        decodedData.AddRange(decoder34.Decode(fileStream.GetSubArray(pointer, pointer + length - 1), 0));
                        pointer += length;
                        category = 0;
                        length = 0;
                        break;
                    case 8:
                        cat8Counter++;
                        pointer += length;
                        category = 0;
                        length = 0;
                        break;
                        break;
                    case 48:
                        int lengthBefore = decodedData.Count;
                        decodedData.AddRange(decoder48.Decode(fileStream.GetSubArray(pointer, pointer + length - 1), 3));
                        cat48Counter += decodedData.Count - lengthBefore;
                        pointer += length;
                        category = 0;
                        length = 0;
                        break;
                    default:
                        catOtherCounter++;
                        pointer += length;
                        category = 0;
                        length = 0;
                        break;
                        break;
                }
            } while (pointer < fileStream.Length);

            return decodedData;
        }
    }
}
