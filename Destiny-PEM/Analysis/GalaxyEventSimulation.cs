using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Analysis.Filtering;
using DestinyPEM.Model;
using DestinyPEM.Discrete;

namespace DestinyPEM.Analysis
{
	public class GalaxyEventSimulation
	{
		public List<Event> ReachedEvents;
		public List<IEventFilter> EventFilters;
		public Location StartLocation;
		public GalaxyMap Reference;
		public GalaxyTraversalSolver TraversalSolver;
		public DateTime SimStartTime;

		public List<GalaxyEventSimulation> GeneratePossibleMoves()
		{
			List<GalaxyEventSimulation> result = new List<GalaxyEventSimulation>();

			foreach (var possibleEvent in Reference.AllEvents.Where(e => e.StartTime.Ticks > SimStartTime.Ticks))
			{
				var filterContext = new FilterContext {CurrentSimulation = this};
				if (EventFilters.Any(f => !f.PassesFilter(possibleEvent, filterContext)))
					continue;


				result.Add(new GalaxyEventSimulation()
				{
					ReachedEvents = new List<Event>(ReachedEvents) { possibleEvent },
					EventFilters = this.EventFilters,
					Reference = this.Reference,
					TraversalSolver = this.TraversalSolver,
					StartLocation = possibleEvent.Location,
					SimStartTime = possibleEvent.EndTime
				});
			}

			return result;
		}
	}
}
