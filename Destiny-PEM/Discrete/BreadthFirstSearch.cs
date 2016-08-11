using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Analysis;
using DestinyPEM.Model;

namespace DestinyPEM.Discrete
{
	public class BreadthFirstSearch
	{
		public Graph Reference { get; private set; }

		//	Support A*-like modified weighting via heuristic (weight modifiers)
		public List<IWeightModifier> WeightModifiers { get; private set; } 

		public BreadthFirstSearch(Graph reference)
		{
			Reference = reference;
			WeightModifiers = new List<IWeightModifier>
			{
				//	Improves search speed by a factor of 5+
				new DifferentPlanetsWeightModifier()
			};
		}

		private long CalculateCostOfSolution(List<Node> solution, Node startNode, Node destinationNode)
		{
			long totalCost = 0;

			for (int i = 0; i < solution.Count - 2; i++)
			{
				var possibleEdges = Reference.Edges.Where(e => e.Connects(solution[i], solution[i + 1]));
				if (possibleEdges.Count() > 1)
					Debugger.Break();

				var edge = possibleEdges.Single();
				totalCost += edge.Weight;

				foreach (var modifier in WeightModifiers)
					totalCost += modifier.ModifyEdgeWeight(solution[i], solution[i + 1], startNode, destinationNode);
			}

			return totalCost;
		}

		public IEnumerable<Edge> ShortestConnectionSet(Node start, Node end)
		{
			List<List<Node>> possibleSolutions = new List<List<Node>>()
			{
				new List<Node> {start}
			};

			List<Node> solution = null;
			List<Node> currentSolution;
			while ((currentSolution = possibleSolutions.OrderBy(s => CalculateCostOfSolution(s, start, end)).FirstOrDefault()) != null)
			{
				possibleSolutions.Remove(currentSolution);
				var currentNode = currentSolution.Last();
				if (currentNode == end)
				{
					solution = currentSolution;
					break;
				}

				foreach (var node in Reference.Nodes.Where(n => Reference.Edges.Any(e => e.Connects(currentNode, n))))
				{
					if (currentSolution.Contains(node))
						continue;

					var newPossibleSolution = new List<Node>(currentSolution) {node};
					possibleSolutions.Add(newPossibleSolution);
				}
			}


			if (solution != null)
			{
				List<Edge> result = new List<Edge>();
				for (int i = 0; i < solution.Count - 1; i++)
				{
					var edge = Reference.Edges.Single(e => e.Connects(solution[i], solution[i + 1]));
					//	Return a new edge that will have the correct directional metadata (since many edges may be bidirectional, this
					//		isn't helpful when returning a purely directed graph)
					result.Add(new Edge
					{
						A = solution[i],
						B = solution[i + 1],
						DomainObject = edge.DomainObject,
						Type = Edge.EdgeType.OneWay,
						Weight = edge.Weight
					});
				}

				return result;
			}

			return null;
		}
	}
}
