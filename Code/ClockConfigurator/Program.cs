using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ClockConfigurator
{
    /// <summary>
    /// The options to run the program from command line:
    /// 1. val path - val is the battery value to configure, path - scripts folder path.
    /// 2. [COM#] val path - COM# - is the com port name (example: COM4), the rest is the same.
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            Configure config;
            string path = null, port = "";
            int res = (int)Configure.ErrorCodes.Failed, val = 0;
            Configure.ErrorCodes err;
            string msg = GetProcName();

            if ((err = XMLparser.Parse()) != Configure.ErrorCodes.Success)
                return (int)err;

            foreach (string str in args)
                msg += " " + str;
            if (args[0] != "?")
            {
                Log.CreateFile("ClockConfigurator");
                Log.Write(msg + "\n");
            }

            switch (args.Length)
            {
                case 0:
                    Log.Write("No arguments.\nFor help press '?'");
                    return Close(res);
                case 1:
                    if (args[0] == "?")
                    {
                        string str;

                        str = GetHelp();
                        Console.WriteLine(str);
                        return (int)Configure.ErrorCodes.Success;
                    }
                    else
                    {
                        Log.Write("Invalid argument.");
                        return Close(res);
                    }
                case 2:
                    if (!int.TryParse(args[0], out val))
                    {
                        Log.Write("The value is not numeric.");
                        return Close(res);
                    }
                    if (CheckValue(val) == -1)
                        return Close(res);
                    path = args[1];
                    break;
                case 3:
                    port = args[0];
                    if (!int.TryParse(args[1], out val))
                    {
                        Log.Write("The value is not numeric.");
                        return Close(res);
                    }
                    if (CheckValue(val) == -1)
                        return Close(res);
                    path = args[2];
                    break;
            }
            
            if (!Directory.Exists(path))
            {
                Log.Write("Check directory path, there is no such directory!");
                return Close(res);
            }

            config = new Configure(port, path);
            if ((res = config.Init()) != 0)
                return Close(res);
            res = config.RunScript(val);
            Log.Close();
            return res;
        }

        private static int CheckValue(int val)
        {
            if ((val < 0) || (val > 3))
            {
                Log.Write("Incorrect value. The value should be between 0-3");
                return (int)Configure.ErrorCodes.Failed;
            }
            return (int)Configure.ErrorCodes.Success;
        }

        private static string GetHelp()
        {
            string str = null;

            str = "[Help:]\n\n" +
                "ClockConfigurator.exe [COM#] value path\n\n" + 
                "Parameters description:\n\n" +
                "COM# - Serial com port name(e.g. COM3)\n" +
                "value - is the battery value to configure, possible values 0-3\n" +
                "path - full path of folder contains all scripts\n\n" + 
                "Example:\n\n" + 
                @"ClockConfigurator.exe 2 C:\Users\arthurs\Git\CMT\Code\ClockConfigurator\bin\x86\Debug\Scripts" +
                "\n--------------------------------------------------------------------------";
                
            return str;
        }

        private static string GetProcName()
        {
            Process proc = Process.GetCurrentProcess();
            string procName = proc.MainModule.FileName;

            int i = procName.LastIndexOf('\\');
            procName = procName.Remove(0, i + 1);

            return procName;
        }

        private static int Close(int ret)
        {
            Log.Close();
            return ret;
        }
    }
}
