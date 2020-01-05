using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    class Cat48Decoder : AsterixDecoder
    {
        public override bool[] FSPEC { get; set; }

        public override Asterix[] Decode(string[] bytes, int pointer)//первый вызов должен быть с pointer=3
        {
            bool specialPurpose = false; //индикатор наличия поля SP
            bool reservedExpansion = false; //индикатор наличия поля RE
            //int pointer = 3; //начинаем с 4 поля, т.к. надо разбирать fspec переменной длины
            byte fspecLength = 1; //показывает количество байт поля fspec

            bool[] tempFSPEC = NumberConverter.HexToBool(bytes[pointer++]);
            while (tempFSPEC[tempFSPEC.Length - 1]) //пока последний бит fspec==1
            {
                tempFSPEC = tempFSPEC.AddFSPEC(NumberConverter.HexToBool(bytes[pointer++]));
                fspecLength++;
            }

            FSPEC = tempFSPEC;
            #region fields declare
            byte SAC = 0;
            byte SIC = 0;
            double TimeOfDay = 0;
            TargetReportDescriptor TargetReportDescriptorValue = new TargetReportDescriptor();
            PolarPosition PolarCoordinates = new PolarPosition();
            ModeA ModeACode = new ModeA();
            FlightLevel FlightLevelValue = new FlightLevel();
            string ICAOAddress = "";
            string AircraftID = "";
            ModeSCommB ModeSCommBData = new ModeSCommB();
            int TrackNumber = 0;
            CartesianPosition CartesianCoordinates = new CartesianPosition();
            PolarVelocity PolarVelocityValue = new PolarVelocity();
            TrackStatus TrackStatusValue = new TrackStatus();
            TransponderCapability CommunicationsACASCapability = new TransponderCapability();
            Mode1 Mode1Value = new Mode1();
            Mode2 Mode2Value = new Mode2();
            #endregion

            #region FSPEC-1 decoding
            //SIC SAC I048/010
            if (FSPEC[0])
            {
                SAC = Convert.ToByte(NumberConverter.HexToDecimal(bytes[pointer++]));
                SIC = Convert.ToByte(NumberConverter.HexToDecimal(bytes[pointer++]));
            }
            //Time of Day I048/140
            if (FSPEC[1])
            {
                TimeOfDay = NumberConverter.HexToDecimal(bytes[pointer] + bytes[pointer + 1] + bytes[pointer + 2]) /
                            (double)128;//в секундах
                pointer += 3;
            }
            //Target report descriptor I048/020
            if (FSPEC[2])
            {
                TargetReportDescriptorValue =
                    decodeTargetReportDescriptor(new[] { bytes[pointer], bytes[pointer + 1] });
                if (TargetReportDescriptorValue.Extended)
                    pointer++;
                pointer++;
            }
            //Measured position in polar coordinates I048/040
            if (FSPEC[3])
            {
                PolarCoordinates = decodePolarPosition(new[]
                    {bytes[pointer], bytes[pointer + 1], bytes[pointer + 2], bytes[pointer + 3]});
                pointer += 4;
            }
            //Mode 3-A code in octal representation I048/070
            if (FSPEC[4])
            {
                ModeACode = decodeModeA(new[] { bytes[pointer], bytes[pointer + 1] });
                pointer += 2;
            }
            //Flight level in binary representation I048/090
            if (FSPEC[5])
            {
                FlightLevelValue = decodeFlightLevel(new[] { bytes[pointer], bytes[pointer + 1] });
                pointer += 2;
            }

            if (FSPEC[6])
            {
                //field I048/130, не используется, но может быть большой длины
                throw new NotImplementedException("Subfield I048/130 occured! FSPEC-6");
            }
            #endregion

            #region FSPEC-2 decoding
            if (fspecLength > 1)
            {
                //Aircraft address I048/220
                if (FSPEC[8])
                {
                    ICAOAddress = bytes[pointer] + bytes[pointer + 1] + bytes[pointer + 2];
                    pointer += 3;
                }

                //Aircraft identification I048/240
                if (FSPEC[9])
                {
                    AircraftID = decodeAircraftID(new[]
                    {
                        bytes[pointer], bytes[pointer + 1], bytes[pointer + 2],
                        bytes[pointer + 3], bytes[pointer + 4], bytes[pointer + 5]
                    });
                    pointer += 6;
                }

                //Mode S Mode-B data I048/250
                if (FSPEC[10])
                {
                    int rep = NumberConverter.HexToDecimal(bytes[pointer]);
                    pointer++;//раскодируем поле REP и отсупаем на 1 байт
                    string[] repetitiveData = new string[rep * 8];
                    for (int i = 0, j = 0; i < rep; i++, j += 8)
                    {
                        repetitiveData[j] = bytes[pointer];
                        repetitiveData[j + 1] = bytes[pointer + 1];
                        repetitiveData[j + 2] = bytes[pointer + 2];
                        repetitiveData[j + 3] = bytes[pointer + 3];
                        repetitiveData[j + 4] = bytes[pointer + 4];
                        repetitiveData[j + 5] = bytes[pointer + 5];
                        repetitiveData[j + 6] = bytes[pointer + 6];
                        repetitiveData[j + 7] = bytes[pointer + 7];
                        pointer += 8;
                    }

                    ModeSCommBData = decodeModeSCommB(repetitiveData, rep);
                }

                //Track number I048/161
                if (FSPEC[11])
                {
                    TrackNumber =
                        NumberConverter.BoolArrayToDecimal(
                            NumberConverter.HexToBool(bytes[pointer] + bytes[pointer + 1]));
                    pointer += 2;
                }

                //Calculated position in cartesian coordinates I048/042
                if (FSPEC[12])
                {
                    CartesianCoordinates = decodeCartesianPosition(new[]
                    {
                        bytes[pointer],
                        bytes[pointer + 1],
                        bytes[pointer + 2],
                        bytes[pointer + 3]
                    });
                    pointer += 4;
                }

                //Calculated track velocity in polar coordinates I048/200
                if (FSPEC[13])
                {
                    PolarVelocityValue = decodePolarVelocity(new[]
                    {
                        bytes[pointer],
                        bytes[pointer + 1],
                        bytes[pointer + 2],
                        bytes[pointer + 3]
                    });
                    pointer += 4;
                }

                //Track status I048/170 не умеет раскодировать длинное поле, просто пропускает
                if (FSPEC[14])
                {
                    bool bigPacket = false;
                    TrackStatusValue = decodeTrackStatus(new[] { bytes[pointer], bytes[pointer + 1] }, out bigPacket);
                    if (bigPacket)
                        pointer++;
                    pointer++;
                }
            }
            #endregion

            #region FSPEC-3 decoding
            if (fspecLength > 2)
            {
                //Track quality I048/210
                if (FSPEC[16])
                {
                    pointer += 4;//поле фиксированной длины - 4 байта, у нас не используется
                }
                //Warning/Error Conditions I048/030
                if (FSPEC[17])
                {
                    //поле 1 или 2 байта, индикатор расширения стандартный, не используется
                    bool[] temp = NumberConverter.HexToBool(bytes[pointer]);
                    if (temp[temp.Length - 1])
                        pointer++;
                    pointer++;

                }
                //Mode-3/A Code Confidence Indicator I048/80
                if (FSPEC[18])
                {
                    //поле фиксированной длины 2 байта, не используется
                    pointer += 2;
                }
                //Mode-C Code and Confidence Indicator I048/100
                if (FSPEC[19])
                {
                    //поле фиксированной длины 4 байта, не используется
                    pointer += 4;
                }
                //Height Measured by 3D Radar I048/110
                if (FSPEC[20])
                {
                    //поле фиксированной длины 2 байта не испольуется
                    pointer += 2;
                }
                //Radial Doppler Speed I048/120
                if (FSPEC[21])
                {
                    //поле переменной длины, детальное описание в Астерикс
                    //у нас не используется
                    bool[] temp = NumberConverter.HexToBool(bytes[pointer]);
                    if (temp[0] && temp[1])//только одно из полей может быть активно!!!
                    {
                        throw new NotSupportedException(" Problem in I048/120 decoding!!!");
                    }

                    if (temp[0])
                        pointer += 3;
                    else if (temp[1])
                        pointer += 8;
                }
                //Communications/ACAS Capability and Flight Status I048/230
                if (FSPEC[22])
                {
                    //используется, поле фиксированной длины 2 байта
                    CommunicationsACASCapability =
                        decodeTransponderCapability(new[] { bytes[pointer], bytes[pointer + 1] });
                    pointer += 2;
                }
            }
            #endregion

            #region FSPEC-4 decoding
            if (fspecLength > 3)
            {
                //ACAS Resolution Advisory Report I048/260
                if (FSPEC[24])
                {
                    //поле фиксировано, 7 байт, не используется
                    pointer += 7;
                }
                //Mode-1 Code in Octal Representation I048/55
                if (FSPEC[25])
                {
                    //1 байт, присутствует согласно UAP, двузначный код полетного задания военных
                    Mode1Value = decodeMode1(bytes[pointer]);
                    pointer++;
                }
                //Mode-2 Code in Octal Representation I048/50
                if (FSPEC[26])
                {
                    //2 байта, присутствует согласно UAP, четырехзначный аналог сквока у военных
                    Mode2Value = decodeMode2(new[] { bytes[pointer], bytes[pointer + 1] });
                    pointer += 2;
                }
                //Mode-1 Code Confidence Indicator I048/65
                if (FSPEC[27])
                {
                    // фиксировано 1 байт, не используется
                    pointer++;
                }
                //Mode-2 Code Confidence Indicator I048/60
                if (FSPEC[28])
                {
                    //фиксировано 2 байта, не используется
                    pointer += 2;
                }
                //Special Purpose Field (SP)
                if (FSPEC[29])
                {
                    //если присутствует, надо обработать (пропустить) специальные поля
                    specialPurpose = true;
                }
                //Reserved Expansion Field (RE)
                if (FSPEC[30])
                {
                    //если присутствует, надо обработать (пропустить) зарезервированные поля
                    reservedExpansion = true;
                }
            }
            #endregion

            #region SP and RE decoding

            if (specialPurpose)
            {
                int length = NumberConverter.HexToDecimal(bytes[pointer]);
                pointer += length;
            }

            if (reservedExpansion)
            {
                int length = NumberConverter.HexToDecimal(bytes[pointer]);
                pointer += length;
            }

            #endregion

            Asterix[] output = new[]
            {
                new Cat48(SAC, SIC, TimeOfDay, TargetReportDescriptorValue, PolarCoordinates,
                    ModeACode, FlightLevelValue, ICAOAddress, AircraftID, ModeSCommBData, TrackNumber,
                    CartesianCoordinates, PolarVelocityValue, TrackStatusValue, CommunicationsACASCapability,
                    Mode1Value, Mode2Value)
            };
            if (pointer == bytes.Length)
            {
                return output;
            }
            else if (pointer > bytes.Length)
            {
                throw new NotSupportedException("pointer exceeded frame length!!!");
            }
            else
            {
                output = output.Add(Decode(bytes, pointer));
                return output;
            }
        }

        #region Методы-декодеры полей
        //разбирает поле TArgetReportDescriptor
        private TargetReportDescriptor decodeTargetReportDescriptor(string[] packet)
        {
            TargetReportType type;
            bool[] temp = NumberConverter.HexToBool(packet[0]);
            switch (NumberConverter.BoolArrayToDecimal(new[] { temp[0], temp[1], temp[2] }))
            {
                case 0:
                    type = TargetReportType.NoDetection;
                    break;
                case 1:
                    type = TargetReportType.SinglePSR;
                    break;
                case 2:
                    type = TargetReportType.SingleSSR;
                    break;
                case 3:
                    type = TargetReportType.SSR_PSR;
                    break;
                case 4:
                    type = TargetReportType.SingleModeSAllCall;
                    break;
                case 5:
                    type = TargetReportType.SingleModeSRollCall;
                    break;
                case 6:
                    type = TargetReportType.ModeSAllCall_PSR;
                    break;
                case 7:
                    type = TargetReportType.ModeSRollCall_PSR;
                    break;
                default://добавить обращение в лог или другую сигнализацию!!!
                    throw new NotImplementedException("Default block in cat48 decoder used!");
                    break;
            }

            if (temp[temp.Length - 1])//значит активен бит расширения поля
            {
                bool[] temp2 = NumberConverter.HexToBool(packet[1]);
                FriendOrFoe friendOrFoeValue;
                switch (NumberConverter.BoolArrayToDecimal(new[] { temp2[5], temp2[6] }))
                {
                    case 0:
                        friendOrFoeValue = FriendOrFoe.NoMode4;
                        break;
                    case 1:
                        friendOrFoeValue = FriendOrFoe.Friendly;
                        break;
                    case 2:
                        friendOrFoeValue = FriendOrFoe.Unknown;
                        break;
                    case 3:
                        friendOrFoeValue = FriendOrFoe.NoReply;
                        break;
                    default:
                        throw new NotImplementedException("Default block in cat48 decoder used!");
                        break;
                }

                return new TargetReportDescriptor(type, temp[3], temp[4], temp[5], temp[6], temp2[0], temp2[3], temp2[4], friendOrFoeValue, true);
            }

            return new TargetReportDescriptor(type, temp[3], temp[4], temp[5], temp[6], false, false, false, FriendOrFoe.Unknown, false);
        }
        //разбирает поле PolarPosition
        private PolarPosition decodePolarPosition(string[] packet)
        {
            double distance = NumberConverter.HexToDecimal(packet[0] + packet[1]) / (double)256;
            double azimuth = NumberConverter.HexToDecimal(packet[2] + packet[3]) * (360 / Math.Pow(2, 16));
            return new PolarPosition(azimuth, distance);
        }
        //разбирает поле ModeA
        private ModeA decodeModeA(string[] packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet[0] + packet[1]);
            bool notValid = temp[0];
            bool garbled = temp[1];
            bool lost = temp[2];
            int squawk = (NumberConverter.BoolArrayToDecimal(new[] { temp[4], temp[5], temp[6] }) * 1000 +
                          NumberConverter.BoolArrayToDecimal(new[] { temp[7], temp[8], temp[9] }) * 100 +
                          NumberConverter.BoolArrayToDecimal(new[] { temp[10], temp[11], temp[12] }) * 10 +
                          NumberConverter.BoolArrayToDecimal(new[] { temp[13], temp[14], temp[15] }));

            return new ModeA(notValid, garbled, lost, squawk);
        }
        //разбирает поле flightlevel
        private FlightLevel decodeFlightLevel(string[] packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            bool notValid = temp[0];
            bool garbled = temp[1];
            //in 1|4 flightlevel
            int level = NumberConverter.BoolArrayToDecimal(temp.GetSubArray(2, 15));
            return new FlightLevel(notValid, garbled, level);
        }
        //разбирает поле aircraft id
        private string decodeAircraftID(string[] packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            string output = "";
            for (int i = 0; i < temp.Length; i += 6) //кодировка по 6 символов
            {
                output += CodingRules.decodeIA5(NumberConverter.BoolArrayToDecimal(temp.GetSubArray(i, i + 5)));
            }

            return output;
        }
        //разбирает поле Mode-S ModeB data
        private ModeSCommB decodeModeSCommB(string[] repetitiveData, int rep)
        {
            CommBData[] data = new CommBData[rep];
            for (int i = 0, j = 0; i < rep; i++, j += 8)
            {
                string commbData;
                string bdsCode;
                commbData = repetitiveData.MakeString(j, j + 6);
                bool[] temp = NumberConverter.HexToBool(repetitiveData[j + 7]);
                bdsCode = NumberConverter.BoolArrayToDecimal(temp.GetSubArray(0, 3)).ToString() +
                                  NumberConverter.BoolArrayToDecimal(temp.GetSubArray(4, 7)).ToString();
                data[i] = new CommBData(commbData, bdsCode);
            }

            return new ModeSCommB(rep, data);
        }
        //разбирает поле cartesian coordinates
        private CartesianPosition decodeCartesianPosition(string[] packet)
        {
            bool[] tempX = NumberConverter.HexToBool(packet[0] + packet[1]);
            bool[] tempY = NumberConverter.HexToBool(packet[2] + packet[3]);
            double x;
            double y;
            if (tempX[0])
            {
                x = CodingRules.decodeNegativeBinary(tempX);
            }
            else
            {
                x = NumberConverter.BoolArrayToDecimal(tempX) / (double)128; //in NM
            }
            if (tempY[0])
            {
                y = CodingRules.decodeNegativeBinary(tempY);
            }
            else
            {
                y = NumberConverter.BoolArrayToDecimal(tempY) / (double)128;
            }

            return new CartesianPosition(x, y);
        }
        //разбирает поле polar velocity
        private PolarVelocity decodePolarVelocity(string[] packet)
        {
            double speed = (NumberConverter.BoolArrayToDecimal(NumberConverter.HexToBool(packet[0] + packet[1])) *
                Math.Pow(2, -14)) * 3600; //in knots
            double heading = NumberConverter.BoolArrayToDecimal((NumberConverter.HexToBool(packet[2] + packet[3]))) *
                             (360 / Math.Pow(2, 16));

            return new PolarVelocity(speed, heading);
        }
        //разбирает поле track status
        private TrackStatus decodeTrackStatus(string[] packet, out bool bigPacket)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            bool tentative = temp[0];
            SensorMaintainingTrack rad;
            switch (NumberConverter.BoolArrayToDecimal(new[] { temp[1], temp[2] }))
            {
                case 0:
                    rad = SensorMaintainingTrack.Combined;
                    break;
                case 1:
                    rad = SensorMaintainingTrack.PSR;
                    break;
                case 2:
                    rad = SensorMaintainingTrack.SSR_ModeS;
                    break;
                case 3:
                    rad = SensorMaintainingTrack.Invalid;
                    break;
                default:
                    throw new NotImplementedException("default block in trackstatus fired!");
                    break;
            }
            bool dou = temp[3];
            bool mah = temp[4];
            ClimbOrDescend cdm;
            switch (NumberConverter.BoolArrayToDecimal(new[] { temp[5], temp[6] }))
            {
                case 0:
                    cdm = ClimbOrDescend.Maintaining;
                    break;
                case 1:
                    cdm = ClimbOrDescend.Climbing;
                    break;
                case 2:
                    cdm = ClimbOrDescend.Descending;
                    break;
                case 3:
                    cdm = ClimbOrDescend.Invalid;
                    break;
                default:
                    throw new NotImplementedException("default block in trackstatus fired!");
                    break;
            }

            bigPacket = false;
            bool endOfTrack = false;
            if (temp[7])
            {
                bigPacket = true;
                endOfTrack = temp[9];
            }
            return new TrackStatus(tentative, rad, dou, mah, cdm, endOfTrack);
        }
        //разбирает поле transponder capability
        private TransponderCapability decodeTransponderCapability(string[] packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            CommunicationCapabilityOfTransponder com;
            switch (NumberConverter.BoolArrayToDecimal(temp.GetSubArray(0, 2)))
            {
                case 0:
                    com = CommunicationCapabilityOfTransponder.NoCapability;
                    break;
                case 1:
                    com = CommunicationCapabilityOfTransponder.CommA_CommB;
                    break;
                case 2:
                    com = CommunicationCapabilityOfTransponder.CommA_CommB_UplinkELM;
                    break;
                case 3:
                    com = CommunicationCapabilityOfTransponder.CommA_CommB_UplinkDownlinkELM;
                    break;
                case 4:
                    com = CommunicationCapabilityOfTransponder.Level5Capability;
                    break;
                case 5:
                    com = CommunicationCapabilityOfTransponder.NoCapability;
                    break;
                case 6:
                    com = CommunicationCapabilityOfTransponder.NoCapability;
                    break;
                case 7:
                    com = CommunicationCapabilityOfTransponder.NoCapability;
                    break;
                default:
                    com = CommunicationCapabilityOfTransponder.NoCapability;
                    throw new NotImplementedException("default in I048/230");
                    break;
            }
            FlightStatus stat;
            switch (NumberConverter.BoolArrayToDecimal(temp.GetSubArray(3, 5)))
            {
                case 0:
                    stat = FlightStatus.NoAlert_NoSPI_Airborne;
                    break;
                case 1:
                    stat = FlightStatus.NoAlert_NoSPI_Ground;
                    break;
                case 2:
                    stat = FlightStatus.Alert_NoSPI_Airborne;
                    break;
                case 3:
                    stat = FlightStatus.Alert_NoSPI_Ground;
                    break;
                case 4:
                    stat = FlightStatus.Alert_SPI_All;
                    break;
                case 5:
                    stat = FlightStatus.NoAlert_SPI_All;
                    break;
                case 6:
                    stat = FlightStatus.NoAlert_NoSPI_Airborne;
                    break;
                case 7:
                    stat = FlightStatus.NoAlert_NoSPI_Airborne;
                    break;
                default:
                    stat = FlightStatus.NoAlert_NoSPI_Airborne;
                    throw new NotImplementedException("default in I048/230");
                    break;
            }
            bool si = temp[6];
            bool mssc = temp[8];
            bool arc = temp[9];
            bool aic = temp[10];
            bool b1a = temp[11];
            string b1b = NumberConverter.BoolArrayToString(temp.GetSubArray(12, 15));

            return new TransponderCapability(com, stat, si, mssc, arc, aic, b1a, b1b);
        }
        //разбирает поле Mode-1
        private Mode1 decodeMode1(string packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            bool notValid = temp[0];
            bool garbled = temp[1];
            bool lost = temp[2];
            byte code = (byte)(NumberConverter.BoolArrayToDecimal(temp.GetSubArray(3, 5)) * 10 +
                               NumberConverter.BoolArrayToDecimal(temp.GetSubArray(6, 7)));
            return new Mode1(notValid, garbled, lost, code);
        }
        //разбрает поле Mode-2
        private Mode2 decodeMode2(string[] packet)
        {
            bool[] temp = NumberConverter.HexToBool(packet);
            bool notValid = temp[0];
            bool garbled = temp[1];
            bool lost = temp[2];
            int code = NumberConverter.BoolArrayToDecimal(temp.GetSubArray(4, 6)) * 1000 +
                       NumberConverter.BoolArrayToDecimal(temp.GetSubArray(7, 9)) * 100 +
                       NumberConverter.BoolArrayToDecimal(temp.GetSubArray(10, 12)) * 10 +
                       NumberConverter.BoolArrayToDecimal(temp.GetSubArray(13, 15));

            return new Mode2(notValid, garbled, lost, code);
        }
        #endregion
    }
}
