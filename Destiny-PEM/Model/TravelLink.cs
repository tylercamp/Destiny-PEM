using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Logging;

namespace DestinyPEM.Model
{
	/// <summary>
	/// A link between two Events, physically substantiated by the act of traveling from one Event to another.
	/// </summary>
	public class TravelLink
	{
		public TimeSpan TravelTime { get; set; }
		public readonly List<Location> Connections;

		public PlanetMap ImpliedPlanet
		{
			get { return Connections.First().Planet; }
		}

		public static TravelLink FromSpecification(TravelLinkSpecification spec, IEnumerable<Location> resolvableLocations)
		{
			try
			{
				var possibleFirst = resolvableLocations.Where(l => l.Name == spec.First);
				var first = possibleFirst.Count() > 1
					? possibleFirst.Single(l => l.Planet.Name == spec.FirstPlanet)
					: possibleFirst.Single();

				var possibleSecond = resolvableLocations.Where(l => l.Name == spec.Second);
				var second = possibleSecond.Count() > 1
					? possibleSecond.Single(l => l.Planet.Name == spec.SecondPlanet)
					: possibleSecond.Single();

				return new TravelLink(first, second, spec.TravelTime);
			}
			catch (Exception e)
			{
				Logger.LogError("Unable to create travel-link between locations '{0}' and '{1}' - location not found", spec.First, spec.Second);
				return null;
			}
		}

		public TravelLink(Location a, Location b, TimeSpan travelTime)
		{
			if (a.Planet != b.Planet && a.Planet != null && b.Planet != null)
			{
				Logger.LogError("Invalid link between locations on different planets - '{0}::{1}' and '{2}::{3}'", a.Planet.Name, a.Name, b.Planet.Name, b.Name);
				return;
			}

			TravelTime = travelTime;

			Connections = new List<Location>()
			{
				a,
				b
			};
		}

		public static Event FromName(String name)
		{
			return null;
		}
	}
}
