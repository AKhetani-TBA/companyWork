using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace WebServiceExcel
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod (MessageName = "Permission_Of_Users")]
        public DataSet LoginCheck(String Username, String Password)
        {
            if (Username == @"abhi" && Password == @"kunu")
            {
                String ans = @"SELECT * FROM BM_ProcessCPULog,SELECT * FROM ABHISHEK_STUDENT";

                char[] delimiterChars = { ',' };

                string[] UserPermission = ans.Split(delimiterChars);
              
                DataSet dataSet = new DataSet();
                SqlConnection sqlConn; 
                int counter = 0;
                foreach (string token in UserPermission)
                {
                    try
                    {
                        
                        string connectionString = null;
                        string sqlQuery = null;
                        DataTable dt = new DataTable();
                        connectionString = "Data Source=beasttestnu3;Initial Catalog=Trainee;User ID=trainee;Password=trainee";
                        sqlConn = new SqlConnection(connectionString);
                        sqlConn.Open();
                        sqlQuery = token;
                        SqlDataAdapter dscmd = new SqlDataAdapter(sqlQuery, sqlConn);
                        dscmd.Fill(dt);

                        dt.Namespace = counter.ToString();
                            counter++;
                        dataSet.Tables.Add(dt);
                    }
                    catch (Exception Ex)
                    {
                        
                    }
                }
                return dataSet;
            }

            else if (Username == "ishita" && Password == "kunu")
            {
                DataSet dataSet = new DataSet();
                return dataSet;
            }

            DataSet dataSet1 = new DataSet();
            return dataSet1;
        }
    }
}