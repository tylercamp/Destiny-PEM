using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Discrete
{
	public interface IWeightModifier
	{
		long ModifyEdgeWeight(Node currentNode, Node nextNode, Node traversalStart, Node traversalEnd);
	}
}
