using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ActionLogger
{
    public class Logger
    {
        public static void WriteLog(Exception ex, bool IsServer)
        {

            //File.Create("E:\\"+DateTime.Today.Year);
            //File.OpenWrite("E:\\" + DateTime.Today.Year);
            //File.Create("E:\\ErrorLog\\" + String.Format("{0:d_M_yyyy_HH_mm_ss}"+".txt", DateTime.Today)).Close();
            try
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\Server_\\" : "\\Client_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy}" + ".txt", DateTime.Today), true))
                {
                    strw.WriteLine("------InnerException----------");
                    try
                    {
                        strw.WriteLine(ex.InnerException.Message);
                    }
                    catch
                    {
                    }
                    strw.WriteLine("------StackTrace----------");

                    strw.WriteLine(ex.StackTrace);
                    strw.WriteLine("------Message----------");

                    strw.WriteLine(ex.Message);
                    strw.Close();
                }

            }
            catch
            {

            }

        }

        public static void WriteLog(string msg, bool IsServer)
        {
            try
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\Server_\\" : "\\Client_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy}" + ".txt", DateTime.Today), true))
                {
                    strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                    strw.Close();
                }
            }
            catch
            {

            }

        }

        public static void WriteImportLog(string msg, bool IsServer)
        {
            try
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerImportLog_\\" : "\\ClientImportLog_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy}" + ".txt", DateTime.Today), true))
                {
                    strw.WriteLine(msg);
                    strw.Close();
                }
            }
            catch
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerCrarsh_\\" : "\\ClientCrarsh_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy}" + ".txt", DateTime.Today), true))
                {
                    strw.WriteLine(msg);
                    strw.Close();
                }
            }

        }

        /// <summary>
        /// New method to save import policy logs at different location
        /// to reduce original log file size and ease of use.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="IsServer"></param>
        public static void WriteImportPolicyLog(string msg, bool IsServer)
        {
            string sizeLimitInBytes = System.Configuration.ConfigurationSettings.AppSettings["SizeLimitInBytes"];
            string path = string.Empty;
            long length = 0;
            string file = string.Empty;
            string fileName = string.Empty;
            string dateFolderLocation = string.Empty;

            long lSizeLimitInBytes = Convert.ToInt64(sizeLimitInBytes);

            try
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerImportPolicyLog_\\" : "\\ClientImportLog_\\");

                Directory.CreateDirectory(folderLocation);

                dateFolderLocation += "\\ErrorLog" + (IsServer ? "\\ServerImportPolicyLog_\\" : "\\ClientImportLog_\\") + String.Format("{0:d_M_yyyy}", DateTime.Now) + "\\";

                if (!Directory.Exists(dateFolderLocation))
                {
                    Directory.CreateDirectory(dateFolderLocation);
                }

                //string[] fileEntries = Directory.GetFiles(folderLocation);
                string[] fileEntries = Directory.GetFiles(dateFolderLocation);

                if (fileEntries.Length == 0)
                {
                    //path = folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now);

                    path = Path.Combine(dateFolderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                    using (StreamWriter strw = new StreamWriter(path, true))
                    {
                        strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                        strw.Close();
                    }
                }
                else
                {
                    //DirectoryInfo dir = new DirectoryInfo(folderLocation);

                    DirectoryInfo dirInfo = new DirectoryInfo(dateFolderLocation);

                    FileInfo[] files = dirInfo.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                    length = files[0].Length;
                    fileName = files[0].Name;

                    if (length < lSizeLimitInBytes)
                    {
                        //path = Path.Combine(folderLocation + fileName);

                        path = Path.Combine(dateFolderLocation + fileName);

                        using (StreamWriter strw = new StreamWriter(path, true))
                        {
                            strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                            strw.Close();
                        }
                    }
                    else
                    {
                        //path = Path.Combine(folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                        path = Path.Combine(dateFolderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                        using (StreamWriter strw = new StreamWriter(path, true))
                        {
                            strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                            strw.Close();
                        }
                    }
                }
            }
            catch
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerCrarsh_\\" : "\\ClientCrarsh_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now), true))
                {
                    strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                    strw.Close();
                }
            }

        }

        /// <summary>
        /// New method to save import policy logs at different location
        /// to reduce original log file size and ease of use.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="IsServer"></param>
        /// <param name="agencyName"></param>
        public static void WriteImportPolicyLog(string msg, bool IsServer, string agencyName)
        {
            if (string.IsNullOrEmpty(agencyName))
            {
                WriteImportPolicyLog(msg, IsServer);
                return;
            }

            string sizeLimitInBytes = System.Configuration.ConfigurationSettings.AppSettings["SizeLimitInBytes"];
            string path = string.Empty;
            long length = 0;
            string file = string.Empty;
            string fileName = string.Empty;
            string dateFolderLocation = string.Empty;

            long lSizeLimitInBytes = Convert.ToInt64(sizeLimitInBytes);

            try
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerImportPolicyLog_\\" : "\\ClientImportLog_\\");

                Directory.CreateDirectory(folderLocation);

                dateFolderLocation += "\\ErrorLog" + (IsServer ? "\\ServerImportPolicyLog_\\" : "\\ClientImportLog_\\") + agencyName + "\\" + String.Format("{0:d_M_yyyy}", DateTime.Now) + "\\";

                if (!Directory.Exists(dateFolderLocation))
                {
                    Directory.CreateDirectory(dateFolderLocation);
                }

                //string[] fileEntries = Directory.GetFiles(folderLocation);
                string[] fileEntries = Directory.GetFiles(dateFolderLocation);

                if (fileEntries.Length == 0)
                {
                    //path = folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now);

                    path = Path.Combine(dateFolderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                    using (StreamWriter strw = new StreamWriter(path, true))
                    {
                        strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                        strw.Close();
                    }
                }
                else
                {
                    //DirectoryInfo dir = new DirectoryInfo(folderLocation);

                    DirectoryInfo dirInfo = new DirectoryInfo(dateFolderLocation);

                    FileInfo[] files = dirInfo.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                    length = files[0].Length;
                    fileName = files[0].Name;

                    if (length < lSizeLimitInBytes)
                    {
                        //path = Path.Combine(folderLocation + fileName);

                        path = Path.Combine(dateFolderLocation + fileName);

                        using (StreamWriter strw = new StreamWriter(path, true))
                        {
                            strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                            strw.Close();
                        }
                    }
                    else
                    {
                        //path = Path.Combine(folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                        path = Path.Combine(dateFolderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now));

                        using (StreamWriter strw = new StreamWriter(path, true))
                        {
                            strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                            strw.Close();
                        }
                    }
                }
            }
            catch
            {
                Environment.SpecialFolder special = Environment.SpecialFolder.MyDocuments;
                string folderLocation = Environment.GetFolderPath(special);
                folderLocation += "\\ErrorLog" + (IsServer ? "\\ServerCrarsh_\\" : "\\ClientCrarsh_\\");
                Directory.CreateDirectory(folderLocation);
                using (StreamWriter strw = new StreamWriter(folderLocation + String.Format("{0:d_M_yyyy_hh_mm_ss}" + ".txt", DateTime.Now), true))
                {
                    strw.WriteLine(DateTime.Now.ToString() + ": " + msg);
                    strw.Close();
                }
            }

        }
    }
}
