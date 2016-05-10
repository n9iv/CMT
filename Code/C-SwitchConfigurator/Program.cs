using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
{
    /// <summary>
    /// The options to run the program from command line:
    /// 1. [val] path - val is the value to be replaced in script, path - full script path destination.
    /// 2. [COM#] [val] path - COM# - is the com port name (example: COM4), the rest is the same.
    /// </summary>
    class Program
    {
        private static int CheckValue(int val)
        {
            if ((val < 0) || (val > 3))
            {
                Console.WriteLine("Incorrect value. The value should be between 0-3");
                return -1;
            }
            return 0;
        }

        static int Main(string[] args)
        {
            Configure config;
            string path = null, port = "";
            int res = -1, val = 0;

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("No arguments.");
                    return res;
                case 1:
                    Console.WriteLine("Argument is missing.");
                    return res;

                case 2:
                    if (!int.TryParse(args[0], out val))
                    {
                        Console.WriteLine("The value is not numeric.");
                        return res;
                    }
                    if (CheckValue(val) == -1)
                        return res;
                    path = args[1];
                    break;
                case 3:
                    port = args[0];
                    if (!int.TryParse(args[1], out val))
                    {
                        Console.WriteLine("The value is not numeric.");
                        return res;
                    }
                    if (CheckValue(val) == -1)
                        return res;
                    path = args[2];
                    break;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("Check file path, there is no such a file!");
                return res;
            }

            config = new Configure(port, path);
            if (config.Init() == -1)
                return -1;
            res = config.RunScript(val);
            return res;
        }
    }
}
