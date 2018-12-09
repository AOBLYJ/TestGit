using ControlUtils.DataModel;
using ControlUtils.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UavProject
{
    class AvDownlinkData : ViewModel
    {
        private AvDnData m_primary = new AvDnData();
        private AvDnData m_secondary = new AvDnData();
        public void  Generate(byte[] buffer, bool return_msg, UInt32 msg_crc, bool uplink_send, AvUplinkData m_up)
        {
            int idx = 0;
            buffer[idx++] = 0xAA;
            buffer[idx++] = 0x33;

            byte temp = (byte)((uplink_send ? (int)m_up.flight_mode : FlightMode) << 1);
            if (uplink_send ? m_up.emergency_mode : EmergencyMode) temp |= 0x01;
            buffer[idx++] = temp;

            temp = 0;

            temp = (byte)((uplink_send ? m_up.heading_sensor : MagCalibration) << 2);
            if (CurrentFcc) temp |= 0x01;
            if (return_msg) temp |= 0x10;
            if (uplink_send ? m_up.rollheading_mode : RollHeadingMode) temp |= 0x20;
            buffer[idx++] = temp;

            //0 primary 1 secondary

            //Primary
            Buffer.BlockCopy(BitConverter.GetBytes(uplink_send ? (PrimarySecondaryMode == 0 ? m_up.roll_cmd : m_primary.roll) : m_primary.roll), 0, buffer, idx, 2); 
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(uplink_send ? (PrimarySecondaryMode == 0 ? m_up.pitch_cmd : m_primary.pitch) : m_primary.pitch), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.heading), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.roll_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.pitch_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.yaw_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.mag_heading), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.gps_longitude), 0, buffer, idx, 4);
            idx += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.gps_latitude), 0, buffer, idx, 4);
            idx += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.gps_altitude), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.gps_speed), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.gps_heading), 0, buffer, idx, 2);
            idx += 2;
            buffer[idx++] = (byte)m_primary.gps_velocity;
            buffer[idx++] = m_primary.gps_used;
            buffer[idx++] = m_primary.lidar_altimeter;
            buffer[idx++] = m_primary.airdata_speed;
            buffer[idx++] = (byte)AirdataTemp;
            buffer[idx++] = (byte)FccTemp;
            buffer[idx++] = uplink_send ? (PrimarySecondaryMode == 0 ? (byte)m_up.throll_cmd : m_primary.throttle_feedback) : m_primary.throttle_feedback;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.sensors_status), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_primary.vehicle_status), 0, buffer, idx, 2);
            idx += 2;

            idx++; // Reserved

            //Secondary
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.roll), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.pitch), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.heading), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.roll_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.pitch_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.yaw_rate), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.mag_heading), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.gps_longitude), 0, buffer, idx, 4);
            idx += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.gps_latitude), 0, buffer, idx, 4);
            idx += 4;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.gps_altitude), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.gps_speed), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.gps_heading), 0, buffer, idx, 2);
            idx += 2;
            buffer[idx++] = (byte)m_secondary.gps_velocity;
            buffer[idx++] = m_secondary.gps_used;
            buffer[idx++] = m_secondary.lidar_altimeter;
            buffer[idx++] = m_secondary.airdata_speed;        
            buffer[idx++] = m_secondary.throttle_feedback;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.sensors_status), 0, buffer, idx, 2);
            idx += 2;
            Buffer.BlockCopy(BitConverter.GetBytes(m_secondary.vehicle_status), 0, buffer, idx, 2);
            idx += 2;

            idx++; // Reserved

            buffer[idx++] = (byte)GeneratorVoltage;
            buffer[idx++] = (byte)MainCurrent;
            buffer[idx++] = (byte)EngineVoltage;
            buffer[idx++] = (byte)ServoVoltage;
            buffer[idx++] = (byte)ServoCurrent;
            buffer[idx++] = (byte)BearingTemp;
            buffer[idx++] = (byte)CollingWaterTemp;
            buffer[idx++] = (byte)MainGearBoxTemp;
            buffer[idx++] = (byte)TailGearBoxTemp;
            buffer[idx++] = (byte)FuelLevel;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)EngineRPM), 0, buffer, idx, 2);
            idx += 2;
            buffer[idx++] = uplink_send ? (byte)m_up.speed_sensor : (byte)CurrentSpeedSensor;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)CameraPanPos), 0, buffer, idx, 2);
            idx += 2;
            buffer[idx++] = (byte)CameraTiltPos;

            idx++; // Reserved

            Buffer.BlockCopy(BitConverter.GetBytes(msg_crc), 0, buffer, idx, 4);
            
            //UInt32 crc = Crc32.MakeCrc(buffer, UavViewModel.DN_SIZE - 4);
            //Buffer.BlockCopy(BitConverter.GetBytes(crc), 0, buffer, UavViewModel.DN_SIZE - 4, 4);

        }

       

        //0  : all,  1 : only primary,  2 only secondary
        public int  PrimarySecondaryMode
        {
            get
            {
                if (!_props.ContainsKey("PrimarySecondaryMode")) _props.Add("PrimarySecondaryMode", new NotifyingProperty("PrimarySecondaryMode", typeof(int), 0));
                return (int)GetValue(_props["PrimarySecondaryMode"]);
            }
            set
            {
                if (!_props.ContainsKey("PrimarySecondaryMode")) _props.Add("PrimarySecondaryMode", new NotifyingProperty("PrimarySecondaryMode", typeof(int), 0));
                SetValue(_props["PrimarySecondaryMode"], value);
            }
        }
          

     
        public int FlightMode
        {
            get
            {
                if (!_props.ContainsKey("FlightMode")) _props.Add("FlightMode", new NotifyingProperty("FlightMode", typeof(int), 0));
                return (int)GetValue(_props["FlightMode"]);
            }
            set
            {
                if (!_props.ContainsKey("FlightMode")) _props.Add("FlightMode", new NotifyingProperty("FlightMode", typeof(int), 0));
                SetValue(_props["FlightMode"], value);
            }
        }
        public bool EmergencyMode
        {
            get
            {
                if (!_props.ContainsKey("EmergencyMode")) _props.Add("EmergencyMode", new NotifyingProperty("EmergencyMode", typeof(bool), false));
                return (bool)GetValue(_props["EmergencyMode"]);
            }
            set
            {
                if (!_props.ContainsKey("EmergencyMode")) _props.Add("EmergencyMode", new NotifyingProperty("EmergencyMode", typeof(bool), false));
                SetValue(_props["EmergencyMode"], value);
            }
        }

        public bool RollHeadingMode
        {
            get
            {
                if (!_props.ContainsKey("RollHeadingMode")) _props.Add("RollHeadingMode", new NotifyingProperty("RollHeadingMode", typeof(bool), false));
                return (bool)GetValue(_props["RollHeadingMode"]);
            }
            set
            {
                if (!_props.ContainsKey("RollHeadingMode")) _props.Add("RollHeadingMode", new NotifyingProperty("RollHeadingMode", typeof(bool), false));
                SetValue(_props["RollHeadingMode"], value);
            }
        }

        public int MagCalibration
        {
            get
            {
                if (!_props.ContainsKey("MagCalibration")) _props.Add("MagCalibration", new NotifyingProperty("MagCalibration", typeof(int), 0));
                return (int)GetValue(_props["MagCalibration"]);
            }
            set
            {
                if (!_props.ContainsKey("MagCalibration")) _props.Add("MagCalibration", new NotifyingProperty("MagCalibration", typeof(int), 0));
                SetValue(_props["MagCalibration"], value);
            }
        }
        public bool CurrentFcc
        {
            get
            {
                if (!_props.ContainsKey("CurrentFcc")) _props.Add("CurrentFcc", new NotifyingProperty("CurrentFcc", typeof(bool), false));
                return (bool)GetValue(_props["CurrentFcc"]);
            }
            set
            {
                if (!_props.ContainsKey("CurrentFcc")) _props.Add("CurrentFcc", new NotifyingProperty("CurrentFcc", typeof(bool), false));
                SetValue(_props["CurrentFcc"], value);
            }
        }

        public int GeneratorVoltage
        {
            get
            {
                if (!_props.ContainsKey("GeneratorVoltage")) _props.Add("GeneratorVoltage", new NotifyingProperty("GeneratorVoltage", typeof(int), 0));
                return (int)GetValue(_props["GeneratorVoltage"]);
            }
            set
            {
                if (!_props.ContainsKey("GeneratorVoltage")) _props.Add("GeneratorVoltage", new NotifyingProperty("GeneratorVoltage", typeof(int), 0));
                SetValue(_props["GeneratorVoltage"], value);
            }
        }
        public int MainCurrent
        {
            get
            {
                if (!_props.ContainsKey("MainCurrent")) _props.Add("MainCurrent", new NotifyingProperty("MainCurrent", typeof(int), 0));
                return (int)GetValue(_props["MainCurrent"]);
            }
            set
            {
                if (!_props.ContainsKey("MainCurrent")) _props.Add("MainCurrent", new NotifyingProperty("MainCurrent", typeof(int), 0));
                SetValue(_props["MainCurrent"], value);
            }
        }
        public int EngineVoltage
        {
            get
            {
                if (!_props.ContainsKey("EngineVoltage")) _props.Add("EngineVoltage", new NotifyingProperty("EngineVoltage", typeof(int), 0));
                return (int)GetValue(_props["EngineVoltage"]);
            }
            set
            {
                if (!_props.ContainsKey("EngineVoltage")) _props.Add("EngineVoltage", new NotifyingProperty("EngineVoltage", typeof(int), 0));
                SetValue(_props["EngineVoltage"], value);
            }
        }
        public int ServoVoltage
        {
            get
            {
                if (!_props.ContainsKey("ServoVoltage")) _props.Add("ServoVoltage", new NotifyingProperty("ServoVoltage", typeof(int), 0));
                return (int)GetValue(_props["ServoVoltage"]);
            }
            set
            {
                if (!_props.ContainsKey("ServoVoltage")) _props.Add("ServoVoltage", new NotifyingProperty("ServoVoltage", typeof(int), 0));
                SetValue(_props["ServoVoltage"], value);
            }
        }
        public int ServoCurrent
        {
            get
            {
                if (!_props.ContainsKey("ServoCurrent")) _props.Add("ServoCurrent", new NotifyingProperty("ServoCurrent", typeof(int), 0));
                return (int)GetValue(_props["ServoCurrent"]);
            }
            set
            {
                if (!_props.ContainsKey("ServoCurrent")) _props.Add("ServoCurrent", new NotifyingProperty("ServoCurrent", typeof(int), 0));
                SetValue(_props["ServoCurrent"], value);
            }
        }
        public int BearingTemp
        {
            get
            {
                if (!_props.ContainsKey("BearingTemp")) _props.Add("BearingTemp", new NotifyingProperty("BearingTemp", typeof(int), 0));
                return (int)GetValue(_props["BearingTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("BearingTemp")) _props.Add("BearingTemp", new NotifyingProperty("BearingTemp", typeof(int), 0));
                SetValue(_props["BearingTemp"], value);
            }
        }
        public int CollingWaterTemp
        {
            get
            {
                if (!_props.ContainsKey("CollingWaterTemp")) _props.Add("CollingWaterTemp", new NotifyingProperty("CollingWaterTemp", typeof(int), 0));
                return (int)GetValue(_props["CollingWaterTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("CollingWaterTemp")) _props.Add("CollingWaterTemp", new NotifyingProperty("CollingWaterTemp", typeof(int), 0));
                SetValue(_props["CollingWaterTemp"], value);
            }
        }
        public int MainGearBoxTemp
        {
            get
            {
                if (!_props.ContainsKey("MainGearBoxTemp")) _props.Add("MainGearBoxTemp", new NotifyingProperty("MainGearBoxTemp", typeof(int), 0));
                return (int)GetValue(_props["MainGearBoxTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("MainGearBoxTemp")) _props.Add("MainGearBoxTemp", new NotifyingProperty("MainGearBoxTemp", typeof(int), 0));
                SetValue(_props["MainGearBoxTemp"], value);
            }
        }
        public int TailGearBoxTemp
        {
            get
            {
                if (!_props.ContainsKey("TailGearBoxTemp")) _props.Add("TailGearBoxTemp", new NotifyingProperty("TailGearBoxTemp", typeof(int), 0));
                return (int)GetValue(_props["TailGearBoxTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("TailGearBoxTemp")) _props.Add("TailGearBoxTemp", new NotifyingProperty("TailGearBoxTemp", typeof(int), 0));
                SetValue(_props["TailGearBoxTemp"], value);
            }
        }
        public int FuelLevel
        {
            get
            {
                if (!_props.ContainsKey("FuelLevel")) _props.Add("FuelLevel", new NotifyingProperty("FuelLevel", typeof(int), 0));
                return (int)GetValue(_props["FuelLevel"]);
            }
            set
            {
                if (!_props.ContainsKey("FuelLevel")) _props.Add("FuelLevel", new NotifyingProperty("FuelLevel", typeof(int), 0));
                SetValue(_props["FuelLevel"], value);
            }
        }
        public int EngineRPM
        {
            get
            {
                if (!_props.ContainsKey("EngineRPM")) _props.Add("EngineRPM", new NotifyingProperty("EngineRPM", typeof(int), 0));
                return (int)GetValue(_props["EngineRPM"]);
            }
            set
            {
                if (!_props.ContainsKey("EngineRPM")) _props.Add("EngineRPM", new NotifyingProperty("EngineRPM", typeof(int), 0));
                SetValue(_props["EngineRPM"], value);
            }
        }
        public int CurrentSpeedSensor
        {
            get
            {
                if (!_props.ContainsKey("CurrentSpeedSensor")) _props.Add("CurrentSpeedSensor", new NotifyingProperty("CurrentSpeedSensor", typeof(int), 0));
                return (int)GetValue(_props["CurrentSpeedSensor"]);
            }
            set
            {
                if (!_props.ContainsKey("CurrentSpeedSensor")) _props.Add("CurrentSpeedSensor", new NotifyingProperty("CurrentSpeedSensor", typeof(int), 0));
                SetValue(_props["CurrentSpeedSensor"], value);
            }
        }

        public int CameraPanPos
        {
            get
            {
                if (!_props.ContainsKey("CameraPanPos")) _props.Add("CameraPanPos", new NotifyingProperty("CameraPanPos", typeof(int), 0));
                return (int)GetValue(_props["CameraPanPos"]);
            }
            set
            {
                if (!_props.ContainsKey("CameraPanPos")) _props.Add("CameraPanPos", new NotifyingProperty("CameraPanPos", typeof(int), 0));
                SetValue(_props["CameraPanPos"], value);
            }
        }

        public int CameraTiltPos
        {
            get
            {
                if (!_props.ContainsKey("CameraTiltPos")) _props.Add("CameraTiltPos", new NotifyingProperty("CameraTiltPos", typeof(int), 0));
                return (int)GetValue(_props["CameraTiltPos"]);
            }
            set
            {
                if (!_props.ContainsKey("CameraTiltPos")) _props.Add("CameraTiltPos", new NotifyingProperty("CameraTiltPos", typeof(int), 0));
                SetValue(_props["CameraTiltPos"], value);
            }
        }
        public int AirdataTemp
        {
            get
            {
                if (!_props.ContainsKey("AirdataTemp")) _props.Add("AirdataTemp", new NotifyingProperty("AirdataTemp", typeof(int), 0));
                return (int)GetValue(_props["AirdataTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("AirdataTemp")) _props.Add("AirdataTemp", new NotifyingProperty("AirdataTemp", typeof(int), 0));
                SetValue(_props["AirdataTemp"], value);
            }
        }
        public int FccTemp
        {
            get
            {
                if (!_props.ContainsKey("FccTemp")) _props.Add("FccTemp", new NotifyingProperty("FccTemp", typeof(int), 0));
                return (int)GetValue(_props["FccTemp"]);
            }
            set
            {
                if (!_props.ContainsKey("FccTemp")) _props.Add("FccTemp", new NotifyingProperty("FccTemp", typeof(int), 0));
                SetValue(_props["FccTemp"], value);
            }
        }
        //pri,snd

        public double AvLongitude
        {
            get
            {
                if (!_props.ContainsKey("AvLongitude")) _props.Add("AvLongitude", new NotifyingProperty("AvLongitude", typeof(double), 0.0));
                return (double)GetValue(_props["AvLongitude"]);
            }
            set
            {
                if (!_props.ContainsKey("AvLongitude")) _props.Add("AvLongitude", new NotifyingProperty("AvLongitude", typeof(double), 0.0));
                SetValue(_props["AvLongitude"], value);
                {
                    long p = (long)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_longitude = p;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_longitude = p;
                }
            }
        }
        public double AvLatitude
        {
            get
            {
                if (!_props.ContainsKey("AvLatitude")) _props.Add("AvLatitude", new NotifyingProperty("AvLatitude", typeof(double), 0.0));
                return (double)GetValue(_props["AvLatitude"]);
            }
            set
            {
                if (!_props.ContainsKey("AvLatitude")) _props.Add("AvLatitude", new NotifyingProperty("AvLatitude", typeof(double), 0.0));
                SetValue(_props["AvLatitude"], value);
                {
                    long p = (long)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_latitude = p;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_latitude = p;
                }
            }
        }

        public int Pitch
        {
            get
            {
                if (!_props.ContainsKey("Pitch")) _props.Add("Pitch", new NotifyingProperty("Pitch", typeof(int), 0));
                return (int)GetValue(_props["Pitch"]);
            }
            set
            {
                if (!_props.ContainsKey("Pitch")) _props.Add("Pitch", new NotifyingProperty("Pitch", typeof(int), 0));
                if(SetValue(_props["Pitch"], value))
                {
                    short p = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.pitch = p;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.pitch = p;                   
                }
            }
        }
        public int Roll
        {
            get
            {
                if (!_props.ContainsKey("Roll")) _props.Add("Roll", new NotifyingProperty("Roll", typeof(int), 0));
                return (int)GetValue(_props["Roll"]);
            }
            set
            {
                if (!_props.ContainsKey("Roll")) _props.Add("Roll", new NotifyingProperty("Roll", typeof(int), 0));
                SetValue(_props["Roll"], value);
                {
                    short r = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.roll = r;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.roll = r;
                }
            }
        }
        public int Heading
        {
            get
            {
                if (!_props.ContainsKey("Heading")) _props.Add("Heading", new NotifyingProperty("Heading", typeof(int), 0));
                return (int)GetValue(_props["Heading"]);
            }
            set
            {
                if (!_props.ContainsKey("Heading")) _props.Add("Heading", new NotifyingProperty("Heading", typeof(int), 0));
                SetValue(_props["Heading"], value);
                {
                    ushort h = (ushort)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.heading = h;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.heading = h;
                }
            }
        }
        public int RollRate
        {
            get
            {
                if (!_props.ContainsKey("RollRate")) _props.Add("RollRate", new NotifyingProperty("RollRate", typeof(int), 0));
                return (int)GetValue(_props["RollRate"]);
            }
            set
            {
                if (!_props.ContainsKey("RollRate")) _props.Add("RollRate", new NotifyingProperty("RollRate", typeof(int), 0));
                SetValue(_props["RollRate"], value);
                {
                    short r = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.roll_rate = r;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.roll_rate = r;
                }
            }
        }
        public int PitchRate
        {
            get
            {
                if (!_props.ContainsKey("PitchRate")) _props.Add("PitchRate", new NotifyingProperty("PitchRate", typeof(int), 0));
                return (int)GetValue(_props["PitchRate"]);
            }
            set
            {
                if (!_props.ContainsKey("PitchRate")) _props.Add("PitchRate", new NotifyingProperty("PitchRate", typeof(int), 0));
                SetValue(_props["PitchRate"], value);
                {
                    short p = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.pitch_rate = p;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.pitch_rate = p;
                }
            }
        }
        public int YawRate
        {
            get
            {
                if (!_props.ContainsKey("YawRate")) _props.Add("YawRate", new NotifyingProperty("YawRate", typeof(int), 0));
                return (int)GetValue(_props["YawRate"]);
            }
            set
            {
                if (!_props.ContainsKey("YawRate")) _props.Add("YawRate", new NotifyingProperty("YawRate", typeof(int), 0));
                SetValue(_props["YawRate"], value);
                {
                    short y = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.yaw_rate = y;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.yaw_rate = y;
                }
            }
        }
        public int MagHeading
        {
            get
            {
                if (!_props.ContainsKey("MagHeading")) _props.Add("MagHeading", new NotifyingProperty("MagHeading", typeof(int), 0));
                return (int)GetValue(_props["MagHeading"]);
            }
            set
            {
                if (!_props.ContainsKey("MagHeading")) _props.Add("MagHeading", new NotifyingProperty("MagHeading", typeof(int), 0));
                SetValue(_props["MagHeading"], value);
                {
                    ushort m = (ushort)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.mag_heading = m;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.mag_heading = m;
                }
            }
        }
        public int GPSAltitude
        {
            get
            {
                if (!_props.ContainsKey("GPSAltitude")) _props.Add("GPSAltitude", new NotifyingProperty("GPSAltitude", typeof(int), 0));
                return (int)GetValue(_props["GPSAltitude"]);
            }
            set
            {
                if (!_props.ContainsKey("GPSAltitude")) _props.Add("GPSAltitude", new NotifyingProperty("GPSAltitude", typeof(int), 0));
                SetValue(_props["GPSAltitude"], value);
                {
                    ushort g = (ushort)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_altitude = g;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_altitude = g;
                }
            }
        }
        public int GPSSpeed
        {
            get
            {
                if (!_props.ContainsKey("GPSSpeed")) _props.Add("GPSSpeed", new NotifyingProperty("GPSSpeed", typeof(int), 0));
                return (int)GetValue(_props["GPSSpeed"]);
            }
            set
            {
                if (!_props.ContainsKey("GPSSpeed")) _props.Add("GPSSpeed", new NotifyingProperty("GPSSpeed", typeof(int), 0));
                SetValue(_props["GPSSpeed"], value);
                {
                    short g = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_speed = g;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_speed = g;
                }
            }
        }
        public int GPSHeading
        {
            get
            {
                if (!_props.ContainsKey("GPSHeading")) _props.Add("GPSHeading", new NotifyingProperty("GPSHeading", typeof(int), 0));
                return (int)GetValue(_props["GPSHeading"]);
            }
            set
            {
                if (!_props.ContainsKey("GPSHeading")) _props.Add("GPSHeading", new NotifyingProperty("GPSHeading", typeof(int), 0));
                SetValue(_props["GPSHeading"], value);
                {
                    ushort g = (ushort)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_heading = g;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_heading = g;
                }
            }
        }
        public int GPSVerticalVelocity
        {
            get
            {
                if (!_props.ContainsKey("GPSVerticalVelocity")) _props.Add("GPSVerticalVelocity", new NotifyingProperty("GPSVerticalVelocity", typeof(int), 0));
                return (int)GetValue(_props["GPSVerticalVelocity"]);
            }
            set
            {
                if (!_props.ContainsKey("GPSVerticalVelocity")) _props.Add("GPSVerticalVelocity", new NotifyingProperty("GPSVerticalVelocity", typeof(int), 0));
                SetValue(_props["GPSVerticalVelocity"], value);
                {
                    sbyte g = (sbyte)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_velocity = g;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_velocity = g;
                }
            }
        }
        public int GPSUsed
        {
            get
            {
                if (!_props.ContainsKey("GPSUsed")) _props.Add("GPSUsed", new NotifyingProperty("GPSUsed", typeof(int), 0));
                return (int)GetValue(_props["GPSUsed"]);
            }
            set
            {
                if (!_props.ContainsKey("GPSUsed")) _props.Add("GPSUsed", new NotifyingProperty("GPSUsed", typeof(int), 0));
                SetValue(_props["GPSUsed"], value);
                {
                    byte g = (byte)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.gps_used = g;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.gps_used = g;
                }
            }
        }
        public int LidarAltimeter
        {
            get
            {
                if (!_props.ContainsKey("LidarAltimeter")) _props.Add("LidarAltimeter", new NotifyingProperty("LidarAltimeter", typeof(int), 0));
                return (int)GetValue(_props["LidarAltimeter"]);
            }
            set
            {
                if (!_props.ContainsKey("LidarAltimeter")) _props.Add("LidarAltimeter", new NotifyingProperty("LidarAltimeter", typeof(int), 0));
                SetValue(_props["LidarAltimeter"], value);
                {
                    byte a = (byte)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.lidar_altimeter = a;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.lidar_altimeter = a;
                }
            }
        }
        public int AirdataSpeed
        {
            get
            {
                if (!_props.ContainsKey("AirdataSpeed")) _props.Add("AirdataSpeed", new NotifyingProperty("AirdataSpeed", typeof(int), 0));
                return (int)GetValue(_props["AirdataSpeed"]);
            }
            set
            {
                if (!_props.ContainsKey("AirdataSpeed")) _props.Add("AirdataSpeed", new NotifyingProperty("AirdataSpeed", typeof(int), 0));
                SetValue(_props["AirdataSpeed"], value);
                {
                    byte a = (byte)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.airdata_speed = a;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.airdata_speed = a;
                }
            }
        }
        public int ThrottleFeedback
        {
            get
            {
                if (!_props.ContainsKey("ThrottleFeedback")) _props.Add("ThrottleFeedback", new NotifyingProperty("ThrottleFeedback", typeof(int), 0));
                return (int)GetValue(_props["ThrottleFeedback"]);
            }
            set
            {
                if (!_props.ContainsKey("ThrottleFeedback")) _props.Add("ThrottleFeedback", new NotifyingProperty("ThrottleFeedback", typeof(int), 0));
                SetValue(_props["ThrottleFeedback"], value);
                {
                    byte t = (byte)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.throttle_feedback = t;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.throttle_feedback = t;
                }
            }
        }
        public int SensorsStatus
        {
            get
            {
                if (!_props.ContainsKey("SensorsStatus")) _props.Add("SensorsStatus", new NotifyingProperty("SensorsStatus", typeof(int), 0));
                return (int)GetValue(_props["SensorsStatus"]);
            }
            set
            {
                if (!_props.ContainsKey("SensorsStatus")) _props.Add("SensorsStatus", new NotifyingProperty("SensorsStatus", typeof(int), 0));
                SetValue(_props["SensorsStatus"], value);
                {
                    short s = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.sensors_status = s;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.sensors_status = s;
                }
            }
        }
        public int VehicleStatus
        {
            get
            {
                if (!_props.ContainsKey("VehicleStatus")) _props.Add("VehicleStatus", new NotifyingProperty("VehicleStatus", typeof(int), 0));
                return (int)GetValue(_props["VehicleStatus"]);
            }
            set
            {
                if (!_props.ContainsKey("VehicleStatus")) _props.Add("VehicleStatus", new NotifyingProperty("VehicleStatus", typeof(int), 0));
                SetValue(_props["VehicleStatus"], value);
                {
                    short v = (short)value;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 1) m_primary.vehicle_status = v;
                    if (PrimarySecondaryMode == 0 || PrimarySecondaryMode == 2) m_secondary.vehicle_status = v;
                }
            }
        }
    }
}
