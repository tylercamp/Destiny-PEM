using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace DestinyPEM.Analysis
{
	public class GalaxyMapAnalyser
	{
		public Model.GalaxyMap Reference { get; private set; }

		public GalaxyMapAnalyser(Model.GalaxyMap reference)
		{
			Reference = reference;
		}

		public IEnumerable<Model.Location> FindUnusedLocations()
		{
			var links = Reference.PlanetMaps.SelectMany(m => m.TravelLinks);
			return Reference.AllLocations.Where(loc => !links.Any(link => link.Connections.Contains(loc)));
		}

		public IEnumerable<Model.Location> FindRedundantLocations()
		{
			return
				Reference.AllLocations.Where(
					l => Reference.AllLocations.Any(l2 => l2.Name == l.Name && l2.Planet == l.Planet && l != l2)).Reverse();
		}

		public IEnumerable<Model.TravelLink> FindRedundantLinks()
		{
			//	For each link, see if there are any other links in this planet with the same connections (that are not the current link)
			return
				Reference.PlanetMaps.SelectMany(
					p => p.TravelLinks.Where(l => p.TravelLinks.Any(l2 => !l2.Connections.Except(l.Connections).Any() && l != l2))).Reverse();
		}
	}
}
