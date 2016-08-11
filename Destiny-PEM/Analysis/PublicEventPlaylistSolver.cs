using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DestinyPEM.Analysis.Filtering;
using DestinyPEM.Logging;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis
{
	public class PublicEventPlaylistSolver
	{
		public GalaxyMap Reference { get; private set; }

		public List<Filtering.IEventFilter> EventFilters { get; set; }

		public PublicEventPlaylistSolver(GalaxyMap reference)
		{
			Reference = reference;

			AllowInterPlanetaryTravel = true;

			EventFilters = new List<IEventFilter>()
			{
				new FinishedBeforeArrivalEventFilter()
			};
		}

		public bool AllowInterPlanetaryTravel { get; set; }

		public List<Event> BuildOptimalEventsOrder(Location startLocation, DateTime startTime)
		{
			if (startTime.Kind != DateTimeKind.Utc)
				startTime = startTime.ToUniversalTime();

			ConcurrentBag<GalaxyEventSimulation> finishedSimulations = new ConcurrentBag<GalaxyEventSimulation>();
			int numRunningSimulations = 0;

			//	Build the list of all possible ways that the available events can be traversed
			Action<object> runSimulationDelegate = null;
			runSimulationDelegate = (state) =>
			{
				GalaxyEventSimulation currentSimulation = state as GalaxyEventSimulation;

				var possibleSubSimulations = currentSimulation.GeneratePossibleMoves();
				if (possibleSubSimulations.Count == 0)
				{
					finishedSimulations.Add(currentSimulation);
				}
				else
				{
					foreach (var sim in possibleSubSimulations)
					{
						Interlocked.Increment(ref numRunningSimulations);
						ThreadPool.QueueUserWorkItem(new WaitCallback(runSimulationDelegate), sim);
					}
				}

				Interlocked.Decrement(ref numRunningSimulations);
			};

			numRunningSimulations++;
			GalaxyEventSimulation simulationStart = new GalaxyEventSimulation()
			{
				ReachedEvents = new List<Event>(),
				EventFilters = new List<IEventFilter> {new FinishedBeforeArrivalEventFilter()},
				Reference = this.Reference,
				TraversalSolver = new GalaxyTraversalSolver(Reference),
				SimStartTime = startTime,
				StartLocation = startLocation
			};
			ThreadPool.QueueUserWorkItem(new WaitCallback(runSimulationDelegate), simulationStart);

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			Thread.Sleep(50);

			while (numRunningSimulations > 0)
			{
				Thread.Sleep(1);
			}

			stopwatch.Stop();

			Logger.LogMessage("Built optimal events order in {0:0.00}s, total configurations considered: {1}", stopwatch.Elapsed.TotalSeconds, finishedSimulations.Count);

			return finishedSimulations.OrderByDescending(s => s.ReachedEvents.Count).First().ReachedEvents;
		}
	}
}
