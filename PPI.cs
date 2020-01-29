using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CARD_Probability
{
    static class PPI
    {
        //1st Azimuth(clockwise from 0), 2nd Range(from close), 3rd Altitude(from lowest)
        public static RadarScreenCell[,,] PPI_Azimuth_15_Range_20;
        public static RadarScreenCell[,,] PPI_Azimuth_15_Range_25;
        public static RadarScreenCell[,,] PPI_Azimuth_10_Range_20;
        public static RadarScreenCell[,,] PPI_Azimuth_10_Range_25;
        public static RadarScreenCell[,,] PPI_Azimuth_20_Range_20;
        public static RadarScreenCell[,,] PPI_Azimuth_20_Range_25;
        public static RadarScreenCell[,,] PPI_Azimuth_15_Range5;


        private static List<Flight> TrackTable;

        static PPI()
        {
            PPI_Azimuth_15_Range_20 = new RadarScreenCell[24, 25, 10];
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_20[x, y, z] = new RadarScreenCell(new Key(x,y,z));
                    }
                }
            }

            TrackTable = new List<Flight>();
        }

        //заново инициализирует массивы для хранения данных отображения и таблицу треков
        public static void ReinitializePPI()
        {
            PPI_Azimuth_15_Range_20 = new RadarScreenCell[24, 25, 10];
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_20[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }

            TrackTable = new List<Flight>();
        }
        public static void ProcessPackets(List<Asterix> input)
        {
            var packets48 = from packet in input
                            where packet.Category == 48
                            select packet;

            foreach (Asterix asterix in packets48)
            {
                Cat48 temp = asterix as Cat48;
                Tracking(temp.TrackNumber, temp.ModeACode.Squawk, temp.ICAOAddress, temp.AircraftID,
                    temp.FlightLevelValue.Level, temp.TimeOfDay, temp.PolarCoordinates, temp.TargetReportDescriptorValue,
                    temp.TargetReportDescriptorValue.Type != TargetReportType.NoDetection);

            }
            //пересчитываем вероятность по всем ячейкам
            foreach (RadarScreenCell cell in PPI_Azimuth_15_Range_20)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
        }

        public static RadarScreenCell SelectByKey(Key key)
        {
            return PPI_Azimuth_15_Range_20[key.Azimuth, key.Range, key.Altitude];
        }
        private static void Tracking(int trackNumber, int squawk, string icaoAddress, string aircraftId, int altitude,
            double lastTimeOfDay, PolarPosition polarPosition,TargetReportDescriptor targetReport, bool detected)
        {
            //любой трек от RHP (категория 48) должен иметь трек, но для идентификации нужно хотя бы еще одно значение
            if (trackNumber == 0 && (squawk == 0 && icaoAddress == "" && (aircraftId == "" || aircraftId.Contains("@"))))
            {
                //такой пакет не обрабатываем
                return;
            }
            //также отбрасываем треки контрольных ответчиков и те, высота котрых превышает лимит рассчета (50000 футов)
            if (trackNumber == 7777 || trackNumber == 7776 || altitude >= 2000)
            {
                return;
            }
            //переделать, удалить внутри цикла??
            List<int> indexesForDelete = new List<int>();

            for (int i = 0; i < TrackTable.Count; i++)
            {
                //ищем в таблице трекинга совпадение пары значений
                if ((TrackTable[i].TrackNumber == trackNumber && TrackTable[i].Squawk == squawk) ||
                    (TrackTable[i].TrackNumber == trackNumber && TrackTable[i].ICaoAddress == icaoAddress) ||
                    (TrackTable[i].TrackNumber == trackNumber && TrackTable[i].AircraftId == aircraftId))
                {
                    //если обрабатываемый пакет был с обнаружением
                    if (detected)
                    {
                        TrackTable[i].UpdateFlightData(squawk, icaoAddress, aircraftId, altitude, lastTimeOfDay, polarPosition);
                        //обнуляем счетчик потерянных
                        TrackTable[i].LostCount = 0;
                        bool PSRdetection = (targetReport.Type == TargetReportType.ModeSAllCall_PSR) ||
                                            (targetReport.Type == TargetReportType.ModeSRollCall_PSR) || (targetReport.Type == TargetReportType.SSR_PSR);
                        PlaceToTable(polarPosition.Distance <= 62, PSRdetection, true, PPIResolution.Azimuth_15_Range_20, polarPosition, TrackTable[i].Altitude);
                        return;
                    }
                    else//если это был пропуск цели
                    {
                        if (TrackTable[i].LostCount == 2)
                        {
                            indexesForDelete.Add(i);
                            PlaceToTable(polarPosition.Distance <= 62, false, false, PPIResolution.Azimuth_15_Range_20, TrackTable[i].Position, TrackTable[i].Altitude);
                        }
                        else
                        {
                            TrackTable[i].LostCount++;
                            PlaceToTable(polarPosition.Distance <= 62, false, false, PPIResolution.Azimuth_15_Range_20, TrackTable[i].Position, TrackTable[i].Altitude);
                        }
                        return;
                    }
                }
            }
            //удаляем все треки,где значение пропаданий 3
            foreach (int i in indexesForDelete)
            {
                TrackTable.RemoveAt(i);
            }
            //если ничего не нашли, надо добавить новый трек в таблицу
            if (detected)
            {
                TrackTable.Add(new Flight(trackNumber, squawk, icaoAddress, aircraftId, altitude, lastTimeOfDay,
                    polarPosition));
                bool PSRdetection = (targetReport.Type == TargetReportType.ModeSAllCall_PSR) ||
                                    (targetReport.Type == TargetReportType.ModeSRollCall_PSR) || (targetReport.Type == TargetReportType.SSR_PSR);
                PlaceToTable(polarPosition.Distance<=62, PSRdetection, true, PPIResolution.Azimuth_15_Range_20, polarPosition, altitude);
            }

        }
        //пока не принимает значений масштаба, установлен вручную
        private static void PlaceToTable(bool inPSRZone, bool detectedPSR, bool detectedSSR, PPIResolution ppiResolution, PolarPosition polarPosition, int FlightLevel)
        {
            double NM = 1.852; //морская миля, пересчет для отображения (экран в километрах, астерикс в милях)
            double convertedDistance = polarPosition.Distance * NM;
            switch (ppiResolution)
            {
                case PPIResolution.Azimuth_15_Range_20:
                    {
                        int azimuth = Convert.ToInt32(polarPosition.Azimuth / 15);
                        int range = Convert.ToInt32(convertedDistance / 20);
                        //костыль, переделать!
                        if (azimuth == 24)
                            azimuth = 23;
                        if (range == 25)
                            range = 24;
                        int flightLevel = FlightLevel / 200;// единица высоты - 1/4 FL, а так же разрешение ячеек по 50 FL
                        if (detectedSSR)
                        {
                            PPI_Azimuth_15_Range_20[azimuth, range, flightLevel].SSRDetection();
                        }
                        else
                        {
                            PPI_Azimuth_15_Range_20[azimuth, range, flightLevel].SSRNoDetection();
                        }

                        if (inPSRZone)
                        {
                            if (detectedPSR)
                            {
                                PPI_Azimuth_15_Range_20[azimuth,range,flightLevel].PSRDetection();
                            }
                            else
                            {
                                PPI_Azimuth_15_Range_20[azimuth,range,flightLevel].PSRNoDetection();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //дополнить при увеличении количества масштабов
        enum PPIResolution
        {
            Azimuth_10_Range_20,
            Azimuth_15_Range_20,
            Azimuth_20_Range_20
        }
    }
}
