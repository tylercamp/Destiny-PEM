using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model
{
	public class PlanetMap
	{
		public String Name { get; set; }
		public List<Location> Locations { get; set; }
		public List<TravelLink> TravelLinks { get; set; }
		public Location SpawnLocation { get; private set; }

		public PlanetMap()
		{
			Locations = new List<Location>();
			TravelLinks = new List<TravelLink>();

			SpawnLocation = new Location()
			{
				Name = "Spawn",
				Planet = this
			};

			Locations.Add(SpawnLocation);
		}

		public Location FindLocation(String locationName)
		{
			return null;
		}
	}
}
