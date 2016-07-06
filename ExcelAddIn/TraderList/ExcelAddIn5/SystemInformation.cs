using System;
using System.Management;
using System.Reflection;
using System.Linq;

namespace ExcelAddIn5
{
    public static class SystemInformation
    {
        public static string GetSystemInformation()
        {
            try
            {
                Configurations config = new Configurations();
                config.OSVersion = Environment.OSVersion.ToString();
                config.Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
                config.ProcessorCount = Environment.ProcessorCount;
                config.UserDomainName = Environment.UserDomainName;
                config.UserName = Environment.UserName;
                config.ProductName = System.Windows.Forms.Application.ProductName;
                config.ProductVersion = System.Windows.Forms.Application.ProductVersion;
                config.BeastProductName = Assembly.GetExecutingAssembly().GetName().Name;
                config.BeastProductVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
                config.OsName = name != null ? name.ToString() : "Unknown";

                return config.ToString();

            }
            catch (Exception EX)
            {
                return ("The Following ERROR Occur ..." + EX.Message.ToString());
            }

        }
    }
    public class Configurations
    {
        public string OSVersion { get; set; }
        public bool Is64BitOperatingSystem { get; set; }
        public int ProcessorCount { get; set; }
        public string UserDomainName { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string BeastProductName { get; set; }
        public string BeastProductVersion { get; set; }
        public string OsName { get; set; }

        public override string ToString()
        {
            return "OSName : "+ OsName +", OSVersion : " + OSVersion + ", Is 64bit : " + Is64BitOperatingSystem +", Processor Count : "+ ProcessorCount + ", User Domain : " + UserDomainName + ", User Name : " + UserName + ", Product Name : " +ProductName + ", Product Version : " + ProductVersion + ", Beast Product Name : " + BeastProductName + ", Beast Product Version : " + BeastProductVersion;
        }
    }
}
