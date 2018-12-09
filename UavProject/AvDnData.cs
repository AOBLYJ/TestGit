using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UavProject
{
    struct AvDnData
    {
        public short pitch;
        public short roll;
        public ushort heading;
        public short roll_rate;
        public short pitch_rate;
        public short yaw_rate;
        public ushort mag_heading;
        public long gps_longitude;
        public long gps_latitude;
        public ushort gps_altitude;
        public short gps_speed;
        public ushort gps_heading;
        public sbyte gps_velocity;
        public byte gps_used;
        public byte lidar_altimeter;
        public byte airdata_speed;
        public byte throttle_feedback;
        public short sensors_status;
        public short vehicle_status;      
    }
}
