using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_BASE.Models.Utilities
{
    /// <summary>
    /// Read system's configuration settings.
    /// </summary>
    public class Config
    {

        /// <summary>
        /// EMS connection string
        /// </summary>
        private string emsConnectionString = string.Empty;

        /// <summary>
        /// Gets EMS connection string
        /// </summary>
        public string EmsConnectionString
        {
            get
            {
                return this.emsConnectionString;
            }
        }

        /// <summary>
        /// Initialize new instance of <see cref="Config"/> class
        /// </summary>
        
        public Config()
        {
            this.emsConnectionString = ConfigurationManager.ConnectionStrings["EMS"].ToString();
            //this.emsConnectionString = ConfigurationManager.ConnectionStrings["EMS_HR"].ToString();
        }
    }
}
