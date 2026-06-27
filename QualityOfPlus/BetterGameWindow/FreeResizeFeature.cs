using BepInEx.Configuration;
using QualityOfPlus.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.BetterGameWindow
{
    public class FreeResizeFeature : QOPFeature, IToggleableFeature, IOnAPIStart
    {
        public bool ValueIfNull => false;
        public ConfigEntry<bool> Enabled { get; private set; }

        public override string ID => "QOP.FEATURE.FREE.WINDOW.RESIZE";

        public override void PreInitialize(QOPCategory category)
        {
            Enabled = category.CreateEntry<bool>("Free Window Resize", false, "Make the game window freely resizable in windowed mode");
        }
        public override void PostInitialize(QOPCategory category)
        {
        }

        public IEnumerator APIStartAction()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                yield break;

            yield return $"Initializing window resize coroutine";
            int pid = Process.GetCurrentProcess().Id;
            EnumWindows((w, param) =>
            {
                if (w == IntPtr.Zero)
                    return true;
                if (GetWindowThreadProcessId(w, out uint lpdwProcessId) == 0)
                    return true;
                if (lpdwProcessId != pid)
                    return true;

                StringBuilder cn = new StringBuilder(256);
                if (GetClassName(w, cn, cn.Capacity) == 0)
                    return true;

                if (cn.ToString() != UNITY_WND_CLASS)
                    return true;

                WindowHandle = w;
                return false;
            }, IntPtr.Zero);

            if (WindowHandle == IntPtr.Zero)
                yield break;

            BasePlugin.Instance.StartCoroutine(HandleWindowResize());

        }
        private IEnumerator HandleWindowResize()
        {
            while (true)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    yield break;

                yield return wait;
                if (!this.IsEnabled())
                    continue;

                bool fullScreen = Screen.fullScreen;
                int windowStyle = GetWindowLong(WindowHandle, GWL_STYLE);
                int resizableStyle = windowStyle & (WS_THICKFRAME | WS_MAXIMIZEBOX);
                if (!fullScreen && resizableStyle == 0)
                {
                    int newStyle = windowStyle | WS_THICKFRAME | WS_MAXIMIZEBOX;
                    SetWindowLong(WindowHandle, GWL_STYLE, newStyle);
                }

                yield return wait;
            }
        }


        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;

        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_THICKFRAME = 0x40000;

        private const string UNITY_WND_CLASS = "UnityWndClass";
        private WaitForSeconds wait = new WaitForSeconds(1f);
        private IntPtr WindowHandle = IntPtr.Zero;
    }
}
