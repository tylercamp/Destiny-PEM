using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model
{
	public class GalaxyMap
	{
		public List<IPredictionConstant> Constants { get; set; }
		
		public List<PlanetMap> PlanetMaps { get; set; }

		public IDictionary<String, TimeSpan> TravelToPlanetTime
		{
			get { return Constants.OfType<PredictionConstants.TravelToPlanetTime>().ToDictionary(k => k.PlanetName, v => v.Value); }
		}

		public TimeSpan BackToOrbitTime
		{
			get { return Constants.OfType<PredictionConstants.BackToOrbitTime>().Single().Value; }
		}

		public TimeSpan DefaultTraversalTime
		{
			get { return Constants.OfType<PredictionConstants.DefaultTraversalTime>().Single().Value; }
		}

		public IEnumerable<Location> AllLocations
		{
			get { return PlanetMaps.SelectMany(p => p.Locations); }
		}

		public IEnumerable<Event> AllEvents
		{
			get { return AllLocations.SelectMany(l => l.NearEventsList); }
		}

		public Location OrbitLocation { get; private set; }

		public GalaxyMap()
		{
			Constants = new List<IPredictionConstant>();
			PlanetMaps = new List<PlanetMap>();
			OrbitLocation = new Location()
			{
				Name = "Orbit"
			};
		}

		public void SetEvents(IEnumerable<Event> newEvents)
		{
			foreach (var location in AllLocations)
			{
				location.NearEventsList.Clear();
				location.NearEventsList.AddRange(newEvents.Where(e => e.Location == location));
			}
		}

		public Location FindLocation(String locationName)
		{
			return AllLocations.SingleOrDefault(l => l.Name == locationName);
		}

		//	Index a planet map by its name
		public Dictionary<String, PlanetMap> PlanetMap
		{
			get { return PlanetMaps.ToDictionary(p => p.Name, p => p); }
		}
	}
}
