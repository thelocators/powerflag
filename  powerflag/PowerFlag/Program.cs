using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NLog;
using NLog.Targets;

namespace PowerFlag
{
	static class Program
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args == null || args.Length < 1)
			{
				RichTextBoxTarget target = new RichTextBoxTarget();
				target.Layout = "${message}";
				target.ControlName = "outputRTB";
				target.FormName = "PowerFlag";
				target.UseDefaultRowColoringRules = true;
				NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Trace);
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new PowerFlag());
			}
			else
			{
				if (args[0] == "-silent")
				{
					logger.Info("Started in silent mode.");
					PowerFlag.DoFlagging();
				}
			}
		}
	}
}
