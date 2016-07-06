using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelAddIn5
{
    public enum CustomAddInsUpdateEvent
    {
        NETWORK_CONNECTIVITY = 0,
        LOGOUT = 1,
        PACKAGE_VERSION = 2
    }

    public enum SignalRConnectionStatus
    {
        SIGNALR_DISCONNECTED = 0,
        SIGNALR_CONNECTED = 1
    }

    public enum CategoryListStatus
    {
        CATEGORY_RETRIEVED = 0,
        CATEGORY_NOTRETRIEVED = 1
    }

    public enum VersionStatusEnum
    {
        NO_UPDATE_NEEDED = 0,
        DOWNLOADING = 1,
        DOWNLOAD_CANCELED = 2,
        VERSION_INFO_NOT_FOUND = 3,
        ERROR_TRY_AGAIN = 4,
        NOT_COMPATIBLE = 5
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class ExeChunk
    {

        private string productVersionField;

        private string noOfChunksField;

        private string chunkNameField;

        private byte[] chunkDataField;

        /// <remarks/>
        public string productVersion
        {
            get
            {
                return this.productVersionField;
            }
            set
            {
                this.productVersionField = value;
            }
        }

        /// <remarks/>
        public string NoOfChunks
        {
            get
            {
                return this.noOfChunksField;
            }
            set
            {
                this.noOfChunksField = value;
            }
        }

        /// <remarks/>
        public string chunkName
        {
            get
            {
                return this.chunkNameField;
            }
            set
            {
                this.chunkNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] chunkData
        {
            get
            {
                return this.chunkDataField;
            }
            set
            {
                this.chunkDataField = value;
            }
        }
    }

}
