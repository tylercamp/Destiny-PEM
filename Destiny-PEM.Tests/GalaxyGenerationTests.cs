using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Analysis;
using DestinyPEM.Discrete;
using DestinyPEM.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DestinyPEM.Tests
{
	[TestClass]
	public class GalaxyGenerationTests
	{
		[TestMethod]
		public void TestPlanetaryInterconnection()
		{
			var galaxy = PrebuiltGalaxyMap.Object;

			var converter = new GalaxyMapConverter();
			var graph = converter.ToGraph(galaxy);
			var graphSearcher = new BreadthFirstSearch(graph);
			foreach (var planet in galaxy.PlanetMaps)
			{
				foreach (Location start in planet.Locations)
				{
					foreach (Location end in planet.Locations.Where(l => l != start))
					{
						//	Test that the graph is fully interconnected
						Assert.IsTrue(graphSearcher.ShortestConnectionSet(graph.NodeForDomainObject(start), graph.NodeForDomainObject(end)) != null);
					}
				}
			}
		}
	}
}
