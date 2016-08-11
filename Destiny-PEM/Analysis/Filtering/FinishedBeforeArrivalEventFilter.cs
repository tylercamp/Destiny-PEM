using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis.Filtering
{
	public class FinishedBeforeArrivalEventFilter : IEventFilter
	{
		public bool PassesFilter(Event e, FilterContext context)
		{
			var traversalSolver = context.CurrentSimulation.TraversalSolver;
			var startLocation = context.CurrentSimulation.StartLocation;
			var simStartTime = context.CurrentSimulation.SimStartTime;

			var shortestPathToEvent = traversalSolver.ShortestPathBetweenLocations(startLocation, e.Location);

			var travelTimeMS = shortestPathToEvent.Sum(l => l.TravelTime.Milliseconds);
			var travelTime = TimeSpan.FromMilliseconds(travelTimeMS);

			return ((simStartTime + travelTime).Ticks < e.EndTime.Ticks);
		}
	}
}
