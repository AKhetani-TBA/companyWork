using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using Microsoft.Office.Interop.Excel;

namespace Beast_RFQ_Addin
{
    public partial class SubmitQuotes : BaseForm
    {
        public string[,] Arraytread { get; set; }
        public string ReturnXml { get; set; }
        public SubmitQuoteXML SubmitQuoteXml = new SubmitQuoteXML();

        public SubmitQuotes()
        {
            InitializeComponent();
        }

        public SubmitQuotes(string[,] arrofTreade)
        {
            InitializeComponent();
            Arraytread = arrofTreade;
        }

        private void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxEmailList.SelectedItems.Count > 0)
                {
                    Microsoft.Office.Interop.Excel.Range selectedRange = (Range)Globals.ThisAddIn.Application.Selection;
                    SubmitQuoteXml.Action = "SubmitQuotes";
                    SubmitQuoteXml.QuotesList = new Quotes();
                    SubmitQuoteXml.QuotesList.Q = new List<Q>();

                    SubmitQuoteXml.ClientList = new Clients();
                    SubmitQuoteXml.ClientList.C = new List<C>();
                    string createXml = string.Empty;

                    SubmitQuoteXml.ClientList.C.Clear();
                    listBoxEmailList.SelectedItems.OfType<string>().ToList().ForEach(x =>
                    {
                        SubmitQuoteXml.ClientList.C.Add(new C()
                        {
                            EmailId = x
                        });
                    });

                    SubmitQuoteXml.QuotesList.Q.Clear();
                    foreach (Range cell in selectedRange.Rows)
                    {
                        SubmitQuoteXml.QuotesList.Q.Add(new Q()
                            {
                                ID = Convert.ToString(Globals.ThisAddIn.Application.Cells[cell.Row, Shared.ColumnCUSIP].Value),
                                Side = Convert.ToString(Globals.ThisAddIn.Application.Cells[cell.Row, Shared.ColumnSIDE].Value),
                                Quantity = Convert.ToInt32(Globals.ThisAddIn.Application.Cells[cell.Row, Shared.ColumnQTY].Value),
                                RequestId = Convert.ToInt32(Globals.ThisAddIn.Application.Cells[cell.Row, Shared.ColumnREQUESTID].Value)
                            });
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select at least one Email Id", "Submit Quote(s)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                RFQUtility.Instance.utils.ErrorLog("SubmitQuotes.cs", "btnSubmitOrder_Click();", ex.Message, ex);
                this.Close();

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.SubmitQuoteXml = null;
            this.Close();
        }

        private void SubmitOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Validates email string.
        /// </summary>
        /// <param name="emailString">Email String</param>
        /// <param name="errorMessage">Error Message</param>
        /// <returns></returns>
        private bool IsValidEmail(string emailString, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isValid = true;
            string regexExpression =
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            if (string.IsNullOrEmpty(emailString))
            {
                errorMessage = "E-Mail string found to be empty, Please provide E-Mail.";
                isValid = false;
            }
            if (!Regex.IsMatch(emailString, regexExpression, RegexOptions.IgnoreCase))
            {
                errorMessage = "E-Mail string found to be invalid, Please provide valid E-Mail.";
                isValid = false;
            }
            return isValid;
        }

        private void buttonAddEmailId_Click(object sender, EventArgs e)
        {
            string emailString = textBoxEmailId.Text;
            string errorMessage = string.Empty;
            if (IsValidEmail(emailString, out errorMessage))
            {
                if (!listBoxEmailList.Items.Contains(emailString))
                {
                    listBoxEmailList.Items.Add(emailString);
                    textBoxEmailId.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("E-mail found to be redundant, Please enter fresh E-mail.", "SubmitQuotes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(errorMessage, "SubmitQuotes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SubmitOrder_Load(object sender, EventArgs e)
        {
            LoadEmailIdList();
        }

        private void LoadEmailIdList()
        {
            List<string> emailList = new List<string>();
            for (int i = 0; i < RFQUtility.Instance.AccountDrlst.Rows.Count; i++)
            {
                string tempElementValue = Convert.ToString(RFQUtility.Instance.AccountDrlst.Rows[i]["EleName"]);
                if (!string.IsNullOrEmpty(tempElementValue))
                {
                    emailList.Add(tempElementValue);
                }
            }
            listBoxEmailList.Items.AddRange(emailList.Cast<object>().ToArray());
        }
    }
}
