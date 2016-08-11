using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Discrete;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis
{
	class DifferentPlanetsWeightModifier : IWeightModifier
	{
		private const long DifferentPlanetsWeightValue = 10000000000;

		public long ModifyEdgeWeight(Node currentNode, Node possibleNextNode, Node traversalStart, Node traversalEnd)
		{
			var startLocation = traversalStart.DomainObject as Location;
			var endLocation = traversalEnd.DomainObject as Location;

			var possibleNextLocation = possibleNextNode.DomainObject as Location;

			if (possibleNextLocation.Planet != null && possibleNextLocation.Planet != endLocation.Planet)
				return DifferentPlanetsWeightValue;
			return 0;
		}
	}
}
