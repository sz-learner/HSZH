using HSZH.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //SelfLog.Enable(Console.Out);

            //var sw = System.Diagnostics.Stopwatch.StartNew();

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.File("log.txt")
            //    .CreateLogger();


            LogHelper.Instance.WriteInfo("Hello, file logger!");
          

            //Log.CloseAndFlush();

            //sw.Stop();

            //Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
            //Console.WriteLine($"Size: {new FileInfo("log.txt").Length}");

            Console.WriteLine("Press any key to delete the temporary log file...");
            Console.ReadKey(true);

            File.Delete("log.txt");
        }
    }
}
