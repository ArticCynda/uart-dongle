using System;
using System.IO.Ports;
using Gtk;

namespace loopback
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();


			


			win.Show ();
			Application.Run ();
		}
	}

	public class MessageBox
	{
		public static void Show(string Msg)
		{
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, Msg);
			md.Run ();
			md.Destroy();
		}
	}
}
