using ControlUtils.DataModel;
using ControlUtils.Network;
using Coordinate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UavProject
{
    class UavViewModel : ViewModel
    {

        private static readonly Lazy<UavViewModel> _data = new Lazy<UavViewModel>(() => new UavViewModel());
        public static UavViewModel UavModel
        {
            get { return _data.Value; }
        }
        private Dictionary<string, AvData> m_uplink_datas = new Dictionary<string, AvData>();
        private AvUplinkData m_up = new AvUplinkData();
        Dictionary<int, byte[]> m_raws = new Dictionary<int, byte[]>();

        private AvDownlinkData m_dn = new AvDownlinkData();

        public static int DN_SIZE = 107;
        public static int UP_SIZE = 48;
        public static int UP_MSG_SIZE = 24;
        



        public UavViewModel()
        {
            m_uplink_datas.Add("FlightMode", new AvData("FlightMode", "b2 : D1-D4"));
            m_uplink_datas.Add("EmergencyMode", new AvData("Emergency Mode", "b2 : D0"));
            m_uplink_datas.Add("RollHeadingMode", new AvData("RollHeading Mode", "b3 : D5"));
            m_uplink_datas.Add("ThrottleCmd", new AvData("ThrottleCmd", "b4-b5"));
            m_uplink_datas.Add("RudderCmd", new AvData("RudderCmd", "b6-b7"));
            m_uplink_datas.Add("RollCmd", new AvData("RollCmd", "b8-b9"));
            m_uplink_datas.Add("PitchCmd", new AvData("PitchCmd", "b10-b11"));
            m_uplink_datas.Add("HeadingCmd", new AvData("HeadingCmd", "b12-b13"));
            m_uplink_datas.Add("LonSpeedCmd", new AvData("LonSpeedCmd", "b14-b15"));
            m_uplink_datas.Add("LatSpeedCmd", new AvData("LatSpeedCmd", "b16-b17"));
            m_uplink_datas.Add("AltitudeCmd", new AvData("AltitudeCmd", "b18-b19"));
            m_uplink_datas.Add("AltRateCmd", new AvData("AltRateCmd", "b20"));
            m_uplink_datas.Add("SlideCmd", new AvData("SlideCmd", "b21-b22"));
            m_uplink_datas.Add("EngineStart", new AvData("EngineStart", "b23 : D1"));
            m_uplink_datas.Add("EngineKill", new AvData("EngineKill", "b23 : D2"));
            m_uplink_datas.Add("CameraPanCmd", new AvData("CameraPanCmd", "b24-b25"));
            m_uplink_datas.Add("CameraTiltCmd", new AvData("CameraTiltCmd", "b26-b27"));
            m_uplink_datas.Add("CameraMode", new AvData("CameraMode", "b28 : D3-D4"));
            m_uplink_datas.Add("CameraZoom", new AvData("CameraZoom", "b28 : D1-D2"));
            m_uplink_datas.Add("CameraRecord", new AvData("CameraRecord", "b28 : D0"));
            m_uplink_datas.Add("GdtPanCmd", new AvData("GdtPanCmd", "b29-b30"));
            m_uplink_datas.Add("HeadingSensor", new AvData("HeadingSensor", "b31 : D4-D5"));
            m_uplink_datas.Add("SpeedSensor", new AvData("SpeedSensor", "b31 : D0"));         
        }

        public void UpdateUplink()
        {
            try
            {
                m_uplink_datas["FlightMode"].Value = m_up.flight_mode.ToString();
                m_uplink_datas["EmergencyMode"].Value = m_up.emergency_mode? "Ground" : "Air"; //
                m_uplink_datas["RollHeadingMode"].Value = m_up.rollheading_mode? "고도율" : "고도"; //
                m_uplink_datas["ThrottleCmd"].Value = string.Format("{0:F2}", m_up.throll_cmd / 100.0);
                m_uplink_datas["RudderCmd"].Value = string.Format("{0:F2}", m_up.rudder_cmd / 100.0);
                m_uplink_datas["RollCmd"].Value = string.Format("{0:F2}", m_up.roll_cmd / 100.0);
                m_uplink_datas["PitchCmd"].Value = string.Format("{0:F2}", m_up.pitch_cmd / 100.0);
                m_uplink_datas["HeadingCmd"].Value = string.Format("{0:F2}", m_up.heading_cmd / 100.0);
                m_uplink_datas["LonSpeedCmd"].Value = string.Format("{0:F2}", m_up.lon_speed_cmd / 100.0);
                m_uplink_datas["LatSpeedCmd"].Value = string.Format("{0:F2}", m_up.lat_speed_cmd / 100.0);
                m_uplink_datas["AltitudeCmd"].Value = string.Format("{0:F2}", m_up.altitude_cmd / 10.0 - 500);
                m_uplink_datas["AltRateCmd"].Value = string.Format("{0:F2}", m_up.altrate_cmd / 10.0);
                m_uplink_datas["SlideCmd"].Value = string.Format("{0:F2}", m_up.slide_cmd / 100.0);
                m_uplink_datas["EngineStart"].Value = m_up.engine_start == 0 ? "-" : "Engine Start";
                m_uplink_datas["EngineKill"].Value = m_up.engine_kill == 0 ? "-" : "Engine Kill";
                m_uplink_datas["CameraPanCmd"].Value = string.Format("{0:F2}", m_up.camera_pan_cmd / 100.0);
                m_uplink_datas["CameraTiltCmd"].Value = string.Format("{0:F2}", m_up.camera_tilt_cmd / 100.0);
                m_uplink_datas["CameraMode"].Value = m_up.camera_mode.ToString();
                m_uplink_datas["CameraZoom"].Value = m_up.camera_zoom == 0 ? "Stop" : (m_up.camera_zoom == 1 ? "ZoomIn" : "ZoomOut");
                m_uplink_datas["CameraRecord"].Value = m_up.camera_record == 0 ? "Stop" : "Record";
                m_uplink_datas["GdtPanCmd"].Value = string.Format("{0}", m_up.gdt_pan_cmd);
                m_uplink_datas["HeadingSensor"].Value = m_up.heading_sensor == 0 ? "defeault" :  (m_up.heading_sensor == 1? "시작" : "종료") ;
                m_uplink_datas["SpeedSensor"].Value = m_up.speed_sensor == 0 ? "GPS" : "Pitot";

                NotifyPropertyChanged("Items");

                if(m_up.init_position.Valid)
                {
                    m_dn.AvLongitude = m_up.init_position.WorldX;
                    m_dn.AvLatitude = m_up.init_position.WorldY;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public bool AutoCheck
        {
            get
            {
                if (!_props.ContainsKey("AutoCheck")) _props.Add("AutoCheck", new NotifyingProperty("AutoCheck", typeof(bool), true));
                return (bool)GetValue(_props["AutoCheck"]);
            }
            set
            {
                if (!_props.ContainsKey("AutoCheck")) _props.Add("AutoCheck", new NotifyingProperty("AutoCheck", typeof(bool), true));
                SetValue(_props["AutoCheck"], value);
            }
        }
        public int UplinkCount
        {
            get
            {
                if (!_props.ContainsKey("UplinkCount")) _props.Add("UplinkCount", new NotifyingProperty("UplinkCount", typeof(int), 0));
                return (int)GetValue(_props["UplinkCount"]);
            }
            set
            {
                if (!_props.ContainsKey("UplinkCount")) _props.Add("UplinkCount", new NotifyingProperty("UplinkCount", typeof(int), 0));
                SetValue(_props["UplinkCount"], value);
            }
        }

        public int DownlinkCount
        {
            get
            {
                if (!_props.ContainsKey("DownlinkCount")) _props.Add("DownlinkCount", new NotifyingProperty("DownlinkCount", typeof(int), 0));
                return (int)GetValue(_props["DownlinkCount"]);
            }
            set
            {
                if (!_props.ContainsKey("DownlinkCount")) _props.Add("DownlinkCount", new NotifyingProperty("DownlinkCount", typeof(int), 0));
                SetValue(_props["DownlinkCount"], value);
            }
        }

        public  IEnumerable<AvData> UplinkItems
        {
            get
            {
                return m_uplink_datas.Values.ToArray();
            }
        }
        
        public AvDownlinkData Downlink
        {
            get { return m_dn; }
        }


        public void TryParseUplink(byte[] buffer)
        {
            if (!(buffer[0] == 0xAA && buffer[1] == 0x33)) return;

            if (buffer.Length == UP_SIZE)
            { //crc check
                UInt32 crc = Crc32.MakeCrc(buffer, UP_SIZE - 4);
                UInt32 rcv_crc = BitConverter.ToUInt32(buffer, UP_SIZE - 4);
                //System.Diagnostics.Debug.WriteLine(string.Format("{0:X}, {1:X}", crc, rcv_crc));
                if (crc != rcv_crc)
                {
                    // m_crc_error++;
               //     return;
                }
                m_up.last_message_crc = 0;
            }
            int cnt = UplinkCount;
            cnt++;
            if (cnt > byte.MaxValue) cnt = 0;
            UplinkCount = cnt;

            int idx = 2;
            byte temp = buffer[idx++];
            bool is_msg = (temp & 0x20) == 0x20;
            int msg_id = 0xFF;

            if (buffer.Length == UP_MSG_SIZE)
            {
                UInt32 crc = Crc32.MakeCrc(buffer, UP_MSG_SIZE - 4);
                UInt32 rcv_crc = BitConverter.ToUInt32(buffer, UP_MSG_SIZE - 4);
                //System.Diagnostics.Debug.WriteLine(string.Format("{0:X}, {1:X}", crc, rcv_crc));
                if (crc != rcv_crc)
                {
                    // m_crc_error++;
                    return;
                }

                if (is_msg)
                {
                    //crc 저장
                    m_up.last_message_crc = crc;
                    m_up.need_reply = true;

                    msg_id = temp & 0x0F;
                    if (!m_raws.ContainsKey(msg_id)) m_raws.Add(msg_id, buffer);
                    Buffer.BlockCopy(buffer, 0, m_raws[msg_id], 0, buffer.Length);

                    if (msg_id == 3)  //귀환점 위치
                    {
                        double lon = BitConverter.ToUInt32(buffer, 4) / 1000000.0;
                        double lat = BitConverter.ToUInt32(buffer, 8) / 1000000.0;
                        m_up.init_position = new WorldType(lon, lat);
                    }

                    return;
                }
            }

            if (!m_raws.ContainsKey(msg_id)) m_raws.Add(msg_id, buffer);
            Buffer.BlockCopy(buffer, 0, m_raws[msg_id], 0, buffer.Length);

            m_up.flight_mode = (eFlightMode)((temp & 0x1E) >> 1);
            m_up.emergency_mode = (temp & 0x01) == 0x01; //

            temp = buffer[idx++];
            m_up.rollheading_mode = (temp & 0x20) == 0x20;
            //((temp & 0x20) >> 5);

            
            m_up.throll_cmd = BitConverter.ToUInt16(buffer, idx);
            idx += 2;

            m_up.rudder_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.roll_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.pitch_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.heading_cmd = BitConverter.ToUInt16(buffer, idx);
            idx += 2;

            m_up.lon_speed_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.lat_speed_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.altitude_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.altrate_cmd = buffer[idx++];

            m_up.slide_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            temp = buffer[idx++];
            m_up.engine_start = (temp & 0x02) >> 1;
            m_up.engine_kill = (temp & 0x01);

            m_up.camera_pan_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            m_up.camera_tilt_cmd = BitConverter.ToInt16(buffer, idx);
            idx += 2;

            temp = buffer[idx++];
            m_up.camera_mode = (temp & 0x18) >> 3;
            m_up.camera_zoom = (temp & 0x06) >> 1;
            m_up.camera_record = (temp & 0x01);          

            m_up.gdt_pan_cmd = BitConverter.ToUInt16(buffer, idx);
            idx += 2;

            temp = buffer[idx++];
            m_up.heading_sensor = (temp & 0x30) >> 4;
            m_up.speed_sensor = (temp & 0x01);

            UpdateUplink(); //
        }


        public byte[] GetUp(int msg_id) //수정완료
        {
            if (!m_raws.ContainsKey(msg_id)) return null; //
            return m_raws[msg_id];
        }

        public byte[] GetDown()
        {
            if (!m_raws.ContainsKey(int.MaxValue)) return null; //
            return m_raws[int.MaxValue];
        }

        public byte[]  GeneratorDownlink()
        {
            byte[] buffer = new byte[DN_SIZE];

            bool uplink_send = AutoCheck;
        
            m_dn.Generate(buffer, m_up.need_reply, m_up.last_message_crc, uplink_send, m_up);

            //업링크 값을 그대로 내려주려면
            //if (AutoCheck)
            //{
            //    //flightmode, emergency
            //    int temp = (int)m_up.flight_mode<<1;
            //    if (m_up.emergency_mode == 1) temp |= 0x01;
            //    buffer[2] = (byte)temp;

            //    //roll heading, heading sensor
            //    temp = 0;
            //    temp = m_up.heading_sensor << 2;
            //    if (m_up.rollheading_mode == 1) temp |= 0x20;
            //    buffer[3] = (byte)temp;

            //    //speed sensor
            //    if (m_up.speed_sensor == 1) buffer[57] = 0x01;
            //}

            UInt32 crc = Crc32.MakeCrc(buffer, DN_SIZE - 4);
            Buffer.BlockCopy(BitConverter.GetBytes(crc), 0, buffer, DN_SIZE - 4, 4);

            m_up.need_reply = false;

            if (!m_raws.ContainsKey(int.MaxValue)) m_raws.Add(int.MaxValue, buffer);
            Buffer.BlockCopy(buffer, 0, m_raws[int.MaxValue], 0, buffer.Length);

            int cnt = DownlinkCount;
            cnt++;
            if (cnt > byte.MaxValue) cnt = 0;
            DownlinkCount = cnt;

            return buffer;
        }
    }
}
