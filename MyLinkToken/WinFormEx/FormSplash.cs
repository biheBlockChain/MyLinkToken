using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MyLinkToken.WinFormEx
{
    public partial class FormSplash : Form
    {
        public FormSplash()
        {
            InitializeComponent();
        }

        private void FormSplash_TextChanged(object sender, EventArgs e)
        {
            lbProgress.Text = this.Text;
        }
        private void FormSplash_Load(object sender, EventArgs e)
        {
            var t = new System.Windows.Forms.Timer();
            int a = 0;
            t.Interval = 1;
            t.Tick += (s, x) =>
            {
                lbTick.Text = a++.ToString();
            };
            t.Start();
        }
    }

    /// <summary>
    /// 加载窗体的调用方法类
    /// </summary>
    public class SplashForm
    {
        private static object _obj = new object();

        private static Form _splashForm = null;

        private static Thread _splashThread = null;

        private delegate void ChangeProgressTextDelegate(string s);

        public static void Show()
        {
            if (_splashThread != null)
                return;

            _splashThread = new Thread(new ThreadStart(delegate ()
            {
                CreateInstance(typeof(FormSplash));
                Application.Run(_splashForm);
            }));

            _splashThread.IsBackground = true;
            _splashThread.SetApartmentState(ApartmentState.STA);
            _splashThread.Start();
        }

        public static void ChangeProgressText(string status)
        {
            ChangeProgressTextDelegate de = new ChangeProgressTextDelegate(ChangeText);
            _splashForm.Invoke(de, status);
        }

        public static void Close()
        {
            if (_splashThread == null || _splashForm == null)
                return;

            try
            {
                _splashForm.Invoke(new MethodInvoker(_splashForm.Close));
            }
            catch (Exception)
            {

            }
            _splashThread = null;
            _splashForm = null;
        }

        private static void ChangeText(string title)
        {
            _splashForm.Text = title.ToString();
        }

        private static void CreateInstance(Type FormType)
        {
            if (_splashForm == null)
            {
                lock (_obj)
                {
                    object obj = FormType.InvokeMember(null,
                                        BindingFlags.DeclaredOnly |
                                        BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                    _splashForm = obj as Form;
                    _splashForm.TopMost = true;
                    _splashForm.ShowInTaskbar = false;
                    _splashForm.BringToFront();
                    _splashForm.StartPosition = FormStartPosition.CenterScreen;
                    if (_splashForm == null)
                    {
                        throw (new Exception());
                    }
                }
            }
        }
    }
}
