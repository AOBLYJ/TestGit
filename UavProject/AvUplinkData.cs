using ControlUtils.DataModel;
using Coordinate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UavProject
{
    struct AvUplinkData 
    {
        public eFlightMode flight_mode;
        public bool emergency_mode;
        public bool rollheading_mode;

        public ushort throll_cmd;
        public short rudder_cmd;
        public short roll_cmd;
        public short pitch_cmd;
        public ushort heading_cmd;
        public short lon_speed_cmd;
        public short lat_speed_cmd;
        public short altitude_cmd;
        public byte altrate_cmd;
        public short slide_cmd;

        public int engine_start;
        public int engine_kill;
        public short camera_pan_cmd;
        public short camera_tilt_cmd;

        public int camera_mode;
        public int camera_zoom;
        public int camera_record;

        public ushort gdt_pan_cmd;
        public int heading_sensor;
        public int speed_sensor;

        public UInt32 last_message_crc;
        public bool need_reply;
        public WorldType init_position;
    }
}
