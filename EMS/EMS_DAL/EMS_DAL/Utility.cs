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
    public class Utility
    {
        public static bool ExtractNewDetails(DataTable dbEmployee, EmployeeAccess empDetails, bool isEmpStatusPage)
        {
            try
            {
                        DateTime dtCheckIn = DateTime.Parse(dbEmployee.Rows[0]["CHECKIN"].ToString());
                        DateTime dtCheckOut = DateTime.Parse(dbEmployee.Rows[0]["CHECKOUT"].ToString());
                       
                        DateTime dtDayCheckIn = dtCheckIn;
                        DateTime dtDayCheckOut = dtCheckOut;

                        TimeSpan tsOffice = new TimeSpan(0);
                        TimeSpan totalOutOffice = new TimeSpan(0);
                
                        int iRegDayMinutes = 0;
                        int iSatDayMinutes = 0;
                        int iSatCount = 0;
                        int iRegDayCount = 0;
                        if (((int)dtCheckIn.DayOfWeek) == 6)
                        {
                            if (dtCheckIn.Date != DateTime.Now.Date)
                            {
                                iSatDayMinutes = int.Parse(dbEmployee.Rows[0]["DURATION"].ToString());
                                iSatCount++;
                            }
                            tsOffice = TimeSpan.FromMinutes(int.Parse(dbEmployee.Rows[0]["DURATION"].ToString()));
                        }
                        else
                        {
                            if (dtCheckIn.Date != DateTime.Now.Date)
                            {
                                iRegDayMinutes = int.Parse(dbEmployee.Rows[0]["DURATION"].ToString());
                                iRegDayCount++;
                            }
                            tsOffice = TimeSpan.FromMinutes(int.Parse(dbEmployee.Rows[0]["DURATION"].ToString()));

                        }
                        string strLog = dtCheckIn.ToString("hh:mm") + " - " + dtCheckOut.ToString("hh:mm") + " = " + Convert.ToDateTime(dtCheckOut.Subtract(dtCheckIn).ToString()).ToString("hh:mm") + "\n";
                        string strLog2 = "";
                        DateTime dtLastCheckOut = dtCheckOut;
                        for (int iCounter = 1; iCounter < dbEmployee.Rows.Count; iCounter++)
                        {
                            if (dtCheckIn.Day == 20)
                            {
                                int itest = 0;
                            }
                            dtCheckIn = DateTime.Parse(dbEmployee.Rows[iCounter]["CHECKIN"].ToString());
                            dtCheckOut = DateTime.Parse(dbEmployee.Rows[iCounter]["CHECKOUT"].ToString());
                           // relv = dbEmployee.Rows[iCounter]["relv"].ToString().Equals("") ? 0 : Convert.ToInt32(dbEmployee.Rows[iCounter]["relv"].ToString());
                            if ((dtDayCheckIn.Day != dtCheckOut.Day) && (!((dtDayCheckIn.Day == (dtCheckOut.Day - 1)) &&
                                (dtCheckOut.Hour == 0) && (dtCheckOut.Minute == 0) && (dtCheckOut.Second == 0))))
                            {
                                //strLog2 += "\n Total Out Time : " + //totalOutOffice.ToString();
                                //System.Convert.ToDateTime(totalOutOffice.ToString()).ToString("HH:mm");

                                empDetails.AddDetails(dtDayCheckIn.Day, dtDayCheckIn, dtDayCheckOut, tsOffice, strLog, strLog2, totalOutOffice);
                                dtDayCheckIn = dtCheckIn;
                                dtDayCheckOut = dtCheckOut;
                                strLog = "";
                                strLog2 = "";
                                tsOffice = new TimeSpan(0);
                                //reset the out office time
                                totalOutOffice = TimeSpan.Zero;

                                if ((((int)dtCheckIn.DayOfWeek) == 6) && (dtCheckIn.Date != DateTime.Now.Date))
                                    iSatCount++;
                                else if (dtCheckIn.Date != DateTime.Now.Date)
                                    iRegDayCount++;
                            }
                            if (((int)dtCheckIn.DayOfWeek) == 6)
                            {
                                if (dtCheckIn.Date != DateTime.Now.Date)
                                    iSatDayMinutes += int.Parse(dbEmployee.Rows[iCounter]["DURATION"].ToString());
                                tsOffice += TimeSpan.FromMinutes(int.Parse(dbEmployee.Rows[iCounter]["DURATION"].ToString()));
                            }
                            else
                            {
                                if (dtCheckIn.Date != DateTime.Now.Date)
                                    iRegDayMinutes += int.Parse(dbEmployee.Rows[iCounter]["DURATION"].ToString());
                                tsOffice += TimeSpan.FromMinutes(int.Parse(dbEmployee.Rows[iCounter]["DURATION"].ToString()));
                            }

                            if (strLog.Length != 0)
                            {
                                try
                                {
                                    strLog2 += dtLastCheckOut.ToString("hh:mm") + " - " + dtCheckIn.ToString("hh:mm") + " = " + // dtCheckIn.Subtract(dtLastCheckOut) + "\n";
                                        // dtCheckIn.Subtract(dtLastCheckOut).Hours + ":" + (dtCheckIn.Subtract(dtLastCheckOut).Seconds > 30 ? (dtCheckIn.Subtract(dtLastCheckOut).Minutes + 1) : dtCheckIn.Subtract(dtLastCheckOut).Minutes) + "\n";
                                    (dtCheckIn.Subtract(dtLastCheckOut).Seconds > 30 ?
                                    (DateTime.Parse((dtCheckIn - dtLastCheckOut).ToString()).AddMinutes(1)).ToString("HH:mm")
                                  : DateTime.Parse((dtCheckIn - dtLastCheckOut).ToString()).ToString("HH:mm"))
                                  + "\n";
                                }
                                catch (Exception e)
                                {                                   
                                }
                                totalOutOffice += dtCheckIn.Subtract(dtLastCheckOut);
                            }                           
                            try
                            {
                                strLog += dtCheckIn.ToString("hh:mm") + " - " + dtCheckOut.ToString("hh:mm") + " = " +
                                (dtCheckOut.Subtract(dtCheckIn).Seconds > 30 ?
                                   (DateTime.Parse((dtCheckOut - dtCheckIn).ToString()).AddMinutes(1)).ToString("HH:mm")
                                 : DateTime.Parse((dtCheckOut - dtCheckIn).ToString()).ToString("HH:mm"))
                                 + "\n";
                            }
                            catch (Exception ex)
                            {

                            }
                            dtDayCheckOut = dtCheckOut;
                            dtLastCheckOut = dtCheckOut;
                            //}
                        }
                        //strLog2 += "\n Total Out Time : " + System.Convert.ToDateTime(totalOutOffice.ToString()).ToString("HH:mm");
                        if (isEmpStatusPage == true)
                            empDetails.AddDetails(1, dtDayCheckIn, dtDayCheckOut, tsOffice, strLog, strLog2, totalOutOffice);
                        else
                            empDetails.AddDetails(dtDayCheckIn.Day, dtDayCheckIn, dtDayCheckOut, tsOffice, strLog, strLog2, totalOutOffice);

                        TimeSpan tsRegOffice = new TimeSpan(0);
                        TimeSpan tsSatOffice = new TimeSpan(0);

                        if (iRegDayCount != 0)
                            tsRegOffice = TimeSpan.FromMinutes(iRegDayMinutes / iRegDayCount);

                        if (iSatCount != 0)
                            tsSatOffice = TimeSpan.FromMinutes(iSatDayMinutes / iSatCount);

                        empDetails.AddDetails(tsRegOffice, tsSatOffice);
                        return true;
                    }
            catch (Exception ex)
            {
                throw;
            }
        }         
    }
}