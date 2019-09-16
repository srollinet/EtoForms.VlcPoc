using System;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Eto.GtkSharp;
using Eto.GtkSharp.Forms;
using EtoForms.VlcPoc.Controls;
using Gtk;
using LibVLCSharp.Shared;
using Window = Gdk.Window;

namespace EtoForms.VlcPoc.ControlHandlers
{
    public class VideoViewGtkHandler : GtkControl<DrawingArea, VideoView, Control.ICallback>, VideoView.IVideoView
    {
        struct Native
        {
            /// <summary>
            /// Gets the window's HWND
            /// </summary>
            /// <remarks>Window only</remarks>
            /// <param name="gdkWindow">The pointer to the GdkWindow object</param>
            /// <returns>The window's HWND</returns>
            [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr gdk_win32_drawable_get_handle(IntPtr gdkWindow);

            /// <summary>
            /// Gets the window's XID
            /// </summary>
            /// <remarks>Linux X11 only</remarks>
            /// <param name="gdkWindow">The pointer to the GdkWindow object</param>
            /// <returns>The window's XID</returns>
            [DllImport("libgdk-x11-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
            internal static extern uint gdk_x11_drawable_get_xid(IntPtr gdkWindow);

            /// <summary>
            /// Gets the nsview's handle
            /// </summary>
            /// <remarks>Mac only</remarks>
            /// <param name="gdkWindow">The pointer to the GdkWindow object</param>
            /// <returns>The nsview's handle</returns>
            [DllImport("libgdk-quartz-2.0.0.dylib")]
            internal static extern IntPtr gdk_quartz_window_get_nsview(IntPtr gdkWindow);
        }
        
        private MediaPlayer _mediaPlayer;
        
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            set
            {
                if (ReferenceEquals(_mediaPlayer, value))
                {
                    return;
                }

                Detach();
                _mediaPlayer = value;
                Attach();
            }
        }

        private bool IsRealized => Control.IsRealized;

        private Window GdkWindow => Control.Window;

        public VideoViewGtkHandler()
        {
            Control = new DrawingArea();
            Control.SetBackground(Colors.Black);
            Control.Realized += (sender, args) => Attach();
        }

        private void Attach()
        {
            if (!IsRealized || _mediaPlayer == null)
            {
                return;
            }

            if (PlatformHelper.IsWindows)
            {
                MediaPlayer.Hwnd = Native.gdk_win32_drawable_get_handle(GdkWindow.Handle);
            }
            else if (PlatformHelper.IsLinux)
            {
                GdkWindow.EnsureNative();
                MediaPlayer.XWindow = Native.gdk_x11_drawable_get_xid(GdkWindow.Handle);
            }
            else if (PlatformHelper.IsMac)
            {
                MediaPlayer.NsObject = Native.gdk_quartz_window_get_nsview(GdkWindow.Handle);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private void Detach()
        {
            if (!IsRealized || _mediaPlayer == null)
            {
                return;
            }

            if (PlatformHelper.IsWindows)
            {
                MediaPlayer.Hwnd = IntPtr.Zero;
            }
            else if (PlatformHelper.IsLinux)
            {
                MediaPlayer.XWindow = 0;
            }
            else if (PlatformHelper.IsMac)
            {
                MediaPlayer.NsObject = IntPtr.Zero;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Detach();
            }
            base.Dispose(disposing);
        }
    }
}