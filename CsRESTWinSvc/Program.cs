/**
 * Service sample
 * 2015-02-24 M.Horigome
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

// Add ...
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;

namespace CsRESTWinSvc
{
    // Application main
    static class Program
    {
        /// <summary>
        /// アプリケーションのメインエントリ ポイント
        /// </summary>
        static void Main(string[] args)
        {
            // Install Option
            if (1 <= args.Length)
            {
                if (TryInstall(args[0]))
                {
                    return;
                }
            }

            // Service Create & run
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() {
                    CanShutdown = true,
                    CanPauseAndContinue = false,
                }
            };
            ServiceBase.Run(ServicesToRun);
        }

        /// <summary>
        // インストール、アンインストールの自己実行
        /// <summary>
        public static bool TryInstall(string arg)
        {
            string mode = arg.ToLower();    // install mode (/i or /u)

            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            string sv_name = ConfigurationManager.AppSettings["ServiceName"];

            // /i: Install Service
            if (mode == "/i")
            {
                if (IsServiceExists(sv_name))
                {
                    Console.WriteLine("[ERR] {0} has been already exists.", sv_name);
                    return false;
                }
                else
                {
                    string[] param = { path };
                    ManagedInstallerClass.InstallHelper(param);
                    Console.WriteLine("[OK] {0} has been installed.", sv_name);

                    StartService();
                }
            }
            // /u : Uninstall service
            else if (mode == "/u")
            {
                if (!IsServiceExists(sv_name))
                {
                    Console.WriteLine("[ERR] {0} is not installed.", sv_name);
                    return false;
                }
                else
                {
                    StopService();

                    string[] param = { "/u", path };
                    ManagedInstallerClass.InstallHelper(param);
                    Console.WriteLine("[OK] {0} has been Uninstalled.", sv_name);
                }
            }
            else
            {
                Console.WriteLine("[ERR] Unknoen install option. try /i or /u");
                return false;
            }

            return true;
        }

        /// <summary>
        // サービスがインストールされているかチェック
        /// <summary>
        public static bool IsServiceExists(string service_name)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Any(s => s.ServiceName == service_name);
        }

        /// <summary>
        /// サービスを開始　
        /// </summary>
        public static bool StartService()
        {
            string sv_name = ConfigurationManager.AppSettings["ServiceName"];

            ServiceController sc = new ServiceController(sv_name);
            if (sc.Status == ServiceControllerStatus.Running)
            {
                Console.WriteLine("[WAR] {0} has been already started.", sv_name);
            }
            else
            {
                try
                {
                    sc.Start();
                    Console.WriteLine("[OK] {0} has been started.", sv_name);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERR] {0} has been not started. {1}", sv_name, e.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// サービスを停止
        /// </summary>
        public static bool StopService()
        {
            string sv_name = ConfigurationManager.AppSettings["ServiceName"];

            ServiceController sc = new ServiceController(sv_name);
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                Console.WriteLine("[WAR] {0} has been already stopped.", sv_name);
            }
            else
            {
                try
                {
                    sc.Stop();
                    Console.WriteLine("[OK] {0} has been stoped.", sv_name);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERR] {0} has been not started. {1}", sv_name, e.Message);
                    return false;
                }
            }
            return true;
        }



    }


    /// <summary>
    /// サービスのインストールに必要なクラス
    /// </summary>
    [RunInstaller(true)]
    public class CsRESRWinSvcInstaller : Installer
    {
        public CsRESRWinSvcInstaller()
        {
            var spi = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem

                // use other account ---
                //Username = Environment.UserName,
                //Account = ServiceAccount.LocalService
                // ---
            };

            var si = new ServiceInstaller()
            {
                ServiceName = ConfigurationManager.AppSettings["ServiceName"],
                DisplayName = ConfigurationManager.AppSettings["DisplayName"],
                Description = ConfigurationManager.AppSettings["Description"],

                // Auto Startup
                StartType = ServiceStartMode.Automatic
            };

            this.Installers.Add(spi);
            this.Installers.Add(si);
        }
    }

}
