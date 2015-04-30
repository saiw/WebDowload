using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleDownload
{
    class Program
    {
        static void RunStringReplace()
        {
            string s1 = "⊕ 0.89";
            string s2 = Regex.Replace(s1, @"[+,-,⊙,⊕]", "").Trim ();
            Console.WriteLine(s2);
        }

        
        static void Main(string[] args)
        {
            Console.WriteLine("press any key to start...");
            Console.ReadKey();

            //WebDownload.DayStockDowload downloader = new WebDownload.DayStockDowload();
            //WebDownload.ExchangeRageByDay downloader = new WebDownload.ExchangeRageByDay();
            WebDownload.YahooNew downloader = new WebDownload.YahooNew();  //parse yahoo website ,save to temp folder

            if (downloader.Excute())
                Console.WriteLine("\nCompleted...");
            else
                Console.WriteLine("\n"+downloader.ErrorMessage);
            
            
            Console.WriteLine("press any key to exit...");
            Console.ReadKey();
        }
    }
}
