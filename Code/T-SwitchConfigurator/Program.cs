using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace T_SwitchConfigurator
{
    class Program
    {
        static int Main(string[] args)
        {
            Switch sw;
            string path = null, type = null, port = "";
            int MN = 0, BN = 0, res = -1;

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
                    path = args[3];
                    break;
                case 5:
                    port = args[0];
                    type = args[1];
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
                    path = args[4];
                    break;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("Check file path, there is no such a file!");
                return res;
            }

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
