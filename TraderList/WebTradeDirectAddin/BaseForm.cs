using System.Windows.Forms;

namespace WebTradeDirectAddin
{
    public partial class BaseForm : Form
    {
        #region Private Ver..
        private string htmlFileName;
        private string baseUrlLocation = string.Empty;
        private string DirectoryName = @"SubmitOrderTradeweb/27/";
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
                    dynamic utils = TWDUtility.Instance.BeastAddin.Object;
                    if (!string.IsNullOrEmpty(utils.GetUrlLocation()) &&
                        !string.IsNullOrEmpty(DirectoryName))
                    {
                        //For Changes get uri..
                        baseUrlLocation = utils.GetUrlLocation() + DirectoryName;
                    }
                    //baseUrlLocation = baseUrlLocation.Replace("SubmitOrderTradeweb/27", "SubmitOrderTradeweb/27");
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
