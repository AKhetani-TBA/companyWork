using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelAddIn1
{
    public partial class LoginForm : Form
    {
        int checkNotification;
        
        public LoginForm()
        {
            InitializeComponent();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (txt_Username.Text == "Abhishek" && txt_Password.Text == "Passw0rd@A")
            {
               /* timer1.Interval = (4000); //For checking, I have set the interval to 2 sec. It actually needs to be 15 minutes.
                timer1.Enabled = true;
                timer1.Start();*/

                ShowNotification();
                
                Close();
            }
            else
            {
                MessageBox.Show("Enter the Right Username and Password.");
                
            }
        }

        public void ShowNotification()
        {
            popupNotifier1.Hide();
            popupNotifier1.GradientPower = 1;

            popupNotifier1.TitleText = "Login Successful";
            popupNotifier1.ContentText = @"Welcome " + txt_Username.Text;
            popupNotifier1.ShowCloseButton = true;
            popupNotifier1.ShowOptionsButton = true;
            popupNotifier1.ShowGrip = true;
            popupNotifier1.Delay = 1000;
            popupNotifier1.AnimationInterval = int.Parse("10");
            popupNotifier1.AnimationDuration = int.Parse("1000");
            popupNotifier1.TitlePadding = new Padding(int.Parse("0"));
            popupNotifier1.ContentPadding = new Padding(int.Parse("0"));
            popupNotifier1.ImagePadding = new Padding(int.Parse("0"));
            popupNotifier1.Scroll = true;

            popupNotifier1.Image = Properties.Resources._157_GetPermission_48x48_72;
            popupNotifier1.Popup();

            checkNotification++;
            // showNotify();
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txt_Password.PasswordChar = '\u25CF';
        }
    }
}
