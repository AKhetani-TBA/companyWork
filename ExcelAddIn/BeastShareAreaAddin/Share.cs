using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace BeastShareAreaAddin
{
    public partial class Share : Form
    {
        Int32 index = -1;

        public Share()
        {

            this.KeyPreview = true;
            InitializeComponent();
           //txtEmailId.Text = "nverma@thebeastapps.com";
        }

        #region Share events
        private void Share_Load(object sender, EventArgs e)
        {

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (EmailValidation(txtEmailId) == true)
                {
                    if (txtEmailId.Text != string.Empty)
                    {

                        if (index != -1)
                        {
                            LBEmailID.Items[index] = txtEmailId.Text;
                        }
                        else
                        {
                            LBEmailID.Visible = true;
                            LBEmailID.Items.Add(txtEmailId.Text);
                        }
                        txtEmailId.Clear();

                        index = -1;
                        LBEmailID.ClearSelected();
                        txtEmailId.Focus();
                    }
                }
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 32) { e.Handled = true; }
        }
        public static bool EmailValidation(System.Windows.Forms.TextBox ctxt)
        {
            if (ctxt.Text.Trim() != "")
            {
                Match rex = Regex.Match(ctxt.Text.Trim(' '), "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,3})$", RegexOptions.IgnoreCase);
                if (rex.Success == false)
                {
                    Messagecls.AlertMessage(23, "");
                    ctxt.Focus();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        private void btnShare_Click(object sender, EventArgs e)
        {
            //txtEmailId.Text = "nverma@thebeastapps.com";
            if (LBEmailID.Items.Count > 0 || txtEmailId.Text != "")
            {
                if (EmailValidation(txtEmailId) == true)
                {
                    string Email = "";
                    if (LBEmailID.Items.Count > 0)
                    {
                        foreach (var item in LBEmailID.Items)
                        {
                            Email += item.ToString() + "#";
                        }
                       // if (txtEmailId.Text != "" && LBEmailID.Items.Count == 1 && LBEmailID.Items[0].ToString() != txtEmailId.Text.ToString())
                        if (txtEmailId.Text != "" && !Email.Contains(txtEmailId.Text))
                            Email += txtEmailId.Text + "#";
                    }
                    else
                    {
                        Email += txtEmailId.Text + "#";
                    }
                    if (Email.Length > 0)
                    {
                        BeastShareUtility.Instance.utils.sendShareImageRequest("vcm_calc_exceljsonshare", Email);
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range SelectShareRange = (Range)Globals.ThisAddIn.Application.Selection;
                            ShareCalculator.Instance._ShareRange = SelectShareRange;
                            ShareCalculator.Instance.ExcelShareingArea("I", SelectShareRange);
                        }
                        catch (Exception ex)
                        {
                            BeastShareUtility.Instance.utils.ErrorLog("Share.cs", "btnShare_Click();", ex.Message, ex);
                        }
                        finally
                        {
                            this.Hide();
                        }
                    }
                }
            }
            else
            {
                Messagecls.AlertMessage(25, "");
            }
        }
        private void LBEmailID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LBEmailID.SelectedItem != null)
            {
                txtEmailId.Text = LBEmailID.SelectedItem.ToString();
                index = LBEmailID.SelectedIndex;
            }
        }
        private void LBEmailID_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Delete)
            {
                if (LBEmailID.Items.Count > 0 && LBEmailID.SelectedItem != null)
                {
                    DialogResult dr = MessageBox.Show("Do you want to delete this email address?", "Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        LBEmailID.Items.Remove(LBEmailID.SelectedItem);
                        txtEmailId.Text = "";
                        index = -1;
                        txtEmailId.Focus();
                    }
                    if (LBEmailID.Items.Count == 0)
                    {
                        LBEmailID.Visible = false;
                    }
                }
                else
                {
                    LBEmailID.Visible = false;
                }
            }
            if (e.KeyCode == Keys.Tab)
            {

            }
        }
        private void Share_FormClosed(object sender, FormClosedEventArgs e)
        {
            BeastShareUtility.Instance.IsShare = false;
        }
        private void LBEmailID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                LBEmailID.SelectedIndex = 0;
            }
        }
        private void btnCancel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                LBEmailID.SelectedIndex = -1;
            }
        }
        #endregion

        private void LBEmailID_MouseClick(object sender, MouseEventArgs e) 
        {
            if (LBEmailID.SelectedItem == null)
            {
                if (LBEmailID.Items.Count > 0)
                    LBEmailID.SelectedIndex = 0;
            }
        }
    }
}
