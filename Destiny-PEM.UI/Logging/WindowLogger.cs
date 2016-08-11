using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace DestinyPEM.Logging
{
	public class WindowLogger
	{
		private FlowDocument flowDocument;
		private Paragraph currentTextContainer = new Paragraph();

		private Color MessageColor = Color.FromRgb(0, 0, 0);
		private Color WarningColor = Color.FromRgb(220, 100, 0);
		private Color ErrorColor = Color.FromRgb(200, 0, 0);

		private double FontSize = 12.0;

		private enum LogType
		{
			Message, Warning, Error, Unspecified
		}

		private LogType currentLogType = LogType.Unspecified;

		private Dispatcher Dispatcher
		{
			get { return flowDocument.Dispatcher; }
		}

		public WindowLogger(FlowDocument flowDocumentDocument)
		{
			flowDocument = flowDocumentDocument;
		}

		private Paragraph GetOutputParagraph(LogType logType)
		{
			if (logType != currentLogType)
			{
				Color color;
				switch (logType)
				{
					case LogType.Message:
						color = MessageColor;
						break;
					case LogType.Warning:
						color = WarningColor;
						break;
					case LogType.Error:
						color = ErrorColor;
						break;

					default:
						throw new Exception();
				}

				currentTextContainer = new Paragraph()
				{
					FontSize = FontSize,
					Foreground = new SolidColorBrush(color),
				};

				flowDocument.Blocks.Add(currentTextContainer);
				currentLogType = logType;
			}

			return currentTextContainer;
		}

		public void LogMessage(String message)
		{
			if (!Dispatcher.CheckAccess())
			{
				Dispatcher.BeginInvoke(new Action(() => LogMessage(message)));
				return;
			}

			String formattedMessage = String.Format("(Message) {0}", message);

			var container = GetOutputParagraph(LogType.Message);
			container.Inlines.Add(new Run(formattedMessage));
			container.Inlines.Add(new LineBreak());
		}

		public void LogWarning(String warning)
		{
			if (!Dispatcher.CheckAccess())
			{
				Dispatcher.BeginInvoke(new Action(() => LogWarning(warning)));
				return;
			}

			String formattedWarning = String.Format("(Warning) {0}", warning);

			var container = GetOutputParagraph(LogType.Warning);
			container.Inlines.Add(new Run(formattedWarning));
			container.Inlines.Add(new LineBreak());
		}

		public void LogError(String error)
		{
			if (!Dispatcher.CheckAccess())
			{
				Dispatcher.BeginInvoke(new Action(() => LogError(error)));
				return;
			}

			String formattedError = String.Format("(Error) {0}", error);

			var container = GetOutputParagraph(LogType.Error);
			container.Inlines.Add(new Bold(new Run(formattedError)));
			container.Inlines.Add(new LineBreak());
		}
	}
}
