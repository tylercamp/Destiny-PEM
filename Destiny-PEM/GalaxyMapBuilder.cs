using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Logging;
using DestinyPEM.Model;

namespace DestinyPEM
{
	public static class GalaxyMapBuilder
	{
		public static GalaxyMap Build(String travelTimesFileText, String locationsListFileText)
		{
			GalaxyMap galaxyMap = new GalaxyMap();

			var locationParser = new Parsing.LocationParser();
			locationParser.Parse(locationsListFileText);
			var locationSpecs = locationParser.GetAll<Model.LocationSpecification>();

			var impliedPlanetNames = locationSpecs.Select(l => l.Planet).Distinct();
			foreach (var planetName in impliedPlanetNames)
			{
				var newPlanet = new PlanetMap() { Name = planetName };
				newPlanet.Locations.AddRange(locationSpecs.Where(l => l.Planet == planetName).Select(l => Model.Location.FromSpecification(l, newPlanet)).ToList());

				galaxyMap.PlanetMaps.Add(newPlanet);
			}

			var traversalParser = new Parsing.TraversalTimesParser();
			traversalParser.Parse(travelTimesFileText);

			var parsedConstants = traversalParser.GetAll<Model.IPredictionConstant>().ToList();
			galaxyMap.Constants.AddRange(parsedConstants);

			var parsedLinkSpecs = traversalParser.GetAll<Model.TravelLinkSpecification>().ToList();
			var fulfilledLinks = parsedLinkSpecs.Select(l => Model.TravelLink.FromSpecification(l, galaxyMap.AllLocations)).Where(l => l != null);
			foreach (var link in fulfilledLinks)
			{
				if (link.TravelTime == TimeSpan.Zero)
				{
					Logger.LogWarning("No traversal time specified between {0} and {1}, using default time", link.Connections[0].Name, link.Connections[1].Name);
					link.TravelTime = galaxyMap.DefaultTraversalTime;
				}
				link.ImpliedPlanet.TravelLinks.Add(link);
			}

			return galaxyMap;
		}
	}
}
