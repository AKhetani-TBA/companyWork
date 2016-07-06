using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelAddIn5
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            // this.KeyPreview = true;
            InitializeComponent();
        }
        /// <summary>
        /// Compare New Password and Confirm Password text.
        /// </summary>
        /// <returns>True, if both are matching. Else, false.</returns>
        private bool ComparePassword()
        {
            bool arePasswordsMatching = true;
            if (txtNewPwd.Text.Equals(txtConfirmNewPwd.Text))
            {
                arePasswordsMatching = true;
            }
            else
            {
                arePasswordsMatching = false;
            }
            return arePasswordsMatching;
        }
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            ExcelAPIHandler excelAPIHandler = new ExcelAPIHandler();

            if (txtNewPwd.Text != string.Empty && txtCurrentPwd.Text != string.Empty && txtConfirmNewPwd.Text != string.Empty)
            {
                if (Utilities.Instance.Password == txtCurrentPwd.Text)
                {
                    if (!ComparePassword())
                    {
                        Messagecls.AlertMessage(4, "");
                        return;
                    }
                    if ((IsValidPassword(txtNewPwd.Text)) && (IsValidPassword(txtConfirmNewPwd.Text)))
                    {
                        if (txtNewPwd.Text == txtConfirmNewPwd.Text)//new password is equal to confirm password
                        {
                            //int result =0;// = AuthenticationManager.Instance.objservice.ChangePassword(Convert.ToInt32(AuthenticationManager.Instance.userID), txtCurrentPwd.Text, txtNewPwd.Text);
                           int  result =  excelAPIHandler.ChangePassword(AuthenticationManager.Instance.userEmailID, txtCurrentPwd.Text, txtNewPwd.Text);
                            
                            if (result == 1)
                            {
                                this.Close();
                                // new Login().Show();
                                Utilities.Instance.Password = txtNewPwd.Text;
                                Utilities.Instance.WriteCredentialonRestry(AuthenticationManager.Instance.userEmailID, txtNewPwd.Text);
                                //MessageBox.Show("Password changed successfully.");
                                Messagecls.AlertMessage(2, "");
                            }
                            else
                            {
                                Messagecls.AlertMessage(3, "");
                                //  MessageBox.Show("Invalid Password. Please try again.");
                            }
                            // }
                        }
                        else
                        {
                            Messagecls.AlertMessage(4, "");
                            // MessageBox.Show("Passwords entered do not match. Please reenter the new password.");
                        }
                    }
                    else
                    {
                        Messagecls.AlertMessage(5, "");
                        // MessageBox.Show("Password must have a minimum of 8 and a maximum 20 characters. It must contain at least 1 uppercase letter, 1 lowercase letter and 1 numeric digit each.");
                    }
                }
                else
                {
                    Messagecls.AlertMessage(3, "");
                    // MessageBox.Show("Invalid Password. Please try again.");
                }
            }
            else
            {
                if (txtNewPwd.Text == string.Empty)
                {
                    // MessageBox.Show("New password is required.");
                    Messagecls.AlertMessage(6, "");
                }
                if (txtConfirmNewPwd.Text == string.Empty)
                {
                    Messagecls.AlertMessage(7, "");
                    // MessageBox.Show("Confirm new password is required.");
                }
                if (txtCurrentPwd.Text == string.Empty)
                {
                    Messagecls.AlertMessage(8, "");
                    // MessageBox.Show("Current password is required.");
                }
                if (txtNewPwd.Text != string.Empty && txtCurrentPwd.Text != string.Empty && txtConfirmNewPwd.Text != string.Empty)
                {
                    Messagecls.AlertMessage(9, "");
                    //  MessageBox.Show("Password is required.");
                }

            }
        }
        private bool IsValidPassword(string password)
        {
            return (password.Length >= 8 &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit)
                );
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
