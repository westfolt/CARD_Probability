using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability //доделать на случай появления полей SP!!!!!!!
{
    class Cat34Decoder : AsterixDecoder
    {
        public override bool[] FSPEC { get; set; }
        //pointer устанавливается по-умолчанию, вызов возможен с любым
        public override Asterix[] Decode(string[] bytes, int pointer)//преобразует фрейм категории Астерикс 34 в объект соответствующего класса
        {
            pointer = 4;//начинаем с 5 поля, т.к. там содержится нужная информация категории
            FSPEC = NumberConverter.HexToBool(bytes[3]);
            byte SAC = 0;
            byte SIC = 0;
            Cat34MessageType MessageType = Cat34MessageType.Error;
            double TimeOfDay = 0;
            double SectorNumber = 0;
            double AntennaRotationPeriod = 0;
            if (FSPEC[0])
            {
                SAC = Convert.ToByte(NumberConverter.HexToDecimal(bytes[pointer++]));
                SIC = Convert.ToByte(NumberConverter.HexToDecimal(bytes[pointer++]));
            }

            if (FSPEC[1])
            {
                switch (NumberConverter.HexToDecimal(bytes[pointer]))
                {
                    case 1:
                        MessageType = Cat34MessageType.North;
                        break;
                    case 2:
                        MessageType = Cat34MessageType.SectorCross;
                        break;
                    default:
                        MessageType = Cat34MessageType.Error;
                        break;
                }
                pointer++;
            }

            if (FSPEC[2])
            {
                TimeOfDay = NumberConverter.HexToDecimal(bytes[pointer] + bytes[pointer + 1] + bytes[pointer + 2]) /
                            (double)128; //в секундах
                pointer += 3;
            }

            if (FSPEC[3])
            {
                SectorNumber = Convert.ToByte(NumberConverter.HexToDecimal(bytes[pointer++])) * 1.41;//в градусах
            }

            if (FSPEC[4])
            {
                AntennaRotationPeriod = NumberConverter.HexToDecimal(bytes[pointer]) / (double)128;
            }

            return new[] { new Cat34(SAC, SIC, MessageType, TimeOfDay, SectorNumber, AntennaRotationPeriod) };
        }
    }
}
