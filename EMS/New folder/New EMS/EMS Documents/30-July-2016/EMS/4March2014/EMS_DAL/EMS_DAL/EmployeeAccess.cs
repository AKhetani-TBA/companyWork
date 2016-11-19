#region Includes
	using System;
	using System.Data;
	using System.Collections.Generic;
	using Microsoft.Practices.EnterpriseLibrary.Data;
	using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
	using Microsoft.Practices.EnterpriseLibrary.Common;
	using System.Configuration;
	using System.Globalization;
	using System.Data.Common;
#endregion

namespace VCM.EMS.Dal
{
    [Serializable]
    public class EmployeeAccess
    {
        public DataTable EmpDetails;
        private TimeSpan EmpRegularDay;
        private TimeSpan EmpSaturday;
        public void CreateTable(string iMonth, int iYear)
        {
            EmpRegularDay = TimeSpan.FromTicks(0);
            EmpSaturday = TimeSpan.FromTicks(0);

            EmpDetails = new DataTable();
            EmpDetails.Columns.Add("Index", System.Type.GetType("System.Int32"));
            EmpDetails.Columns.Add("Date", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log1", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log2", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("CheckIn", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("CheckOut", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("Duration", System.Type.GetType("System.TimeSpan"));
            EmpDetails.Columns.Add("DurationOutTime", System.Type.GetType("System.TimeSpan"));
            // EmpDetails.Columns.Add("relv", System.Type.GetType("System.Int32"));

            DateTime dtTemp = new DateTime();
            dtTemp = DateTime.Parse(iMonth + "/1/" + iYear.ToString());
            int iDays = dtTemp.AddMonths(1).DayOfYear - dtTemp.DayOfYear < 0 ? 31 : dtTemp.AddMonths(1).DayOfYear - dtTemp.DayOfYear;

            for (int iCounter = 1; iCounter <= iDays; iCounter++)
            {
                EmpDetails.Rows.Add(iCounter, dtTemp.ToString("dd-MMMM-yyyy"), "");
                dtTemp = dtTemp.AddDays(1);
            }
        }   
        public void CreateTable(DateTime dtSelect)
        {
            EmpRegularDay = TimeSpan.FromTicks(0);
            EmpSaturday = TimeSpan.FromTicks(0);
            EmpDetails = new DataTable();
            EmpDetails.Columns.Add("Index", System.Type.GetType("System.Int32"));
            EmpDetails.Columns.Add("Date", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log1", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log2", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("CheckIn", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("CheckOut", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("Duration", System.Type.GetType("System.TimeSpan"));
            EmpDetails.Columns.Add("DurationOutTime", System.Type.GetType("System.TimeSpan"));
            EmpDetails.Rows.Add(1, dtSelect.ToString("dd-MMMM-yyyy"), "");
        }
        //table having fields CheckOutLog=Log for outTime greater than X minutes,LogCounter= no of times 
        public void CreateTable2(string iMonth, int iYear)
        {
            EmpRegularDay = TimeSpan.FromTicks(0);
            EmpSaturday = TimeSpan.FromTicks(0);

            EmpDetails = new DataTable();
            EmpDetails.Columns.Add("Index", System.Type.GetType("System.Int32"));
            EmpDetails.Columns.Add("Date", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log1", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("Log2", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("CheckIn", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("CheckOut", System.Type.GetType("System.DateTime"));
            EmpDetails.Columns.Add("Duration", System.Type.GetType("System.TimeSpan"));
            EmpDetails.Columns.Add("CheckOutLog", System.Type.GetType("System.String"));
            EmpDetails.Columns.Add("LogCounter", System.Type.GetType("System.Int32"));
            EmpDetails.Columns.Add("DurationOutTime", System.Type.GetType("System.TimeSpan"));

            DateTime dtTemp = new DateTime();
            dtTemp = DateTime.Parse(iMonth + "/1/" + iYear.ToString());
            int iDays = dtTemp.AddMonths(1).DayOfYear - dtTemp.DayOfYear < 0 ? 31 : dtTemp.AddMonths(1).DayOfYear - dtTemp.DayOfYear;

            for (int iCounter = 1; iCounter <= iDays; iCounter++)
            {
                EmpDetails.Rows.Add(iCounter, dtTemp.ToString("dd-MMMM-yyyy"), "");
                dtTemp = dtTemp.AddDays(1);
            }
        }
        public void AddDetails(int iDate, DateTime dtCheckIn, DateTime dtCheckOut, TimeSpan dtDuration, String strTooltip, string strOutsideLog, TimeSpan totalOutOffice)
        {
            iDate--;
            EmpDetails.Rows[iDate]["Log1"] = strTooltip;
            EmpDetails.Rows[iDate]["Log2"] = strOutsideLog;
            EmpDetails.Rows[iDate]["CheckIn"] = dtCheckIn.ToString("hh:mm:ss tt");
            EmpDetails.Rows[iDate]["CheckOut"] = dtCheckOut.ToString("hh:mm:ss tt");
            EmpDetails.Rows[iDate]["Duration"] = dtDuration;
            EmpDetails.Rows[iDate]["DurationOutTime"] = totalOutOffice;
        }
        public void AddDetails(int iDate, DateTime dtCheckIn, DateTime dtCheckOut, TimeSpan dtDuration, String strTooltip, string strOutsideLog, TimeSpan totalOutOffice, int relv)
        {
            iDate--;
            EmpDetails.Rows[iDate]["Log1"] = strTooltip;
            EmpDetails.Rows[iDate]["Log2"] = strOutsideLog;
            EmpDetails.Rows[iDate]["CheckIn"] = dtCheckIn.ToString("yyyy-MMM-dd hh:mm:ss tt");
            EmpDetails.Rows[iDate]["CheckOut"] = dtCheckOut.ToString("yyyy-MMM-dd hh:mm:ss tt");
            EmpDetails.Rows[iDate]["Duration"] = dtDuration;
            EmpDetails.Rows[iDate]["DurationOutTime"] = totalOutOffice;
            // EmpDetails.Rows[iDate]["relv"] = relv;
        }
        //add details to EmpDetails table having fields CheckOutLog=Log for outTime greater than X minutes,LogCounter= no of times 
        public void AddDetails2(int iDate, DateTime dtCheckIn, DateTime dtCheckOut, TimeSpan dtDuration, String strTooltip, string strOutsideLog, string strLogRecord, int LogCount, TimeSpan totalOutOffice)
        {
            iDate--;
            EmpDetails.Rows[iDate]["Log1"] = strTooltip;
            EmpDetails.Rows[iDate]["Log2"] = strOutsideLog;
            EmpDetails.Rows[iDate]["CheckIn"] = dtCheckIn.ToString("yyyy-MMM-dd hh:mm:ss tt");
            EmpDetails.Rows[iDate]["CheckOut"] = dtCheckOut.ToString("yyyy-MMM-dd hh:mm:ss tt");
            EmpDetails.Rows[iDate]["Duration"] = dtDuration;
            EmpDetails.Rows[iDate]["CheckOutLog"] = strLogRecord;
            EmpDetails.Rows[iDate]["LogCounter"] = LogCount;
            EmpDetails.Rows[iDate]["DurationOutTime"] = totalOutOffice;
        }
        public void AddDetails(TimeSpan dtRegularday, TimeSpan dtSaturday)
        {
            EmpRegularDay = dtRegularday;
            EmpSaturday = dtSaturday;
        }
        public void GetDetails(ref TimeSpan dtRegularday, ref TimeSpan dtSaturday)
        {
            dtRegularday = EmpRegularDay;
            dtSaturday = EmpSaturday;
        }
        public bool GetDetails(int iDate, ref  DateTime dtCheckIn, ref DateTime dtCheckOut, ref TimeSpan dtDuration, ref String strLog1, ref string strLog2)
        {

            CultureInfo culture = new CultureInfo("en-US");
            //myDTFI.ShortDatePattern = "yyyy-MM-dd hh:mm:ss";

            for (int iCounter = 0; iCounter < EmpDetails.Rows.Count; iCounter++)
            {
                if ((EmpDetails.Rows[iCounter]["CheckIn"].ToString().Length != 0))// &&
                {
                    int iTempDate = Convert.ToDateTime(EmpDetails.Rows[iCounter]["Date"].ToString(), culture).Day;
                    if (iTempDate == iDate)
                    {
                        dtCheckIn = DateTime.Parse(EmpDetails.Rows[iCounter]["CheckIn"].ToString());
                        dtCheckOut = DateTime.Parse(EmpDetails.Rows[iCounter]["CheckOut"].ToString());
                        dtDuration = TimeSpan.Parse(EmpDetails.Rows[iCounter]["Duration"].ToString());
                        strLog1 = EmpDetails.Rows[iCounter]["Log1"].ToString();
                        strLog2 = EmpDetails.Rows[iCounter]["Log2"].ToString();
                        return true;
                    }
                }
            }
            return false;
        }
      
    }
}