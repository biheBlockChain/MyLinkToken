using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MyLinkToken
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //启动自动更新功能
            //Updater.CheckUpdateSimple(@"\\192.168.105.12\publish$\VolControl\{0}", "update_c.xml");

            //各类数据库连接字符串
            //sql_sql验证：@"server = IP,PORT\NAME;database = XXX;uid = XX;pwd = XXX;Connection Timeout = 3";
            //sql_windows验证：@"server=localhost;database=XXX;Integrated Security=SSPI；Connection Timeout = 3";
            //localdb：@"Data Source=(LocalDB)\v11.0;Initial Catalog=mdf_pathORdbName;Integrated Security=True;Connection Timeout = 3";
            //sqlite："Data Source = data_path";
            //OLEDB4.0："Provider=Microsoft.Jet.OLEDB.4.0;Data Source=data_path";
            //OLEDB12.0："Provider=Microsoft.ACE.OLEDB.12.0;Data Source=data_path";
            //Oracle："user id=xxx;password=xxx;data source=192.168.1.1:1521/ORCL"

            //NLog使用示例
            //try
            //{
            //    int a = 0;
            //    int b = 1;
            //    var c = b / a;
            //}
            //catch (Exception err)
            //{
            //    Helpers.NLogHelper.Default.Error("Error", err);
            //}

#if DEBUG//开发调试不进行异常处理
#else
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            //全局异常捕捉
            Application.ThreadException += Application_ThreadException; //UI线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; //多线程异常
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
            //单实例模式启动
            RunOneApp(new FormMain(), true);
        }

        //UI线程异常
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WinFormEx.FormBugReport.ShowBug(e.Exception);
        }
        //多线程异常
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WinFormEx.FormBugReport.ShowBug((Exception)e.ExceptionObject);
        }

#region 只运行启动一个程序实例
        /// <summary>
        /// 防止程序重复运行
        /// </summary>
        /// <param name="startForm">new form()</param>
        /// <param name="activate">是否激活已运行程序</param>
        public static void RunOneApp(Form startForm, bool activate)
        {
            if (activate)
            {
                Process instance = RunningInstance();
                if (instance == null)
                {
                    //没有实例在运行
                    Application.Run(startForm);
                }
                else
                {
                    //已经有一个实例在运行
                    HandleRunningInstance(instance);
                }
            }
            else
            {
                bool initiallyOwned = true;
                bool isCreated;
                Mutex m = new Mutex(initiallyOwned, Application.ProductName, out isCreated);
                if (!(initiallyOwned && isCreated))
                {
                    MessageBox.Show("程序已在运行,请勿重复打开！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else
                {
                    Application.Run(startForm);
                }
            }
        }
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表  
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程  
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;
                    }
                }
            }
            return null;
        }
        private static void HandleRunningInstance(Process instance)
        {
            //MessageBox.Show("已经在运行！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowWindowAsync(instance.MainWindowHandle, 9);  //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
#endregion
    }
}
