using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
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
        public static RadarScreenCell[,,] PPI_Azimuth_15_Range_5;


        private static List<Flight> TrackTable;

        static PPI()
        {
            PPI_Azimuth_15_Range_20 = new RadarScreenCell[24, 25, 10];
            PPI_Azimuth_15_Range_25 = new RadarScreenCell[24, 20, 10];
            PPI_Azimuth_10_Range_20 = new RadarScreenCell[36, 25, 10];
            PPI_Azimuth_10_Range_25 = new RadarScreenCell[36, 20, 10];
            PPI_Azimuth_20_Range_20 = new RadarScreenCell[18, 25, 10];
            PPI_Azimuth_20_Range_25 = new RadarScreenCell[18, 20, 10];
            PPI_Azimuth_15_Range_5 = new RadarScreenCell[24, 13, 10];//max range 65, other 500
            //Azimuth_15_Range_20 Fill
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
            //Azimuth_15_Range_25 Fill
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth10_Range_20 Fill
            for (int x = 0; x < 36; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_10_Range_20[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_10_Range_25 Fill
            for (int x = 0; x < 36; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_10_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_20_Range_20 Fill
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_20_Range_20[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_20_Range_25 Fill
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_20_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_15_Range5 Fill
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 13; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_5[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }

            TrackTable = new List<Flight>();
        }

        //заново инициализирует массивы для хранения данных отображения и таблицу треков
        public static void ReinitializePPI()
        {
            PPI_Azimuth_15_Range_20 = new RadarScreenCell[24, 25, 10];
            PPI_Azimuth_15_Range_25 = new RadarScreenCell[24, 20, 10];
            PPI_Azimuth_10_Range_20 = new RadarScreenCell[36, 25, 10];
            PPI_Azimuth_10_Range_25 = new RadarScreenCell[36, 20, 10];
            PPI_Azimuth_20_Range_20 = new RadarScreenCell[18, 25, 10];
            PPI_Azimuth_20_Range_25 = new RadarScreenCell[18, 20, 10];
            PPI_Azimuth_15_Range_5 = new RadarScreenCell[24, 13, 10];//max range 65, other 500
            //Azimuth_15_Range_20 Fill
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
            //Azimuth_15_Range_25 Fill
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth10_Range_20 Fill
            for (int x = 0; x < 36; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_10_Range_20[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_10_Range_25 Fill
            for (int x = 0; x < 36; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_10_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_20_Range_20 Fill
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 25; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_20_Range_20[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_20_Range_25 Fill
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_20_Range_25[x, y, z] = new RadarScreenCell(new Key(x, y, z));
                    }
                }
            }
            //Azimuth_15_Range5 Fill
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 13; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        PPI_Azimuth_15_Range_5[x, y, z] = new RadarScreenCell(new Key(x, y, z));
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
            RecalculateAllCells();
            
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
                        PlaceToTable(polarPosition.Distance <= 62, PSRdetection, true, polarPosition, TrackTable[i].Altitude);
                        return;
                    }
                    else//если это был пропуск цели
                    {
                        if (TrackTable[i].LostCount == 2)
                        {
                            indexesForDelete.Add(i);
                            PlaceToTable(polarPosition.Distance <= 62, false, false, TrackTable[i].Position, TrackTable[i].Altitude);
                        }
                        else
                        {
                            TrackTable[i].LostCount++;
                            PlaceToTable(polarPosition.Distance <= 62, false, false, TrackTable[i].Position, TrackTable[i].Altitude);
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
                PlaceToTable(polarPosition.Distance<=62, PSRdetection, true, polarPosition, altitude);
            }

        }
        
        private static void PlaceToTable(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            PolarPosition polarPosition, int FlightLevel)
        {
            double NM = 1.852; //nautical mile, for calculation (asterix uses miles, screen in kilometers)
            double convertedDistance = polarPosition.Distance * NM;
            //range exceeds calculation value
            if(polarPosition.Distance>500)
                return;
            PlaceToTableA15R20(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA15R25(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA10R20(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA10R25(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA20R20(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA20R25(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
            PlaceToTableA15R5(inPSRZone, detectedPSR, detectedSSR, polarPosition.Azimuth, convertedDistance,
                FlightLevel);
        }
        //add methods when scales added
        //Azimuth_15_Range_20
        private static void PlaceToTableA15R20(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 15);
            int rg = Convert.ToInt32(range / 20);
            //due to specific of converter methods
            if (az == 24)
                az = 23;
            if (rg == 25)
                rg = 24;
            int fl = FlightLevel / 200; // единица высоты - 1/4 FL, а так же разрешение ячеек по 50 FL
            if (detectedSSR)
            {
                PPI_Azimuth_15_Range_20[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_15_Range_20[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_15_Range_20[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_15_Range_20[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_15_Range_25
        private static void PlaceToTableA15R25(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 15);
            int rg = Convert.ToInt32(range / 25);
            //due to specific of converter methods
            if (az == 24)
                az = 23;
            if (rg == 20)
                rg = 19;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_15_Range_25[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_15_Range_25[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_15_Range_25[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_15_Range_25[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_10_Range_20
        private static void PlaceToTableA10R20(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 10);
            int rg = Convert.ToInt32(range / 20);
            //due to specific of converter methods
            if (az == 36)
                az = 35;
            if (rg == 25)
                rg = 24;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_10_Range_20[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_10_Range_20[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_10_Range_20[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_10_Range_20[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_10_Range_25
        private static void PlaceToTableA10R25(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 10);
            int rg = Convert.ToInt32(range / 25);
            //due to specific of converter methods
            if (az == 36)
                az = 35;
            if (rg == 20)
                rg = 19;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_10_Range_25[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_10_Range_25[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_10_Range_25[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_10_Range_25[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_20_Range_20
        private static void PlaceToTableA20R20(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 20);
            int rg = Convert.ToInt32(range / 20);
            //due to specific of converter methods
            if (az == 18)
                az = 17;
            if (rg == 25)
                rg = 24;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_20_Range_20[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_20_Range_20[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_20_Range_20[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_20_Range_20[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_20_Range_25
        private static void PlaceToTableA20R25(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            int az = Convert.ToInt32(azimuth / 20);
            int rg = Convert.ToInt32(range / 25);
            //due to specific of converter methods
            if (az == 18)
                az = 17;
            if (rg == 20)
                rg = 19;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_20_Range_25[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_20_Range_25[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_20_Range_25[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_20_Range_25[az, rg, fl].PSRNoDetection();
                }
            }
        }
        //Azimuth_15_Range_5
        private static void PlaceToTableA15R5(bool inPSRZone, bool detectedPSR, bool detectedSSR,
            double azimuth, double range, int FlightLevel)
        {
            //
            if(range>65)
                return;
            int az = Convert.ToInt32(azimuth / 15);
            int rg = Convert.ToInt32(range / 5);
            //due to specific of converter methods
            if (az == 24)
                az = 23;
            if (rg == 13)
                rg = 12;
            int fl = FlightLevel / 200;
            if (detectedSSR)
            {
                PPI_Azimuth_15_Range_5[az, rg, fl].SSRDetection();
            }
            else
            {
                PPI_Azimuth_15_Range_5[az, rg, fl].SSRNoDetection();
            }

            if (inPSRZone)
            {
                if (detectedPSR)
                {
                    PPI_Azimuth_15_Range_5[az, rg, fl].PSRDetection();
                }
                else
                {
                    PPI_Azimuth_15_Range_5[az, rg, fl].PSRNoDetection();
                }
            }
        }

        //gets value from RadarCell in selected Scale
        public static RadarScreenCell GetCell(int azimuthResolution, int rangeResolution, int azimuth, int range,
            int flightLevel)
        {
            try
            {
                switch (rangeResolution)
                {
                    case 5:
                        return PPI_Azimuth_15_Range_5[azimuth, range, flightLevel];
                        break;
                    case 20:
                        switch (azimuthResolution)
                        {
                            case 10:
                                return PPI_Azimuth_10_Range_20[azimuth, range, flightLevel];
                                break;
                            case 15:
                                return PPI_Azimuth_15_Range_20[azimuth, range, flightLevel];
                                break;
                            case 20:
                                return PPI_Azimuth_20_Range_20[azimuth, range, flightLevel];
                                break;
                            default:
                                throw new NotImplementedException("wrong call to PPI arrays");
                                break;
                        }
                        break;
                    case 25:
                        switch (azimuthResolution)
                        {
                            case 10:
                                return PPI_Azimuth_10_Range_25[azimuth, range, flightLevel];
                                break;
                            case 15:
                                return PPI_Azimuth_15_Range_25[azimuth, range, flightLevel];
                                break;
                            case 20:
                                return PPI_Azimuth_20_Range_25[azimuth, range, flightLevel];
                                break;
                            default:
                                throw new NotImplementedException("wrong call to PPI arrays");
                            break;
                        }
                        break;
                    default:
                        throw new NotImplementedException("wrong call to PPI arrays");
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("wrong call to PPI arrays");
            }
        }
        //recalculates PR in all cells of all scales
        private static void RecalculateAllCells()
        {
            //Azimuth_15_Range_20
            foreach (RadarScreenCell cell in PPI_Azimuth_15_Range_20)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_15_Range_25
            foreach (RadarScreenCell cell in PPI_Azimuth_15_Range_25)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_10_Range_20
            foreach (RadarScreenCell cell in PPI_Azimuth_10_Range_20)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_10_Range_25
            foreach (RadarScreenCell cell in PPI_Azimuth_10_Range_25)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_20_Range_20
            foreach (RadarScreenCell cell in PPI_Azimuth_20_Range_20)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_20_Range_25
            foreach (RadarScreenCell cell in PPI_Azimuth_20_Range_25)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
            //Azimuth_15_Range_5
            foreach (RadarScreenCell cell in PPI_Azimuth_15_Range_5)
            {
                if (cell != null)
                    cell.ReCalculatePR();
            }
        }
    }
}
