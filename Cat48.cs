using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CARD_Probability
{
    class Cat48 : Asterix
    {
        public byte SAC { get; }
        public byte SIC { get; }
        public double TimeOfDay { get; }
        public TargetReportDescriptor TargetReportDescriptorValue { get; }
        public PolarPosition PolarCoordinates { get; }
        public ModeA ModeACode { get; }
        public FlightLevel FlightLevelValue { get; }
        public string ICAOAddress { get; }
        public string AircraftID { get; }
        public ModeSCommB ModeSCommBData { get; }
        public int TrackNumber { get; }
        public CartesianPosition CartesianCoordinates { get; }
        public PolarVelocity PolarVelocityValue { get; }
        public TrackStatus TrackStatusValue { get; }
        public TransponderCapability CommunicationsACASCapability { get; }
        public Mode1 Mode1Value { get; }
        public Mode2 Mode2Value { get; }

        public Cat48(byte SAC, byte SIC, double TimeOfDay, TargetReportDescriptor TargetReportDescriptorValue,
            PolarPosition PolarCoordinates, ModeA ModeACode,
            FlightLevel FlightLevelValue, string ICAOAddress, string AircraftID, ModeSCommB ModeSCommBData,
            int TrackNumber, CartesianPosition CartesianCoordinates,
            PolarVelocity PolarVelocityValue, TrackStatus TrackStatusValue,
            TransponderCapability CommunicationsACASCapability, Mode1 Mode1Value, Mode2 Mode2Value) : base(48)
        {
            this.SAC = SAC;
            this.SIC = SIC;
            this.TimeOfDay = TimeOfDay;
            this.TargetReportDescriptorValue = TargetReportDescriptorValue;
            this.PolarCoordinates = PolarCoordinates;
            this.ModeACode = ModeACode;
            this.FlightLevelValue = FlightLevelValue;
            this.ICAOAddress = ICAOAddress;
            this.AircraftID = AircraftID;
            this.ModeSCommBData = ModeSCommBData;
            this.TrackNumber = TrackNumber;
            this.CartesianCoordinates = CartesianCoordinates;
            this.PolarVelocityValue = PolarVelocityValue;
            this.TrackStatusValue = TrackStatusValue;
            this.CommunicationsACASCapability = CommunicationsACASCapability;
            this.Mode1Value = Mode1Value;
            this.Mode2Value = Mode2Value;
        }
    }
    #region Структуры

    struct Mode2
    {
        public bool NotValid { get; }
        public bool Garbled { get; }
        public bool Lost { get; }//1 - smoothed by tracker, 0 - from transponder
        public int Code { get; }//двухзначный код, отражает полетное задание

        public Mode2(bool notValid, bool garbled, bool lost, int code)
        {
            NotValid = notValid;
            Garbled = garbled;
            Lost = lost;
            Code = code;
        }
    }
    struct Mode1
    {
        public bool NotValid { get; }
        public bool Garbled { get; }
        public bool Lost { get; }//1 - smoothed by tracker, 0 - from transponder
        public byte Code { get; }//двухзначный код, отражает полетное задание

        public Mode1(bool notValid, bool garbled, bool lost, byte code)
        {
            NotValid = notValid;
            Garbled = garbled;
            Lost = lost;
            Code = code;
        }
    }
    struct TransponderCapability
    {
        public CommunicationCapabilityOfTransponder COM { get; }
        public FlightStatus STAT { get; }
        public bool IICapable { get; }//0 - SI capable
        public bool ModeSSpecificService { get; }//1 - capable
        public bool AltitudeResolution { get; }//1 - 25ft, 0 - 100ft
        public bool AircraftIdentification { get; }//1 - capable
        public bool B1A { get; }// BDS 1,0 bit 16
        public string B1B { get; }//BDS 1,0 bits 37-40, сделал строку, в КАРД декодируют в десятичное число

        public TransponderCapability(CommunicationCapabilityOfTransponder com, FlightStatus sat,
            bool si, bool mssc, bool arc, bool aic, bool b1a, string b1b)
        {
            COM = com;
            STAT = sat;
            IICapable = si;
            ModeSSpecificService = mssc;
            AltitudeResolution = arc;
            AircraftIdentification = aic;
            B1A = b1a;
            B1B = b1b;
        }
    }
    struct TrackStatus
    {
        public bool Tentative { get; }//0 - Confirmed, 1 - Tentative
        public SensorMaintainingTrack TypeOfSensor { get; }
        public bool LowConfidenceToAssociation { get; }//1-Low confidence, 0 - normal (plot to track association)
        public bool HorizontalManoeuvre { get; }//1 - sensed, 0 - not
        public ClimbOrDescend VerticalManoeuvre { get; }
        public bool EndOfTrack { get; }//1 - ending, 0 - alive

        public TrackStatus(bool cnf, SensorMaintainingTrack rad,
            bool dou, bool mah, ClimbOrDescend cdm, bool tre)
        {
            Tentative = cnf;
            TypeOfSensor = rad;
            LowConfidenceToAssociation = dou;
            HorizontalManoeuvre = mah;
            VerticalManoeuvre = cdm;
            EndOfTrack = tre;
        }
    }
    struct PolarVelocity
    {
        public double GroundSpeed { get; }//in knots
        public double Heading { get; }//in degrees

        public PolarVelocity(double speed, double heading)
        {
            GroundSpeed = speed;
            Heading = heading;
        }
    }
    struct CartesianPosition
    {
        public double XComponent { get; }
        public double YComponent { get; }

        public CartesianPosition(double xComponent, double yComponent)
        {
            XComponent = xComponent;
            YComponent = yComponent;
        }
    }
    struct ModeSCommB
    {
        public int REP { get; }//repetition factor, присутствует всегда
        public CommBData[] Data { get; }

        public ModeSCommB(int rep, CommBData[] data)
        {
            REP = rep;
            Data = data;
        }
    }//не разобрался с назначением полей, всегда REP=1 если вообще есть поле
    struct FlightLevel
    {
        public bool NotValid { get; }
        public bool Garbled { get; }
        public int Level { get; }//in 1/4 of FL

        public FlightLevel(bool notvalid, bool garbled, int level)
        {
            NotValid = notvalid;
            Garbled = garbled;
            Level = level;
        }
    }
    struct ModeA
    {
        public bool NotValid { get; }
        public bool Garbled { get; }
        public bool Lost { get; }//1 - not derived from last scan
        public int Squawk { get; }//in octal

        public ModeA(bool notValid, bool garbled, bool lost, int squawk)
        {
            NotValid = notValid;
            Garbled = garbled;
            Lost = lost;
            Squawk = squawk;
        }
    }
    struct PolarPosition
    {
        public double Azimuth { get; }
        public double Distance { get; }

        public PolarPosition(double azimuth, double distance)
        {
            Azimuth = azimuth;
            Distance = distance;
        }
    }
    struct TargetReportDescriptor
    {
        public TargetReportType Type { get; }
        public bool Simulated { get; }//true - simulated, false - actual
        public bool RDP { get; }//false - RHP1, true - RHP2
        public bool SPI { get; }//true - present
        public bool FixedTransponder { get; }//false - aircraft
        public bool TestTarget { get; }//false - real
        public bool MilitaryEmergency { get; }//true - ME
        public bool MilitaryIdentification { get; }//true - MI
        public FriendOrFoe FriendFoe { get; }
        public bool Extended { get; }

        public TargetReportDescriptor(TargetReportType type, bool simulated, bool rdp,
            bool spi, bool fixedTransponder, bool testTarget, bool militaryEmergency,
            bool militaryIdentification, FriendOrFoe friendFoe, bool extended)
        {
            Type = type;
            Simulated = simulated;
            RDP = rdp;
            SPI = spi;
            FixedTransponder = fixedTransponder;
            TestTarget = testTarget;
            MilitaryEmergency = militaryEmergency;
            MilitaryIdentification = militaryIdentification;
            FriendFoe = friendFoe;
            Extended = extended;
        }
    }
    #endregion

    #region Структуры для использования внутри структур
    struct CommBData
    {
        public string Data { get; }
        public string BDSCode { get; }

        public CommBData(string data, string bdsCode)
        {
            Data = data;
            BDSCode = bdsCode;
        }
    }

    #endregion

    #region Перечисления
    enum FlightStatus
    {
        NoAlert_NoSPI_Airborne,
        NoAlert_NoSPI_Ground,
        Alert_NoSPI_Airborne,
        Alert_NoSPI_Ground,
        Alert_SPI_All,
        NoAlert_SPI_All
    }
    enum CommunicationCapabilityOfTransponder
    {
        NoCapability,
        CommA_CommB,
        CommA_CommB_UplinkELM,
        CommA_CommB_UplinkDownlinkELM,
        Level5Capability
    }
    enum ClimbOrDescend
    {
        Maintaining,
        Climbing,
        Descending,
        Invalid
    }
    enum SensorMaintainingTrack
    {
        Combined,
        PSR,
        SSR_ModeS,
        Invalid
    }
    enum TargetReportType
    {
        NoDetection,
        SinglePSR,
        SingleSSR,
        SSR_PSR,
        SingleModeSAllCall,
        SingleModeSRollCall,
        ModeSAllCall_PSR,
        ModeSRollCall_PSR
    }
    enum FriendOrFoe
    {
        NoMode4,
        Friendly,
        Unknown,
        NoReply
    }
    #endregion
}
