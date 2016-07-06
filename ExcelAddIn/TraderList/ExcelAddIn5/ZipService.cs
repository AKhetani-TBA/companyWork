using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelAddIn5
{
    public class ZipService
    {

        ZipFile zipFileForCompress;
        /// <summary>
        /// Adding the file to zipServicetype object
        /// </summary>
        /// <param name="fileName"></param>
        public void AddFile(List<string> fileName)
        {
            try
            {
                zipFileForCompress = new ZipFile();
                zipFileForCompress.AddFiles(fileName);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ZipService.cs", "AddFile()", ex.Message, ex);
            }
        }
        /// <summary>
        /// Creating the zip file at destination Folder
        /// </summary>
        /// <param name="destinationPath">Destination folder path</param>
        public void Save(string destinationPath)
        {
            try
            {
                zipFileForCompress.Save(destinationPath);
            }
            catch (Exception ex)
            {
                LogUtility.Error("ZipService.cs", "Save()", ex.Message, ex);
            }
        }

        /// <summary>
        /// Deleting the ZipFile
        /// </summary>
        /// <param name="destinationPath">destination Path</param>
        public void DeleteFile(string destinationPath)
        {
            try
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error("ZipService.cs", "DeleteFile()", ex.Message, ex);
            }
        }
    }
}
