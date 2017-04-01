using System;
using Gtk;
using System.Threading;

public partial class MainWindow: Gtk.Window
{
	Thread xt;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		this.AllowGrow = false;
		this.AllowShrink = false;
		this.DefaultWidth = 330;
		this.DefaultHeight = 200;
		this.SetSizeRequest (330, 200);
		this.Title = "UART Dongle Self Test v1.0";

		ScanPorts ();

		this.btnSend.Clicked += BtnSend_Clicked;
		this.btnQuit.Clicked += BtnQuit_Clicked;
		this.btnScan.Clicked += BtnScan_Clicked;


	}

	protected void ScanPorts()
	{
		string[] ports = System.IO.Ports.SerialPort.GetPortNames ();
		foreach (string port in ports) {
			if (port.Contains ("ttyUSB")) {
				//MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, port);
				//md.Run ();
				//md.Destroy();
				this.comPorts.AppendText (port);
			}

		}
		if (ports.Length > 0)
			this.comPorts.Active = 0;
	}

	void BtnScan_Clicked (object sender, EventArgs e)
	{
		this.comPorts.Clear();
		CellRendererText cell = new CellRendererText();
		this.comPorts.PackStart(cell, false);
		this.comPorts.AddAttribute(cell, "text", 0);
		ListStore store = new ListStore(typeof (string));
		this.comPorts.Model = store;
		ScanPorts ();

	}

	void BtnQuit_Clicked (object sender, EventArgs e)
	{
		if (xt != null)
			xt.Abort ();
		this.Hide ();
		Application.Quit ();
	}

	void BtnSend_Clicked (object sender, EventArgs e)
	{
		if (xt != null)
			xt.Abort ();
		xt = new Thread (new ThreadStart (LoopData));
		xt.Start ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public void LoopData()
	{
		string sActivePort = this.comPorts.ActiveText;
		System.IO.Ports.SerialPort xPort = new System.IO.Ports.SerialPort (sActivePort, 9600, System.IO.Ports.Parity.None, 8);
		if (xPort.IsOpen)
			xPort.Close ();

		xPort.Open ();
		for (int i = 0; i < 25; i++) {
			for (int j = 0; j < 10; j++)
				xPort.Write ("hello world!");
			Thread.Sleep (400);
		}
		xPort.Close ();
	}

		
}
