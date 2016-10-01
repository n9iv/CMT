using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace C_SwitchConfigurator
{
    class Log
    {
        private static FileStream _logFile;
        private static StreamWriter _fileWrite;
        private static int _noLogsToSave;
        private static string _fdPath;
        private static bool _logCreationFailed = false;
        private static string fullPath;

        public static int noLogToSave
        {
            set
            {
                _noLogsToSave = value;
            }
        }

        /// <summary>
        /// Creates text file.
        /// </summary>
        /// <param name="confName"></param>
        public static void CreateFile(string confName)
        {
            _fdPath = confName + @"\Logs\";
            string fileName = confName + "_" + GetDateTime() + ".txt";
            fullPath = _fdPath + fileName;
            if (CheckLogFolder(_fdPath) == false)
            {
                _logCreationFailed = true;
                return;
            }
            try
            {
                if (VerifyNoLogFiles() == 0)
                {
                    _logFile = File.Open(fullPath, FileMode.Create, FileAccess.Write);
                    _fileWrite = new StreamWriter(_logFile);
                    Close();
                }
                else
                    _logCreationFailed = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logCreationFailed = true;
            }
        }

        /// <summary>
        /// Write message to log.
        /// In the beginning of the message full time is added
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string msg)
        {
            if (!_logCreationFailed)
            {
                _logFile = File.Open(fullPath, FileMode.Append, FileAccess.Write);
                _fileWrite = new StreamWriter(_logFile);
            }
            string str = string.Format("{0:HH:mm:ss:fff}", DateTime.Now);

            str = string.Format("{0}:\t{1}", str, msg);

            Console.WriteLine(msg);
            if (!_logCreationFailed)
            {
                _fileWrite.WriteLine(str);
                Close();
            }

        }

        public static void Close()
        {
            if (!_logCreationFailed)
            {
                _fileWrite.Close();
                _logFile.Close();
            }
        }

        /// <summary>
        /// Checks the number of existing logs in the folder.
        /// if the number is more than required, erases the older log.
        /// </summary>
        /// <returns></returns>
        private static int VerifyNoLogFiles()
        {
            FileSystemInfo fileInfo;
            FileSystemInfo[] filesInfo = new DirectoryInfo(_fdPath).GetFileSystemInfos();
            int filesNo = filesInfo.Count();
            if (filesNo > 0)
                fileInfo = filesInfo.OrderByDescending(fi => fi.CreationTime).Last();
            else
                return 0;
            try
            {
                if ((fileInfo != null) && (filesNo >= _noLogsToSave))
                    File.Delete(fileInfo.FullName);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

        }

        /// <summary>
        /// Get date and time in correspondig format yyyy_mm_dd_hh_mm_ss_ms
        /// </summary>
        /// <returns></returns>
        private static string GetDateTime()
        {
            string str;
            DateTime time = DateTime.Now;

            // str = string.Format("{0:d/M/yyyy HH:mm:ss}", time);
            str = string.Format("{0:yyyy/M/d HH:mm:ss}", time);
            str = str.Replace("/", "_");
            str = str.Replace(" ", "_");
            str = str.Replace(":", "_");
            return str;
        }

        /// <summary>
        /// Checks if the folder the logs stored in exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool CheckLogFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
