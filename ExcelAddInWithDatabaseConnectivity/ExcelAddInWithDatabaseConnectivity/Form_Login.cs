using System;
using System.Data;
using System.Windows.Forms;
using ExcelAddInWithDatabaseConnectivity.ExcelWebService;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelAddInWithDatabaseConnectivity
{

    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            string strUsername;
            string strPassword;

            strUsername = txt_Username.Text;
            strPassword = txt_Password.Text;

            //Service1 webService = new Service1();

            //DataSet ds = webService.LoginCheck(strUsername, strPassword);

            //if (ds == null)
            //{
            //    return;
            //}
            //this.Close();

            //foreach (DataTable dt in ds.Tables)
            //{
            //    FillRecordsFromDataTableToExcelSheet(dt);
            //}
        }

        private void FillRecordsFromDataTableToExcelSheet(DataTable dt)
        {
            Excel.Worksheet sheet = Globals.ThisAddIn.Application.Worksheets.Add();
            int columnNo = 1;
            int rowNo = 1;
            columnNo = 1;
            rowNo += 1;
            foreach (DataRow dr in dt.Rows)
            {
                columnNo = 1;
                foreach (DataColumn dc in dt.Columns)
                {
                    sheet.Cells[1, columnNo] = dc.ColumnName;
                    sheet.Cells[rowNo, columnNo] = Convert.ToString(dr[dc.ColumnName]);
                    columnNo++;
                }
                rowNo++;

            }

            sheet.Protect();

        }
    }
}
