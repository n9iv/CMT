using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T_SwitchConfigurator
{
    /// <summary>
    /// The options to run the program from command line:
    /// 1. r r/s [MN] [BN] path - r - to run reset script if need, r/s is the type (r - router, s - switch), path - is full script path destination.
    /// 2. [COM#] r r/s [MN] [BN] path - COM# - is the com name (example: COM4), the rest is the same.
    /// </summary>
    class Program
    {
        private static int CheckMNValue(int MN)
        {
            if ((MN < 0) || ((MN > 10) && (MN < 101)) || (MN > 109))
            {
                Console.WriteLine("Incorrect value. The MN values should be between 1-9 or 101-109");
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
                Console.WriteLine("Check script files.");
                return -1;
            }

            return 0;

        }

        private static int CheckBNValue(int BN)
        {
            if ((BN < 0) || (BN > 3))
            {
                Console.WriteLine("Incorrect value. The BN values should be between 1-3");
                return -1;
            }
            return 0;
        }

        static int Main(string[] args)
        {
            Switch sw;
            string path = null, type = null, port = "";
            int MN = 0, BN = 0, res = -1, rstFlag = 0;

            //Check correctness of inserted arguments
            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("No arguments.");
                    return res;
                case 1:
                case 2:
                case 3:
                    Console.WriteLine("Argument is missing.");
                    return res;
                case 4:
                    type = args[0];
                    if (!int.TryParse(args[1], out MN))
                    {
                        Console.WriteLine("The MN value is not numeric");
                        return res;
                    }
                    if (!int.TryParse(args[2], out BN))
                    {
                        Console.WriteLine("The BN value is not numeric");
                        return res;
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return res;
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return res;
                    path = args[3];
                    break;
                case 5:
                    type = args[0];
                    if (args[1] == "r")
                        rstFlag = 1;
                    if (!int.TryParse(args[2], out MN))
                    {
                        Console.WriteLine("The MN value is not numeric");
                        return res;
                    }
                    if (!int.TryParse(args[3], out BN))
                    {
                        Console.WriteLine("The BN value is not numeric");
                        return res;
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return res;
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return res;
                    path = args[4];
                    break;
                case 6:
                    port = args[0];
                    type = args[1];
                    if (args[2] == "r")
                        rstFlag = 1;
                    if (!int.TryParse(args[3], out MN))
                    {
                        Console.WriteLine("The MN value is not numeric");
                        return res;
                    }
                    if (!int.TryParse(args[4], out BN))
                    {
                        Console.WriteLine("The BN value is not numeric");
                        return res;
                    }
                    if (MN != 0)
                        if (CheckMNValue(MN) == -1)
                            return res;
                    if (BN != 0)
                        if (CheckBNValue(BN) == -1)
                            return res;
                    path = args[5];
                    break;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Check folder path, there is no such a folder!");
                return res;
            }

            if (CheckFiles(path) == -1)
            {
                return -1;
            }

            if (XMLparser.Parse(type) == -1)
                return -1;
            sw = new Switch(port, MN, BN, path);
            switch (type)
            {
                case "s":
                    res = sw.SwitchConfig();
                    break;
                case "r":
                    res = sw.RouterConfig();
                    break;
            }
            return res;
        }
    }
}
