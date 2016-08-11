using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis.Filtering
{
	class RequireSamePlanetEventFilter : IEventFilter
	{
		public bool PassesFilter(Event e, FilterContext context)
		{
			if (context.CurrentSimulation.StartLocation.Planet == null)
				return true;

			return e.Location.Planet == context.CurrentSimulation.StartLocation.Planet;
		}
	}
}
