using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    enum Cat34MessageType
    {
        North = 1,
        SectorCross = 2,
        Error = 15
    }
    class Cat34 : Asterix
    {
        public byte SAC { get; }
        public byte SIC { get; }
        public Cat34MessageType MessageType { get; }
        public double TimeOfDay { get; }
        public double SectorNumber { get; }
        public double AntennaRotationPeriod { get; }

        public Cat34(byte SAC, byte SIC, Cat34MessageType messageType, double timeOfDay, double sectorNumber,
            double antennaRotationPeriod) : base(34)
        {

            this.SAC = SAC;
            this.SIC = SIC;
            this.MessageType = messageType;
            this.TimeOfDay = timeOfDay;
            this.SectorNumber = sectorNumber;
            this.AntennaRotationPeriod = antennaRotationPeriod;
        }
    }
}
