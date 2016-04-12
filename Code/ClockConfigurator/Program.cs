using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClockConfigurator
{
    class Program
    {
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
                    path = args[1];
                    break;
                case 3:
                    port = args[0];
                    if (!int.TryParse(args[1], out val))
                    {
                        Console.WriteLine("The value is not numeric.");
                        return res;
                    }
                    path = args[2];
                    break;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("Check file path, there is no such a file!");
                return res;
            }

            config = new Configure(port, path);
            config.Init();
            res = config.RunScript(val);
            return res;
        }
    }
}
