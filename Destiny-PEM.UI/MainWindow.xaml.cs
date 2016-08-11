using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using DestinyPEM.Analysis;
using DestinyPEM.Discrete;
using DestinyPEM.Logging;
using DestinyPEM.Model;

namespace DestinyPEM.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Model.GalaxyMap galaxyMap = new GalaxyMap();
		private WindowLogger windowLogger;

		private void InitializeLogger()
		{
			windowLogger = new WindowLogger(DebugOutput);

			Logger.OnMessage += s => windowLogger.LogMessage(s);
			Logger.OnWarning += s => windowLogger.LogWarning(s);
			Logger.OnError += s => windowLogger.LogError(s);
		}

		public MainWindow()
		{
			InitializeComponent();
			InitializeLogger();


			//	Generate galaxy map
			galaxyMap = GalaxyMapBuilder.Build(
				travelTimesFileText: File.ReadAllText(ConfigurationManager.AppSettings["LocationTravelTimesFile"]),
				locationsListFileText: File.ReadAllText(ConfigurationManager.AppSettings["LocationsListingFile"])
				);

			ReportDiscrepancies(galaxyMap);

			//	Load events
			var eventsSource = new PublicEventsSource(galaxyMap);

			var availableEvents = eventsSource.PollForEvents(DateTime.UtcNow);
			galaxyMap.SetEvents(availableEvents);



			var playlistSolver = new PublicEventPlaylistSolver(galaxyMap);
			var optimalOrder = playlistSolver.BuildOptimalEventsOrder(galaxyMap.OrbitLocation, DateTime.Now);

			Logger.LogMessage("Optimal:");
			foreach (var e in optimalOrder)
			{
				Logger.LogMessage("Go to event at {0}::{1}, starting at {2}", e.Location.Planet.Name, e.Location.Name,
					e.StartTime.ToLocalTime().ToShortTimeString());
			}

			Logger.LogMessage("Naive:");
			var nearestEvents = NearestEventsSolver(galaxyMap.OrbitLocation, DateTime.Now);
			foreach (var e in nearestEvents)
			{
				Logger.LogMessage("Go to event at {0}::{1}, starting at {2}", e.Location.Planet.Name, e.Location.Name,
					e.StartTime.ToLocalTime().ToShortTimeString());
			}
		}

		List<Event> NearestEventsSolver(Location startEvent, DateTime startTime)
		{
			var result = new List<Event>();
			var availableEvents = galaxyMap.AllEvents;
			startTime = startTime.ToUniversalTime();
			do
			{

				IEnumerable<Event> events;
				if (result.Count > 0)
					events = availableEvents.Where(e => (e.StartTime - result.Last().EndTime).Ticks > 0)
						.OrderBy(e => e.StartTime.Ticks);
				else
					events = availableEvents.OrderBy(e => e.StartTime.Ticks);

				result.Add(events.First());

			} while (availableEvents.Any(e => (e.StartTime - result.Last().EndTime).Ticks > 0));

			return result;
		}

		void ReportDiscrepancies(GalaxyMap galaxyMap)
		{
			GalaxyMapAnalyser analyser = new GalaxyMapAnalyser(galaxyMap);

			foreach (var link in analyser.FindRedundantLinks())
				Logger.LogWarning("Redundant link on {0} between {1} and {2}", link.ImpliedPlanet.Name, link.Connections[0].Name, link.Connections[1].Name);

			foreach (var location in analyser.FindRedundantLocations())
				Logger.LogWarning("Redundant location '{0}' on {1}", location.Name, location.Planet.Name);

			foreach (var location in analyser.FindUnusedLocations())
				Logger.LogWarning("Unused location '{0}' on {1}", location.Name, location.Planet.Name);
		}
	}
}
