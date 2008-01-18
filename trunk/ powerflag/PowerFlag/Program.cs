using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PowerFlag
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args == null || args.Length < 1)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new PowerFlag());
			}
			else
			{
				if (args[0] == "-silent")
				{
					
				}
			}
		}
	}
}
