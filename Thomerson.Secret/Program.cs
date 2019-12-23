using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Thomerson.Secret
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Info("StartApp");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.Run(new FMMain());
            glExitApp = true;//标志应用程序可以退出
        }

        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        static bool glExitApp = false;


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error($"CurrentDomain_UnhandledException:{JsonConvert.SerializeObject(e.ExceptionObject)}");

            while (true)
            {//循环处理，否则应用程序将会退出
                if (glExitApp)
                {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                    Log.Info("ExitApp");
                    return;
                }
                System.Threading.Thread.Sleep(2 * 1000);
            };
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Error($"Application_ThreadException:{JsonConvert.SerializeObject(e.Exception)}");
        }
    }
}
