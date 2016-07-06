using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace ExcelAddIn5
{
    class DataUtil
    {
        private static volatile DataUtil instance = null;
        public string ProductName = string.Empty;
        public string ProductVersion = string.Empty;
        public string ServerName = string.Empty;
        public string DomainURL = string.Empty;
        public string SignalRHubKey = string.Empty;
        public string DirectoryPath = string.Empty;
        public int LogLevel = 0;
        public bool bIsUserLoggedIn = false;
        public List<string> AddInsList = new List<string>();

        public static DataUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    if (instance == null)
                    {
                        instance = new DataUtil();
                    }
                }
                return instance;
            }
            set
            {
                instance = value;

            }
        }

        public void Init()
        {
        }

        public DataUtil()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            ProductName = fileVersionInfo.ProductName;
            ProductVersion = fileVersionInfo.ProductVersion;

            LogLevel = 0;

            ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"].ToString();

            switch (ServerName.ToUpper())
            {
                case "PRODUCTION":
                    DomainURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PDomainUrl"].ToString());
                    SignalRHubKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PSignslRHubkey"].ToString());
                    break;
                case "DEMO":
                    DomainURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DDomainUrl"].ToString());
                    SignalRHubKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DSignslRHubkey"].ToString());
                    break;
                case "TEST":
                    DomainURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TDomainUrl"].ToString());
                    SignalRHubKey = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["TSignslRHubkey"].ToString());
                    break;
            }

            DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TheBeast\\BeastExcel";
        }
    }
}
