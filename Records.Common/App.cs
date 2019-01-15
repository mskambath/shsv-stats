using System;
using Xwt;
using Xwt.Drawing;

namespace Records.Common
{
	public class App
	{
		public static void Run (ToolkitType type)
		{
			Application.Initialize (type);
			
			MainWindow w = new MainWindow ();
			w.Show ();
			
			Application.Run ();
			
			w.Dispose ();

			Application.Dispose ();
		}
	}
}	

