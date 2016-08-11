using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model
{
	public class Location
	{
		public String Name { get; set; }

		public PlanetMap Planet { get; set; }

		public List<Event> NearEventsList { get; set; }

		public Location()
		{
			NearEventsList = new List<Event>();
		}

		public override int GetHashCode()
		{
			int hashCode = Name.GetHashCode();
			if (Planet != null)
				hashCode ^= Planet.Name.GetHashCode();

			return hashCode;
		}

		public static Location FromSpecification(LocationSpecification spec, PlanetMap planet)
		{
			return new Location{Name = spec.Name, Planet = planet};
		}
	}
}
