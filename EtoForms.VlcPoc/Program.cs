using System;
using Eto.Forms;
using EtoForms.VlcPoc.ControlHandlers;
using EtoForms.VlcPoc.Controls;
using LibVLCSharp.Shared;

namespace EtoForms.VlcPoc
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			var platform = new Eto.GtkSharp.Platform();
			platform.Add<VideoView.IVideoView>(() => new VideoViewGtkHandler());
			
			Core.Initialize();
			new Application(platform).Run(new MainForm());
		}
	}
}