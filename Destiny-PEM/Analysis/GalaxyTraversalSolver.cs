using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DestinyPEM.Discrete;
using DestinyPEM.Model;
using DestinyPEM.Logging;

namespace DestinyPEM.Analysis
{
	public class GalaxyTraversalSolver
	{
		public GalaxyMap Reference { get; private set; }
		private Graph galaxyGraph;

		private static ConcurrentDictionary<long, List<TravelLink>> cachedTraversals = new ConcurrentDictionary<long, List<TravelLink>>();

		long GenerateTraversalHash(Location start, Location end)
		{
			//	Need to generate a hash where going a -> b and b -> a generate different values
			long lowPart = (start.GetHashCode() & 0x7FFFFFFF);
			long highPart = (end.GetHashCode() & 0x7FFFFFFF);
			highPart <<= 32;

			long hash = highPart | lowPart;
			return hash;
		}

		public GalaxyTraversalSolver(GalaxyMap reference)
		{
			Reference = reference;
			var galaxyConverter = new GalaxyMapConverter();
			galaxyGraph = galaxyConverter.ToGraph(reference);
		}

		public List<TravelLink> ShortestPathBetweenLocations(Location start, Location end)
		{
			long traversalIndex = GenerateTraversalHash(start, end);
			List<TravelLink> result;

			if (cachedTraversals.TryGetValue(traversalIndex, out result))
				return result;

			lock (cachedTraversals)
			{
				if (cachedTraversals.TryGetValue(traversalIndex, out result))
					return result;

				result = new List<TravelLink>();

				Node startNode, endNode;
				endNode = galaxyGraph.NodeForDomainObject(end);

				//	Optimization: If the two locations are on different planets, a return to orbit and travel to spawn MUST occur.
				if (start.Planet != null && end.Planet != null && start.Planet != end.Planet)
				{
					result.Add(new TravelLink(start, Reference.OrbitLocation, Reference.BackToOrbitTime));
					result.Add(new TravelLink(Reference.OrbitLocation, end.Planet.SpawnLocation,
						Reference.TravelToPlanetTime[end.Planet.Name]));
					startNode = galaxyGraph.NodeForDomainObject(end.Planet.SpawnLocation);

					//	If we ended up targetting the spawn node for a location, we're already at the end
					if (startNode == endNode)
					{
						cachedTraversals.TryAdd(traversalIndex, result);
						return result;
					}
				}
				else
				{
					startNode = galaxyGraph.NodeForDomainObject(start);
				}

				var graphSearcher = new BreadthFirstSearch(galaxyGraph);
				result.AddRange(graphSearcher.ShortestConnectionSet(startNode, endNode).Select(e => e.DomainObject as TravelLink));

				cachedTraversals.TryAdd(traversalIndex, result);

				return result;
			}
		}
	}
}
