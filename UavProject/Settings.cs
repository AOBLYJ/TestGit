using ControlUtils.DataModel;
using ControlUtils.Units;
using Coordinate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UavProject.DataModel
{
    public class Settings : ViewModel
    {
        private static readonly Lazy<Settings> _data = new Lazy<Settings>(() => Load());
        public static Settings Setting
        {
            get { return _data.Value; }
        }

        Settings()
        {
        }

        [XmlElement(ElementName = "SendIP")]
        public string SendIP
        {
            get
            {
                if (!_props.ContainsKey("SendIP")) _props.Add("SendIP", new NotifyingProperty("SendIP", typeof(string), "192.168.1.12"));
                return (string)GetValue(_props["SendIP"]);
            }
            set
            {
                if (!_props.ContainsKey("SendIP")) _props.Add("SendIP", new NotifyingProperty("SendIP", typeof(string), "192.168.1.12"));
                SetValue(_props["SendIP"], value);
            }
        }

        [XmlElement(ElementName = "SndPort")]
        public int SndPort
        {
            get
            {
                if (!_props.ContainsKey("SndPort")) _props.Add("SndPort", new NotifyingProperty("SndPort", typeof(int), 9200));
                return (int)GetValue(_props["SndPort"]);
            }
            set
            {
                if (!_props.ContainsKey("SndPort")) _props.Add("SndPort", new NotifyingProperty("SndPort", typeof(int), 9200));
                SetValue(_props["SndPort"], value);
            }
        }

        [XmlElement(ElementName = "RcvPort")]
        public int RcvPort
        {
            get
            {
                if (!_props.ContainsKey("RcvPort")) _props.Add("RcvPort", new NotifyingProperty("RcvPort", typeof(int), 9200));
                return (int)GetValue(_props["RcvPort"]);
            }
            set
            {
                if (!_props.ContainsKey("RcvPort")) _props.Add("RcvPort", new NotifyingProperty("RcvPort", typeof(int), 9200));
                SetValue(_props["RcvPort"], value);
            }
        }

        public static string GetExePathInDirectory(string dirname)
        {
            string str = System.Windows.Forms.Application.ExecutablePath;
            string path = Path.Combine(Path.GetDirectoryName(str), dirname);
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                try
                {
                    dir.Create();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
            return dir.FullName;
        }


        private static string GetSettingFileName()
        {
            string str = System.Windows.Forms.Application.ExecutablePath;
            string path = Path.GetDirectoryName(str);

            int idx = str.LastIndexOf('\\');
            string name = System.Windows.Forms.Application.ProductName;
            if (idx >= 0)
            {
                str = str.Remove(0, idx + 1);
                if (!string.IsNullOrEmpty(str))
                {
                    idx = str.LastIndexOf('.');
                    if (idx >= 0)
                    {
                        str = str.Remove(idx, str.Length - idx);
                        if (!string.IsNullOrEmpty(str) && name != str)
                        {
                            name = str;
                        }
                    }
                }
            }

            return Path.Combine(path, string.Format("{0}_setting.xml", name));
        }
        public static Settings Load()
        {

            string filename = GetSettingFileName();

            Settings result = null;
            if (!File.Exists(filename))
            {
                result = new Settings();
            }
            else
            {
                FileStream fs = new FileStream(filename, FileMode.Open);
                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreWhitespace = true;
                set.IgnoreProcessingInstructions = true;
                set.IgnoreComments = true;

                XmlReader xr = XmlReader.Create(fs, set);
                try
                {
                    XmlSerializer sx = new XmlSerializer(typeof(Settings));

                    if (sx.CanDeserialize(xr))
                    {
                        result = (Settings)sx.Deserialize(xr);

                    }
                }
                catch (XmlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    result = new Settings();
                }
                finally
                {
                    fs.Close();
                }
            }
            return result;
        }
        public void Save()
        {
            //Properties.Settings.Default.Save();

            string filename = GetSettingFileName();

            FileStream fs = new FileStream(filename, FileMode.Create);
            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;
            set.WriteEndDocumentOnClose = true;

            XmlWriter wr = XmlWriter.Create(fs, set);
            try
            {
                XmlSerializer sx = new XmlSerializer(typeof(Settings));
                sx.Serialize(wr, Settings.Setting);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {

                fs.Close();
            }

        }

    }
}
