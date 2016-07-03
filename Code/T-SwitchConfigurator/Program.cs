using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace T_SwitchConfigurator
{
    /// <summary>
    /// The options to run the program from command line:
    /// 1. r r/s [MN] [BN] path - r - to run reset script if need, r/s is the type (r - router, s - switch), path - is full script path destination.
    /// 2. [COM#] r r/s [MN] [BN] path - COM# - is the com name (example: COM4), the rest is the same.
    /// </summary>
    class Program
    {
        static int Main(string[] args)
        {
            Switch sw;
            string path = null, type = null, port = "", msg = GetProcName();
            int MN = 0, BN = 0, res = (int)ErrorCodes.Failed;
            bool reset = false;

            foreach (string str in args)
                msg += " " + str;
            if (args[0] != "?")
            {
                Log.CreateFile("T-SwitchConfigurator");
                Log.Write(msg + "\n");
            }

            if ((res = XMLparser.Parse(type)) != (int)ErrorCodes.Success)
            {
                return Close(res);
            }

            //Check correctness of inserted arguments
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
                case 3:
                    Log.Write("Argument is missing.");
                    return Close(res);
                case 4:
                    type = args[0];
                    if (!int.TryParse(args[1], out MN))
                    {
                        Log.Write("The MN value is not numeric");
                        return Close(res);
                    }
                    if (!int.TryParse(args[2], out BN))
                    {
                        Log.Write("The BN value is not numeric");
                        return Close(res);
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return Close(res);
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return Close(res);
                    path = args[3];
                    break;
                case 5:

                    if (args[0] == "reset")
                        reset = true;
                    else
                    {
                        Log.Write("One of the arguments is invalid");
                        return Close(res);
                    }
                    type = args[1];
                    if (!int.TryParse(args[2], out MN))
                    {
                        Log.Write("The MN value is not numeric");
                        return Close(res);
                    }
                    if (!int.TryParse(args[3], out BN))
                    {
                        Log.Write("The BN value is not numeric");
                        return Close(res);
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return Close(res);
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return Close(res);
                    path = args[4];
                    break;
                case 6:
                    port = args[0];
                    if (args[1] == "reset")
                        reset = true;
                    else
                    {
                        Log.Write("One of the arguments is invalid");
                        return Close(res);
                    }
                    type = args[2];
                    if (!int.TryParse(args[3], out MN))
                    {
                        Log.Write("The MN value is not numeric");
                        return Close(res);
                    }
                    if (!int.TryParse(args[4], out BN))
                    {
                        Log.Write("The BN value is not numeric");
                        return Close(res);
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return Close(res);
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return Close(res);
                    path = args[5];
                    break;
            }

            if (!Directory.Exists(path))
            {
                Log.Write("Check folder path, there is no such a folder!");
                return Close(res);
            }

            if (CheckFiles(path) != (int)ErrorCodes.Success)
            {
                return Close((int)ErrorCodes.Failed);
            }

            XMLparser.ParseInfo(type);
            sw = new Switch(port, MN, BN, path);
            switch (type)
            {
                case "s":
                    res = sw.SwitchConfig(reset);
                    break;
                case "r":
                    res = sw.RouterConfig(reset);
                    break;
            }

            Log.Close();
            return res;
        }

        private static int CheckMNValue(int MN)
        {
            if ((MN < 0) || ((MN > 10) && (MN < 101)) || (MN > 109))
            {
                Log.Write("Incorrect value. The MN values should be between 1-9 or 101-109");
                return -1;
            }
            return 0;
        }

        private static int CheckFiles(string path)
        {
            string routerBN = path + "\\" + Switch.routeBN;
            string routerMN = path + "\\" + Switch.routeMN;
            string switchMN = path + "\\" + Switch.switchMN;
            string switchBN = path + "\\" + Switch.switchBN;

            if (!File.Exists(routerBN) || !File.Exists(routerMN) || !File.Exists(switchMN) || !File.Exists(switchBN))
            {
                Log.Write("Check script files.");
                return (int)ErrorCodes.Failed;
            }

            return (int)ErrorCodes.Success;

        }

        private static int CheckBNValue(int BN)
        {
            if ((BN < 0) || (BN > 3))
            {
                Log.Write("Incorrect value. The BN values should be between 1-3");
                return (int)ErrorCodes.Failed;
            }
            return (int)ErrorCodes.Success;
        }

        private static string GetHelp()
        {
            string str = null;

            str = "[Help:]\n\n" +
                "T-SwitchConfigurator.exe [COM#] [reset] r/s MN BN path\n\n" +
                "Parameters description:\n\n" +
                "COM# - Serial com port name(e.g. COM3)\n" +
                "reset - reset switch\n" +
                "r/s - 'r' for router and 's' for switch\n" +
                "MN - is the MN value to configure, possible values 1-9 or 101-109\n" +
                "BN - is the BN value to configure, possible values 1-3\n" +
                "path - full path of folder contains all scripts\n\n" +
                "Note: one of the MN or BN parameters can be 0. In that case, the configurator will ignore that parameter\n\n" +
                "Example:\n\n" +
                @"T-SwitchConfigurator.exe r 2 0 C:\Users\arthurs\Git\CMT\Code\ClockConfigurator\bin\x86\Debug\Scripts" +
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
