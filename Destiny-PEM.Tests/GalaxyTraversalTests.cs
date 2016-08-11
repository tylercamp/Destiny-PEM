using System;
using System.IO;
using System.Linq;
using DestinyPEM.Analysis;
using DestinyPEM.Discrete;
using DestinyPEM.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DestinyPEM.Tests
{
	[TestClass]
	public class GalaxyTraversalTests
	{
		[TestMethod]
		public void TestTraversalCorrectness()
		{
			GalaxyMap referenceMap = PrebuiltGalaxyMap.Object;

			GalaxyTraversalSolver solver = new GalaxyTraversalSolver(referenceMap);

			var galaxyConverter = new GalaxyMapConverter();
			Graph galaxyGraph = galaxyConverter.ToGraph(referenceMap);

			var breadthFirstSearch = new BreadthFirstSearch(galaxyGraph);

			foreach (var start in referenceMap.AllLocations)
			{
				foreach (var end in referenceMap.AllLocations.Where(l => l != start))
				{
					var expectedConnectionSet = breadthFirstSearch.ShortestConnectionSet(galaxyGraph.NodeForDomainObject(start),
						galaxyGraph.NodeForDomainObject(end)).ToList();

					var traversalSolution = solver.ShortestPathBetweenLocations(start, end);

					Assert.AreEqual(expectedConnectionSet.Count, traversalSolution.Count);

					for (int i = 0; i < traversalSolution.Count; i++)
					{
						var expectedLink = expectedConnectionSet[i].DomainObject as TravelLink;
						Assert.IsTrue(expectedLink.Connections.All(le => traversalSolution[i].Connections.Any(l => le.Name == l.Name && le.Planet == l.Planet)));
					}
				}
			}
		}
	}
}
