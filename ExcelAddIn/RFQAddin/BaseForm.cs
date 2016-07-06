using System.Windows.Forms;

namespace Beast_RFQ_Addin
{
    public partial class BaseForm : Form
    {
        #region Private Ver..
        private string htmlFileName;
        private string baseUrlLocation = string.Empty;
        private string DirectoryName = "SubmitOrderTradeweb/26";
        #endregion

        #region Public ver..
        public string HtmlFileName
        {
            get { return htmlFileName; }
            set { htmlFileName = value; }
        }
        public string BaseUrlLocation
        {
            get
            {
                if (!this.DesignMode)
                {
                    dynamic utils = RFQUtility.Instance.BeastAddin.Object;
                    if (!string.IsNullOrEmpty(utils.GetUrlLocation()) &&
                        !string.IsNullOrEmpty(DirectoryName))
                    {
                        baseUrlLocation = utils.GetUrlLocation() + DirectoryName + @"/";
                    }
                }
                return baseUrlLocation;
            }
            set
            {
                baseUrlLocation = value;
            }
        }
        #endregion

        #region Constructor....
        public BaseForm()
        {
            InitializeComponent();
        }
        #endregion
    }
}
