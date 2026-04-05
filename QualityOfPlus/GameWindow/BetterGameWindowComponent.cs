using BepInEx.Configuration;
using PlusLevelStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace QualityOfPlus.GameWindow
{
    class BetterGameWindowComponent : BaseQOLThing
    {
        protected override string CategoryName => "Better Game Window";

        private static ConfigEntry<bool> freeWindowResize;
        private static ConfigEntry<bool> pauseOnFocusLose;

        public static bool FreeWindowResize => freeWindowResize.Value;
        public static bool PauseOnFocusLose => pauseOnFocusLose.Value;

        public override void Initialize()
        {
            freeWindowResize = CreateConfig("Free Window Resize", false, "Allows you to resize the game window freely when in windowed mode");
            pauseOnFocusLose = CreateConfig("Pause On Focus Lose", true, "Pauses the game when the game window loses focus");
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

        private const int WS_CAPTION = 0XC00000;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x80000;
        private const int WS_THICKFRAME = 0x40000;

        private const string UNITY_WND_CLASS = "UnityWndClass";
        private WaitForSeconds wait = new WaitForSeconds(1f);
        private IntPtr WindowHandle = IntPtr.Zero;

        public override IEnumerator OnAPIPre()
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

            StartCoroutine(HandleWindowResize());
        }
        private IEnumerator HandleWindowResize()
        {
            while (true)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    yield break;

                yield return wait;
                if (!FreeWindowResize)
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

        private void PauseWithLevelStudio()
        {
            if (EditorController.Instance == null)
                CoreGameManager.Instance.Pause(true);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (CoreGameManager.Instance == null || CoreGameManager.Instance.disablePause || GlobalCam.Instance.TransitionActive || CoreGameManager.Instance.Paused || !PauseOnFocusLose || hasFocus)
                return;

            if (Compats.LevelStudioInstalled)
                PauseWithLevelStudio();
            else
                CoreGameManager.Instance.Pause(true);
        }
    }
}
