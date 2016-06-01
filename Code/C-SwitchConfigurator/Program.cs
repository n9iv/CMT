using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
{
    /// <summary>
    /// The options to run the program from command line:
    /// 1. val path - val is the value to be replaced in script, path - full script path destination.
    /// 2. [COM#] val path - COM# - is the com port name (example: COM4), the rest is the same.
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            Configure config;
            string path = null, port = "", msg = GetProcName();
            bool isMain = false, reset = false;
            int res = (int)ErrorCodes.Failed, val = 0;
            ErrorCodes err;
            
            if ((err = XMLparser.Parse()) != ErrorCodes.Success)
                return (int)err;

            foreach (string str in args)
                msg += " " + str;
            if (args[0] != "?")
            {
                Log.CreateFile("C-SwitchConfigurator");
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
                        return (int)ErrorCodes.Success;
                    }
                    else
                    {
                        Log.Write("Invalid arguments.");
                        return Close(res);
                    }
                case 2:
                    Log.Write("Invalid arguments.");
                    return Close(res);
                case 3:
                    if (args[0] == "r")
                        isMain = false;
                    else
                        if (args[0] == "m")
                            isMain = true;
                        else
                        {
                            Log.Write("One of the arguments is invalid");
                            return Close(res);
                        }
                    if (!int.TryParse(args[1], out val))
                    {
                        Log.Write("The value is not numeric.");
                        return Close(res);
                    }
                    if (CheckValue(val) == -1)
                        return Close(res);
                    path = args[2];
                    break;
                case 4:
                    if (args[0] == "reset")
                        reset = true;
                    else
                    {
                        Log.Write("One of the arguments is invalid");
                        return Close(res);
                    }
                    if (args[1] == "r")
                        isMain = false;
                    else
                        if (args[1] == "m")
                            isMain = true;
                        else
                        {
                            Log.Write("One of the arguments is invalid");
                            return Close(res);
                        }
                    if (!int.TryParse(args[2], out val))
                    {
                        Log.Write("The value is not numeric.");
                        return Close(res);
                    }
                    if (CheckValue(val) == -1)
                        return Close(res);
                    path = args[3];
                    break;
                case 5:
                    port = args[0];
                    if (args[1] == "reset")
                        reset = true;
                    else
                    {
                        Log.Write("One of the arguments is invalid");
                        return Close(res);
                    }
                    if (args[2] == "r")
                        isMain = false;
                    else
                        if (args[2] == "m")
                            isMain = true;
                        else
                        {
                            Log.Write("One of the arguments is invalid");
                            return Close(res);
                        }
                    if (!int.TryParse(args[3], out val))
                    {
                        Log.Write("The value is not numeric.");
                        return Close(res);
                    }
                    if (CheckValue(val) == -1)
                        return Close(res);
                    path = args[4];
                    break;
            }

            if (!Directory.Exists(path))
            {
                Log.Write("Check directory path, there is no such directory!");
                return Close(res);
            }

            config = new Configure(port, path,isMain);
            if ((res = config.Init()) != (int)ErrorCodes.Success)
                return Close(res);
            if (reset)
                res = config.ResetRouter();
            else
                res = config.RunScript(val);
            Log.Close();
            return res;
        }

        private static int CheckValue(int val)
        {
            if ((val < 1) || (val > 3))
            {
                Log.Write("Incorrect value. The value should be between 0-3");
                return (int)ErrorCodes.Failed;
            }
            return (int)ErrorCodes.Success;
        }

        private static string GetHelp()
        {
            string str = null;

            str = "[Help:]\n\n" +
                "C-SwitchConfigurator.exe [COM#] [reset] r/m BN path\n\n" +
                "Parameters description:\n\n" +
                "COM# - Serial com port name(e.g. COM3)\n" +
                "reset - reset switch\n" +
                "r/m - 'r' for redundancy and 'm' for main\n" +
                "BN - is the BN value to configure, possible values 1-3\n" +
                "path - full path of folder contains all scripts\n\n" +
                "Example:\n\n" +
                @"C-SwitchConfigurator.exe r 2 C:\Users\arthurs\Git\CMT\Code\ClockConfigurator\bin\x86\Debug\Scripts" +
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
