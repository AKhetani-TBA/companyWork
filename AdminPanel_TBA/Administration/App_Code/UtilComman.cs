using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
//using VCM.Common.Log;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using MaxMind.Db;
using TBA.Utilities;

    public class UtilComman
    {
        public UtilComman()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string GetJSONString(DataTable Dt)
        {
            string retValue = null;
            string HeadStr = string.Empty;
            StringBuilder Sb = null;
            Sb = new StringBuilder();
            try
            {
                string[] StrDc = new string[Dt.Columns.Count];


                for (int i = 0; i < Dt.Columns.Count; i++)
                {

                    StrDc[i] = Dt.Columns[i].Caption;
                    HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";

                }

                HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

                Sb.Append("{\"" + Dt.TableName + "\" : [");

                for (int i = 0; i < Dt.Rows.Count; i++)
                {

                    string TempStr = HeadStr;

                    Sb.Append("{");
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        string tmpStr = Dt.Rows[i][j].ToString();

                        if (tmpStr.Contains("ERROR"))
                        {

                        }
                        tmpStr = tmpStr.Replace('[', ' ').Replace(']', ' ').Replace(@"""", "").Replace(Environment.NewLine, "_").Replace("\r\n", "_").Replace("\n", "_");

                        TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", tmpStr);
                    }
                    Sb.Append(TempStr + "},");

                }
                Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

                Sb.Append("]}");
                retValue = Sb.ToString();
            }
            catch (Exception ex)
            {
                Sb = null;
                HeadStr = null;
                LogUtility.Error("UtilComman.cs", "GetJSONString()", ex.Message, ex);
            }
            finally
            {
                Sb = null;
                HeadStr = null;
            }
            return retValue;
        }

        public DataSet GetInfoFromMaxMind(string pIPAddress)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            if (dt.Columns.Count > 0)
            {
            }
            else
            {
                dt.Columns.Add("Organization");
                dt.Columns.Add("ISP");
                dt.Columns.Add("City");
                dt.Columns.Add("Region");
                dt.Columns.Add("RegionCode");
                dt.Columns.Add("Country");
                dt.Columns.Add("CountryCode");
                dt.Columns.Add("Zipcode");
                dt.Columns.Add("Logintude");
                dt.Columns.Add("Latitude");
                dt.Columns.Add("TimeZone");
                dt.Columns.Add("HostName");
                dt.Columns.Add("IPAddress");
                dt.AcceptChanges();
            }
            DataRow dr = dt.NewRow();

            try
            {
                string url = string.Format("https://geoip.maxmind.com/geoip/v2.1/city/{0}", pIPAddress);
                var client = new WebServiceClient(91678, "CxqJs3TaYcsT");

                var locationdetails = client.City(pIPAddress);

                dr["Organization"] = locationdetails.Traits.Organization;
                dr["Organization"] = locationdetails.Traits.Organization;
                dr["ISP"] = locationdetails.Traits.Isp;
                dr["City"] = locationdetails.City.Name;
                dr["Region"] = locationdetails.MostSpecificSubdivision.Name;
                dr["RegionCode"] = locationdetails.MostSpecificSubdivision.IsoCode;
                dr["Country"] = locationdetails.Country.Name;
                dr["CountryCode"] = locationdetails.Country.IsoCode;
                dr["Zipcode"] = locationdetails.Postal.Code;
                dr["Logintude"] = Convert.ToString(locationdetails.Location.Longitude);
                dr["Latitude"] = Convert.ToString(locationdetails.Location.Latitude);
                dr["TimeZone"] = locationdetails.Location.TimeZone;
                dr["HostName"] = locationdetails.Traits.Domain;
                dr["IPAddress"] = locationdetails.Traits.IPAddress;

                dt.Rows.Add(dr);
                ds.Tables.Add(dt);

            }
            catch (Exception ex)
            {

            }

            return ds;
        }
    }
