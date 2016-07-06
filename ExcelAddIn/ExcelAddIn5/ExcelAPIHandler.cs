using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNet;
using System.Net.Http;
using System.Net.Http.Headers;
using TBA.BeastModels.User;

using System.Net.Http.Formatting;
using TBA.BeastModels.Excel;
//using System.Net.Http;


namespace ExcelAddIn5
{
    public class ExcelAPIHandler
    {
        private string APIURL = ConfigurationManager.AppSettings["APIURL"].ToString();


        /// <summary>
        /// User Authentication Method 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public ClientInfo ValidateUser(string username, string password)
        {
            var endpoint = "api/User/SignIn/" + "?";
            var parameters = new List<string>();
            HttpClient client = new HttpClient();

            ////Adding Parameters
            parameters.Add(string.Concat("username=", username));
            parameters.Add(string.Concat("password=", password));

            ////api endpoint with parameters
            endpoint += string.Join("&", parameters);
            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(endpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<ClientInfo>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("Login call to Web api Failed");
            }
        }


        /// <summary>
        /// GetLatestVersionID
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="excelObjectID"></param>
        /// <returns></returns>
        public ExcelObject GetLatestVersionID(string userName, string excelObjectID)
        {
            var endpoint = "api/Excel/GetLatestVersionID/" + "?";
            var parameters = new List<string>();
            HttpClient client = new HttpClient();

            ////Adding Parameters
            parameters.Add(string.Concat("userName=", userName));
            parameters.Add(string.Concat("excelObjectId=", excelObjectID));

            ////api endpoint with parameters
            endpoint += string.Join("&", parameters);


            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(endpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<ExcelObject>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("GetLatestVersionID call to Web api Failed");
            }
        }

        /// <summary>
        /// GetObjectVersionMappings
        /// </summary>
        /// <param name="excelObjectId"></param>
        /// <param name="excelVersion"></param>
        /// <returns></returns>
        public List<ExcelVersionUpdateMap> GetObjectVersionMappings(string excelObjectId, int excelVersion)
        {
            HttpClient client = new HttpClient();

            var endpoint = "api/Excel/GetObjectVersionMappings/" + "?";
            var parameters = new List<string>();

            ////Adding Parameters
            parameters.Add(string.Concat("excelObjectId=", excelObjectId));
            parameters.Add(string.Concat("excelVersion=", excelVersion));

            ////api endpoint with parameters
            endpoint += string.Join("&", parameters);

            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(endpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<ExcelVersionUpdateMap>>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("GetObjectVersionMappings call to Web api Failed");
            }
        }

        /// <summary>
        /// GetSetupOfLatestVersion
        /// </summary>
        /// <param name="excelObjectId"></param>
        /// <returns></returns>
        public List<ExcelObject> GetSetupOfLatestVersion(string excelObjectId)
        {
            var endpoint = "api/Excel/GetSetupOfLatestVersion/" + "?";
            var parameters = new List<string>();

            HttpClient client = new HttpClient();
            ////Adding Parameters
            parameters.Add(string.Concat("excelObjectId=", excelObjectId));

            ////api endpoint with parameters
            endpoint += string.Join("&", parameters);

            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(endpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<ExcelObject>>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("GetSetupOfLatestVersion call to Web api Failed");
            }
        }

        public string ForgotPassword(string userEmail)
        {

            //Random randomNumber = new Random();
            //string newPassword = Convert.ToString(randomNumber.Next(100000, 999999));

            var endpoint = "api/User/ForgotPassword/" + "?";
            var parameters = new List<string>();

            HttpClient client = new HttpClient();
            ////Adding Parameters

            ////api endpoint with parameters

            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync(endpoint, userEmail).Result;


            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("ForgotPassword call to Web api Failed");
            }

        }

        public int ChangePassword(string userEmail, string oldPassword, string newPassword)
        {
            var endpoint = "api/User/ChangePassword/" + "?";

            HttpClient client = new HttpClient();
            //Adding Parameters
            TBA.BeastModels.Parameters.ChangePassword changePassword = new TBA.BeastModels.Parameters.ChangePassword();
            changePassword.EmailId = userEmail;
            changePassword.OldPassword = oldPassword;
            changePassword.NewPassword = newPassword;

            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthenticationManager.Instance.UserToken);

            var response = client.PostAsJsonAsync(endpoint, changePassword).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<int>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("ForgotPassword call to Web api Failed");
            }

        }


        public string GetUserRole(string userEmail)
        {

            var endpoint = "api/User/UserRole/" + "?";
            var parameters = new List<string>();
            HttpClient client = new HttpClient();

            ////Adding Parameters
            parameters.Add(string.Concat("userEmail=", userEmail));

            ////api endpoint with parameters
            endpoint += string.Join("&", parameters);
            client.BaseAddress = new Uri(APIURL);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync(endpoint).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<string>().Result;
            }
            else
            {
                // to be implent for throwing exception
                throw new Exception("Login call to Web api Failed");
            }
        }

    }
}
