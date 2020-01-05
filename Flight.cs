using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    class Flight
    {
        public int TrackNumber { get; }
        public int Squawk { get; set; }
        public string ICaoAddress { get; set; }
        public string AircraftId { get; set; }
        public int Altitude { get; set; }
        public double LastTimeOfDay { get; set; }
        public byte LostCount { get; set; }
        public PolarPosition Position { get; set; }

        public Flight(int trackNumber, int squawk, string icaoAddress, string aircraftId, int altitude,
            double lastTimeOfDay, PolarPosition polarPosition)
        {
            TrackNumber = trackNumber;
            Squawk = squawk;
            ICaoAddress = icaoAddress;
            AircraftId = (aircraftId.Contains("@")) ? "" : aircraftId;
            Altitude = altitude;
            LastTimeOfDay = lastTimeOfDay;
            LostCount = 0;
            Position = polarPosition;
        }

        public void UpdateFlightData(int squawk, string icaoAddress, string aircraftId, int altitude,
            double lastTimeOfDay, PolarPosition polarPosition)
        {
            if (Squawk == 0)
                Squawk = squawk;
            if (ICaoAddress == "")
                ICaoAddress = icaoAddress;
            if (AircraftId == "")
                AircraftId = (aircraftId.Contains("@")) ? "" : aircraftId;
            if (altitude != 0)
                Altitude = altitude;
            if (lastTimeOfDay > 0)
                LastTimeOfDay = lastTimeOfDay;
            //возможно ли пустое??
            Position = polarPosition;
        }
    }
}
