using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;


namespace ExcelAddIn1
{
    public partial class LoginForm : Form
    {

        public static System.Drawing.Size ss = Screen.PrimaryScreen.WorkingArea.Size;
        public static int LatestX = 0, LatestY = 0;
        public static int top = 0, left = 0;
        public static int height = 130 , width = 400;
        int checkNotification=0;

        public LoginForm()
        {
            InitializeComponent();
            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }


        private void btn_Login_Click(object sender, EventArgs e)
        {
            #region Move Load Login Form from there location

            LoginForm loadForm = new LoginForm();

            if (this.Location.X + width < ss.Width)
            {
                loadForm.Location = new System.Drawing.Point(this.Location.X + width, this.Location.Y);
            }
            else
            {
                loadForm.Location = new System.Drawing.Point(0, this.Location.Y + height);
            }
            if (loadForm.Location.Y + height > ss.Height)
            {
                loadForm.Location = new System.Drawing.Point(0, 0);
            }

            loadForm.Show();
            
            #endregion

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
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
