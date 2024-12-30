using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;

namespace MiniWriter
{
    public class HotKeyHelper
    {
        private readonly Window _window;
        private readonly HwndSource _source;
        private Action _callback;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public HotKeyHelper(Window window)
        {
            _window = window;
            _source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
            _source.AddHook(WndProc);
        }

        public void RegisterHotKey(ModifierKeys modifier, Key key, Action callback)
        {
            var modifierKeys = (uint)modifier;
            var vk = (uint)KeyInterop.VirtualKeyFromKey(key);
            _callback = callback;

            RegisterHotKey(_source.Handle, 1, modifierKeys, vk);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                _callback?.Invoke();
                handled = true;
            }
            return IntPtr.Zero;
        }

        public void Dispose()
        {
            UnregisterHotKey(_source.Handle, 1);
            _source.RemoveHook(WndProc);
        }
    }
} 