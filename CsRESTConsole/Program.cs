/**
 *  2016-02-24 M.Horigome
 *  WCF REST WebService Sample
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// add
using System.ServiceModel.Web;

// 注意： Admonistrator 権限で実行すること
namespace CsRESTConsole
{
    class Program
    {
        /// <summary>
        /// main entry
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (WebServiceHost serviceHost = new WebServiceHost(typeof(ImagingService)))
            {
                Console.WriteLine("============================================");
                Console.WriteLine(" REST Service sample");
                Console.WriteLine(" [GET] /localhost:8081/api/version");
                Console.WriteLine(" [GET] /localhost:8081/api/image?location=xxx");
                Console.WriteLine("============================================");

                serviceHost.Open();
                Console.WriteLine("Service is running...");

                Console.ReadLine();
                serviceHost.Close();
            }
        }
    }
}
