using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RaptorDB;

namespace RaptorServerr
{
    class Program
    {
        static RaptorDBServer server;
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            RaptorDB.Global.EnableWebStudio = true;
            RaptorDB.Global.WebStudioPort = 91;
            RaptorDB.Global.LocalOnlyWebStudio = false;
            server = new RaptorDBServer(90, "C:\\Users\\nikol\\Documents\\GitHub\\BanjiNetV2\\data");

            Console.WriteLine("Server started on port 90");
            Console.WriteLine("Press Enter to exit...");
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Console.ReadLine();
            server.Shutdown();

            return;
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Shutting down...");
            server.Shutdown();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.WriteAllText("error.txt", "" + e.ExceptionObject);
        }
    }
}
