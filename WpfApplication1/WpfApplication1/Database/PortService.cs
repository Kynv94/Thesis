using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfApplication1.Database
{
    class ServiceInfo
    {
        public int Port { get; set; }
        public string Name { get; set; }
        //public string Type { get; set; }
    }
    class PortService
    {
        public string GetServiceName(int _port)
        {
            var services = ReadServicesFile();
            var portinfo = services.FirstOrDefault(s => s.Port == _port);
            if (portinfo != null)
            {
                return portinfo.Name;
            }
            return "Others";
        }
        private static List<ServiceInfo> ReadServicesFile()
        {
            var sysFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
            if (!sysFolder.EndsWith("\\"))
                sysFolder += "\\";

            var svcFileName = sysFolder + "drivers\\etc\\services";

            var lines = File.ReadAllLines(svcFileName);

            var result = new List<ServiceInfo>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                var info = new ServiceInfo();

                var index = 0;

                // Name
                info.Name = line.Substring(index, 16).Trim();
                index += 16;

                // Port number and type
                var temp = line.Substring(index, 9).Trim();
                var tempSplitted = temp.Split('/');

                info.Port = ushort.Parse(tempSplitted[0]);
                //info.Type = tempSplitted[1].ToLower();

                result.Add(info);
            }

            return result;
        }
    }

}
