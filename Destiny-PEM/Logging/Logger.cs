using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DestinyPEM.Logging
{
	public static class Logger
	{
		public static event Action<String> OnMessage;
		public static event Action<String> OnWarning;
		public static event Action<String> OnError;

		public static void LogMessage(String message, params object[] args)
		{
			String formattedMessage = String.Format(message, args);

			if (OnMessage != null)
				OnMessage(formattedMessage);
		}

		public static void LogWarning(String warning, params object[] args)
		{
			String warningMessage = String.Format(warning, args);

			if (OnWarning != null)
				OnWarning(warningMessage);
		}

		public static void LogError(String error, params object[] args)
		{
			String errorMessage = String.Format(error, args);

			//	Allow debugger to catch errors
			if (Debugger.IsAttached)
				Debugger.Break();

			if (OnError != null)
				OnError(errorMessage);
		}
	}
}
