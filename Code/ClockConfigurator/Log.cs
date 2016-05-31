using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClockConfigurator
{
    class Log
    {
        private static FileStream _logFile;
        private static StreamWriter _fileWrite;
        private static int _noLogsToSave;
        private static string _fdPath;
        private static bool _logCreationFailed = false;

        public static int noLogToSave
        {
            set
            {
                _noLogsToSave = value;
            }
        }

        public static void CreateFile(string confName)
        {
            _fdPath = confName + @"\Logs\";
            string fileName = confName + "_" + GetDateTime() + ".txt";
            string fullPath = _fdPath + fileName;
            if(CheckLogFolder(_fdPath) == false)
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

        public static void Write(string msg)
        {
            string str = string.Format("{0:HH:mm:ss:fff}", DateTime.Now);

            str = string.Format("{0}:\t{1}", str, msg);

            Console.WriteLine(msg);
            if (!_logCreationFailed)
                _fileWrite.WriteLine(str);
        }

        public static void Close()
        {
            if (!_logCreationFailed)
            {
                _fileWrite.Close();
                _logFile.Close();
            }
        }

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
