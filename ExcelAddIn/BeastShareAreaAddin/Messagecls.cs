using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeastShareAreaAddin
{
    static class Messagecls
    {
        public static void AlertMessage(int MessageID, string Message) 
        {
            switch (MessageID)
            {
                case 1:
                    MessageBox.Show("Internet connection required. Please connect to internet and retry.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 2:
                    MessageBox.Show("Password changed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 3:
                    MessageBox.Show("Invalid Password. Please try again.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 4:
                    MessageBox.Show("Passwords entered do not match. Please reenter the new password.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 5:
                    MessageBox.Show("Password must have a minimum of 8 and a maximum 20 characters. It must contain at least 1 uppercase letter, 1 lowercase letter and 1 numeric digit each.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 6:
                    MessageBox.Show("New password is required.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 7:
                    MessageBox.Show("Confirm new password is required.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 8:
                    MessageBox.Show("Current password is required.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 9:
                    MessageBox.Show("Password is required.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 10:
                    MessageBox.Show("You are already logged in.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 11:
                    MessageBox.Show("Login is must to perform this action.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 12:
                    MessageBox.Show("Please select a category and an app to start.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 13:
                    MessageBox.Show("Please select a calculator and an app to start.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 14:
                    MessageBox.Show("Invalid sheet to place calculator.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 15:
                    MessageBox.Show("Please select a cell that belongs to the app for sharing an app.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 16:
                    MessageBox.Show("This addin is already loaded.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 17:
                    MessageBox.Show("Please enter a valid login and password.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 18:
                    MessageBox.Show("Application disconnected from server. Please check your connection.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 19:
                    MessageBox.Show("An instance of this app is already running in Excel.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 20:
                    MessageBox.Show("The Excel cell where you want to place the app must be empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 21:
                    MessageBox.Show("An another app is already running at the selected cell in Excel. Please select some other empty cell instead.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 22:
                    MessageBox.Show("Please login first before playing top of the book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 23:
                    MessageBox.Show("Please enter email(s) in correct format.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 24:
                    MessageBox.Show("Your credentials have been changed.you have to relogin to continue.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 25:
                    MessageBox.Show("Provide atleast one email to proceed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    break;
            }
        }
    }
}
