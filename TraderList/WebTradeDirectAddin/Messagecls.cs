using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebTradeDirectAddin
{
    static class Messagecls
    {
        public static void AlertMessage(int MessageID, string Message)
        {
            switch (MessageID)
            {
                case 1:
                    MessageBox.Show("A worksheet by name " + Message + " already exists. Please delete that sheet to continue.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    MessageBox.Show("Login is must to perform this action.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 3:
                    MessageBox.Show("You must select an action, cusip and quantity to Get Price(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 4:
                    MessageBox.Show("You must select an action, cusip, quantity and price to Submit Order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 5:
                    MessageBox.Show("Please login for Get Top Of the Book(TradeWeb).!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 6:
                    MessageBox.Show("You must select valid cusip(s) to Get Top Of the Book(TradeWeb).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 7:
                    MessageBox.Show("You must select valid cusip(s) to Get Depth Of The Book(TradeWeb).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 8:
                    MessageBox.Show("You can not submit/modify a filled or an expired order.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 9:
                    MessageBox.Show("You can not cancel order(s). after submitted order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 10:
                    MessageBox.Show("You can not get order price if order is submitted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 11:
                    MessageBox.Show("Invalid Request for Get Order Price. Please check Action, Cusip and Qty. Action must be BUY or SELL. Cusip must be 9 characters. Qty must be greater than or equal to 1.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 12:
                    MessageBox.Show("Invalid Request for Submit/Modify order. Please check Qty and Price.Cusip must be 9 characters. Qty must be greater than or equal to 1. Price should not be 0 or N/A.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 13:
                    MessageBox.Show("Invalid request for submit order, Please review the items you are submit for order.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 14:
                    MessageBox.Show("You can not submit order.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 15:
                    MessageBox.Show("Invalid Request for Cancel Order(s). Selected cell(s) must contain valid order id(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 16:
                    MessageBox.Show("Invalid request for get order status,One or more order id(s) are not found.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 17:
                    MessageBox.Show("Invalid request for cancelletion, Please review the items you are cancelling.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 18:
                    MessageBox.Show("Multiple cell selection using Ctrl Key is not supported. You can use mouse selection instead.", "Ctrl Key!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 19:
                    MessageBox.Show("Invalid Request for Submit order.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 20:
                    MessageBox.Show("Invalid Request for Cancel Order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 21:
                    MessageBox.Show("Please login for Pull order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 22:
                    MessageBox.Show("Invalid Request for Pull Order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 23:
                    //MessageBox.Show("Invalid Request for Publish Order(s).", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    MessageBox.Show("Invalid values provided for quantity/price. Re-enter quantity/price.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 24:
                    MessageBox.Show("Multiple cell selection using Ctrl + Shift Key is not supported. You can use mouse selection instead.", "Ctrl + Shift Key!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                case 25:
                    MessageBox.Show("Only 2500 cusips supported at a time in Top of the Book.", "Cusip Max Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                case 26:
                    MessageBox.Show("Only 500 cusips supported at a time in Depth of the Book.", "Cusip Max Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                case 27:
                    MessageBox.Show("Selected cusip is invalid for publish market", "Selected cusip is invalid for publish market", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                case 28:
                    MessageBox.Show(" You are not allowed to publish offers more than " + Message + "MM.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                case 29:
                    MessageBox.Show("Your daily maximum limit is exceeded, you are not allowed to publish offers.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    break;
                default:
                    break;
            }
        }
    }
}
