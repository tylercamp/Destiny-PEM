using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Discrete;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis
{
	public class GalaxyMapConverter
	{
		public Graph ToGraph(GalaxyMap map)
		{
			Graph result = new Graph();

			var orbitNode = new Node() { DomainObject = map.OrbitLocation };
			result.Nodes.Add(orbitNode);

			foreach (var planet in map.PlanetMaps)
			{
				//	Add spawn node for planet, add link between orbit and spawn node
				var planetSpawnNode = new Node {DomainObject = planet.SpawnLocation};
				result.Nodes.Add(planetSpawnNode);
				result.Edges.Add(new Edge
				{
					A = orbitNode,
					B = planetSpawnNode,
					DomainObject = new TravelLink(orbitNode.DomainObject as Location, planetSpawnNode.DomainObject as Location, map.TravelToPlanetTime[planet.Name]),
					Type = Edge.EdgeType.OneWay,
					Weight = map.TravelToPlanetTime[planet.Name].Ticks
				});

				//	Add all locations as nodes
				foreach (var location in planet.Locations)
				{
					Node newNode;
					if (location != planet.SpawnLocation)
					{
						newNode = new Node { DomainObject = location };
						result.Nodes.Add(newNode);
					}
					else
					{
						newNode = planetSpawnNode;
					}

					//	We can go back to orbit from any node
					result.Edges.Add(new Edge
					{
						A = newNode,
						B = orbitNode,
						DomainObject =
							new TravelLink(newNode.DomainObject as Location, orbitNode.DomainObject as Location, map.BackToOrbitTime),
						Type = Edge.EdgeType.OneWay,
						Weight = map.BackToOrbitTime.Ticks
					});
				}

				//	Add travel links as edges, connections between spawn and other areas should have been loaded via travel-times.txt
				foreach (var travelLink in planet.TravelLinks)
				{
					result.Edges.Add(new Edge
					{
						A = result.Nodes.Single(n => n.DomainObject == travelLink.Connections[0]),
						B = result.Nodes.Single(n => n.DomainObject == travelLink.Connections[1]),
						DomainObject = travelLink,
						Weight = travelLink.TravelTime.Ticks
					});
				}
			}

			return result;
		}
	}
}
