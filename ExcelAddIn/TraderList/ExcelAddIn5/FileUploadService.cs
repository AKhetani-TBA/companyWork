using System;
using System.IO;
using System.Windows.Forms;

namespace ExcelAddIn5
{
    public class FileUploadService
    {
        /// <summary>
        /// Uploading the File to server
        /// </summary>
        /// <param name="url">Server url where file to be uploaded</param>
        /// <param name="fileName">Name of Zipfile</param>
        /// <param name="userName">Sender Name to Identify user</param>
        public void UploadFile(string url, string fileName, string userName)
        {
            System.Net.HttpWebRequest req = null;
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                req.ContentType = "multipart/form-data boundary=" + boundary;
                req.Method = "POST";
                string template = "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                string content = string.Format(template, "from", userName);
                byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);

                Stream rs = req.GetRequestStream();
                rs.Write(contentBytes, 0, contentBytes.Length);
                rs.Write(boundaryBytes, 0, boundaryBytes.Length);
                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\" filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";

                FileStream oFileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

                if (oFileStream.Length > 0)
                {
                    string header = string.Format(headerTemplate, "file0", fileName);

                    byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);

                    rs.Write(headerBytes, 0, headerBytes.Length);

                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = oFileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                    rs.Write(boundaryBytes, 0, boundaryBytes.Length);
                }

                oFileStream.Close();

                rs.Close();

            }
            catch (Exception ex)
            {
                LogUtility.Error("FileUploadService()", "UploadFile", ex.Message, ex);
            }

            System.Net.WebResponse wresp = null;
            try
            {
                wresp = req.GetResponse();
                Stream str = wresp.GetResponseStream();
                StreamReader strReader = new StreamReader(str);
                LogUtility.Debug("FileUploadService.cs", "UploadFile", string.Format("File uploaded, server response is: {0}", strReader.ReadToEnd()));
                MessageBox.Show("File sent successfully.");
            }
            catch (Exception ex)
            {
                LogUtility.Error("FileUploadService.cs", "UploadFile", "Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                MessageBox.Show("Error in Sending the File");
            }
            finally { req = null; }

        }
    }
}
