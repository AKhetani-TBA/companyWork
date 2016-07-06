using System.Windows.Forms;

namespace Beast_Barclay_Addin
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
                    MessageBox.Show("Please login for Get Top Of the Book.!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 6:
                    MessageBox.Show("You must select valid cusip(s) to Get Top Of The Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 7:
                    MessageBox.Show("You must select valid cusip(s) to Get Depth Of The Book.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Invalid Request for Submit/Modify order. Please check Action, Cusip, Qty and Price. Action must be BUY or SELL. Cusip must be 9 characters. Qty must be greater than or equal to 1. Price should not be 0 or N/A.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                default:
                    break;
            }
        }
    }
}
