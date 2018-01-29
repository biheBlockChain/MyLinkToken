using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyLinkToken.WinFormEx
{
    /// <summary>
    /// 窗体展示动态效果
    /// 【一般用在 Form_Load() 与 Form_Closing()事件中】
    /// </summary>
    class AnimateWindows
    {
        #region 调用系统API
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        /// <summary>
        /// 普通显示.【激活窗口。在使用了AW_HIDE标志后不要使用这个标志】
        /// </summary>
        public const int AW_ACTIVATE = 0x20000;

        /// <summary>
        /// 从左向右显示.【该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略】
        /// </summary>
        public const int AW_HOR_POSITIVE = 0x0001;

        /// <summary>
        /// 从右向左显示.【该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略】
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0x0002;

        /// <summary>
        /// 从上到下显示.【该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略】
        /// </summary>
        public const int AW_VER_POSITIVE = 0x0004;

        /// <summary>
        /// 从下到上显示.【该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略】
        /// </summary>
        public const int AW_VER_NEGATIVE = 0x0008;

        /// <summary>
        /// 从中间向四周.【若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展】
        /// </summary>
        public const int AW_CENTER = 0x0010;

        /// <summary>
        /// 隐藏窗口.【缺省则显示窗口】
        /// </summary>
        public const int AW_HIDE = 0x10000;

        /// <summary>
        /// 使用滑动类型.【缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略】
        /// </summary>
        public const int AW_SLIDE = 0x40000;

        /// <summary>
        /// 使用淡出效果【只有当hWnd为顶层窗口的时候才可以使用此标志】
        /// </summary>
        public const int AW_BLEND = 0x80000;
        #endregion

        /// <summary>
        /// 窗体显示时的动画效果
        /// </summary>
        /// <param name="form">目标窗体对象</param>
        /// <param name="dwTime">自定义动画时间，单位：毫秒</param>
        /// <param name="dwFalg">自定义组合动画标准。通过该类中的常量，自由组合。例如：AW_CENTER 或者 AW_SLIDE | AW_HOR_POSITIVE | AW_VER_POSITIVE 等</param>
        public static void ShowAnimateWindow(Form form, int dwTime, int dwFalg)
        {
            IntPtr hwnd = form.Handle;//窗体句柄
            int time = dwTime;//动画时间

            AnimateWindow(hwnd, time, dwFalg);
        }

        /// <summary>
        /// 窗体关闭或者隐藏时的动画效果(滑动类型)
        /// </summary>
        /// <param name="form">目标窗体对象</param>
        /// <param name="dwTime">自定义动画时间，单位：毫秒</param>
        /// <param name="dwFalg">自定义组合动画标志。通过该类中的常量，自由组合。
        /// 已经配置好了隐藏属性，只需要提供特效即可。
        /// 特效标志使用以下值：AW_HOR_POSITIVE、AW_HOR_NEGATIVE、AW_VER_POSITIVE、AW_VER_NEGATIVE、AW_CENTER
        /// </param>
        public static void HideAnimateWindowSlide(Form form, int dwTime, int dwFalg)
        {
            IntPtr hwnd = form.Handle;//窗体句柄
            int time = dwTime;//动画时间

            AnimateWindow(hwnd, time, AW_HIDE | AW_SLIDE | dwFalg);
        }

        /// <summary>
        /// 窗体关闭或者隐藏时的动画效果(淡入淡出类型)
        /// </summary>
        /// <param name="form">目标窗体对象</param>
        /// <param name="dwTime">自定义动画时间，单位：毫秒</param>
        /// <param name="dwFalg">自定义组合动画标志。通过该类中的常量，自由组合。
        /// 已经配置好了隐藏属性，只需要提供特效即可。
        /// 特效标志使用以下值：AW_HOR_POSITIVE、AW_HOR_NEGATIVE、AW_VER_POSITIVE、AW_VER_NEGATIVE、AW_CENTER
        /// </param>
        public static void HideAnimateWindowBlend(Form form, int dwTime, int dwFalg)
        {
            IntPtr hwnd = form.Handle;//窗体句柄
            int time = dwTime;//动画时间

            AnimateWindow(hwnd, time, AW_HIDE | AW_BLEND | dwFalg);
        }

        /// <summary>
        /// 窗体显示时的动画效果
        /// </summary>
        /// <param name="form">目标窗体对象</param>
        /// <param name="dwTime">自定义动画时间，单位：毫秒</param>
        public static void ShowAnimateWindowRandom(Form form, int dwTime)
        {
            int animateType = 10;
            Random a = new Random();
            int dwFlags = (int)a.Next(animateType);

            int time = dwTime;//动画时间

            IntPtr hwnd = form.Handle;//窗体句柄

            switch (dwFlags)
            {
                case 0://普通显示
                    AnimateWindow(hwnd, time, AW_ACTIVATE);
                    break;
                case 1://从左向右显示
                    AnimateWindow(hwnd, time, AW_HOR_POSITIVE);
                    break;
                case 2://从右向左显示
                    AnimateWindow(hwnd, time, AW_HOR_NEGATIVE);
                    break;
                case 3://从上到下显示
                    AnimateWindow(hwnd, time, AW_VER_POSITIVE);
                    break;
                case 4://从下到上显示
                    AnimateWindow(hwnd, time, AW_VER_NEGATIVE);
                    break;
                case 5://透明渐变显示
                    AnimateWindow(hwnd, time, AW_BLEND);
                    break;
                case 6://从中间向四周
                    AnimateWindow(hwnd, time, AW_CENTER);
                    break;
                case 7://左上角伸展
                    AnimateWindow(hwnd, time, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_POSITIVE);
                    break;
                case 8://左下角伸展
                    AnimateWindow(hwnd, time, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_NEGATIVE);
                    break;
                case 9://右上角伸展
                    AnimateWindow(hwnd, time, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_POSITIVE);
                    break;
                case 10://右下角伸展
                    AnimateWindow(hwnd, time, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_NEGATIVE);
                    break;
            }
        }
    }
}
