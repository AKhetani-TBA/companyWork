using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Beast_Barclay_Addin
{
    /// <summary>
    /// ProposedLimit
    /// </summary>
    public partial class ProposedLimit : Form
    {
        #region private Verb..
        /// <summary>
        /// string _userEmailId.
        /// </summary>
        private string _userEmailId = string.Empty;
        /// <summary>
        /// Array of string for _userName.
        /// </summary>
        private readonly string[] _userName;

        #endregion

        #region Public Verb..
        /// <summary>
        /// dynamic Utils
        /// </summary>
        private readonly dynamic _utils;
        /// <summary>
        /// string For UserEmailId.
        /// </summary>
        public string UserEmailId
        {
            get { return _userEmailId; }
        }

        #endregion

        #region Constructer..
        /// <summary>
        /// ProposedLimit.
        /// </summary>
        /// <param name="userName">Specfies string argument for userName <B>userName As A,B,C</B>></param>
        public ProposedLimit(string userName)
        {
            InitializeComponent();
            _userName = userName.Split(',');
            object addinRef = "TheBeastAppsAddin";
            var beastAddin = Globals.ThisAddIn.Application.COMAddIns.Item(ref addinRef);
            _utils = beastAddin.Object;
            AddControlInForm();
        }
        /// <summary>
        /// ProposedLimit
        /// </summary>
        public ProposedLimit()
        {
            InitializeComponent();
        }

        #endregion

        #region private Methods..
        /// <summary>
        /// AddControlInForm
        /// </summary>
        private void AddControlInForm()
        {
            try
            {
                var counterControl = 1;
                foreach (var users in _userName)
                {
                    var lbl = new Label
                    {
                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                        Text = users,
                        Name = users,
                        Size = new Size(150, 13),
                        TextAlign = ContentAlignment.BottomRight
                    };
                    AddControlTableLayout.Controls.Add(lbl, 0, counterControl - 1);
                    var txt = new TextBox
                    {
                        Name = users + "EmailId",
                        Anchor = AnchorStyles.Left | AnchorStyles.Right
                    };
                    txt.TextChanged += EmailId_TextChanged;
                    txt.Dock = DockStyle.Fill;
                    txt.Size = new Size(250, 13);
                    AddControlTableLayout.Controls.Add(txt, 1, counterControl - 1);
                    counterControl++;
                }
                AddControlTableLayout.Padding = new Padding(5, 5, SystemInformation.VerticalScrollBarWidth, 5);
                AddControlTableLayout.ResumeLayout();
                AddControlTableLayout.PerformLayout();
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("ProposedLimit.cs", "UpdateExcel();", ex.Message, ex);
            }
        }
        /// <summary>
        /// EmailValidation
        /// </summary>
        /// <param name="ctxt">emailId string.</param>
        /// <returns>return True and false.</returns>
        private static bool EmailValidation(string ctxt)
        {
            if (
                Regex.Match(ctxt,
                    "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,3})$",
                    RegexOptions.IgnoreCase).Success)
                return true;
            else
            {
                string errorMessage = "Please enter valid Email address";
                MessageBox.Show(errorMessage, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        #endregion

        #region Private Events..
        /// <summary>
        /// Ok_Click.
        /// </summary>
        /// <param name="sender">Object argument for sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void Ok_Click(object sender, EventArgs e)
        {
            try
            {
                const int arrayIndex = 0;
                foreach (var ctrl in AddControlTableLayout.Controls.OfType<TextBox>())
                {
                    if (string.IsNullOrEmpty(ctrl.Text.Trim()))
                    {
                        ctrl.Focus();
                        errprovider.SetError(ctrl, "Enter Email Id");
                        return;
                    }
                    if (!EmailValidation(ctrl.Text.Trim()))
                    {
                        ctrl.Focus();
                        errprovider.SetError(ctrl, "Enter Email Id");
                        return;
                    }
                    _userEmailId = _userEmailId == string.Empty
                        ? _userName[arrayIndex] + "=" + ctrl.Text.Trim()
                        : _userEmailId + "," + _userName[arrayIndex] + "=" + ctrl.Text.Trim();
                }
                Close();
            }
            catch (Exception ex)
            {
                _utils.ErrorLog("ProposedLimit.cs", "UpdateExcel();", ex.Message, ex);
            }
        }
        /// <summary>
        /// EmailId_TextChanged
        /// </summary>
        /// <param name="sender">Object argument for the sender.</param>
        /// <param name="e">Events argument for the e.</param>
        private void EmailId_TextChanged(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            if (box != null)
            {
                errprovider.SetError(box, "");
            }
        }

        /// <summary>
        /// ProposedLimit_FormClosing
        /// </summary>
        /// <param name="sender">Objecvt argument for the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void ProposedLimit_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = _userEmailId == string.Empty ? DialogResult.No : DialogResult.Yes;
        }
        /// <summary>
        /// ProposedLimit_KeyUp
        /// </summary>
        /// <param name="sender">Objecvt argument for the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void ProposedLimit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
        /// <summary>
        /// ProposedLimit_KeyUp
        /// </summary>
        /// <param name="sender">Objecvt argument for the sender.</param>
        /// <param name="e">Events argument for e.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
       #endregion
    }
}