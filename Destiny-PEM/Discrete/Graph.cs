using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Discrete
{
	public class Graph
	{
		/// <summary>
		/// The domain object that this graph represents
		/// </summary>
		public object DomainObject;

		public List<Node> Nodes = new List<Node>();
		public List<Edge> Edges = new List<Edge>();

		public Node NodeForDomainObject(object domainObject)
		{
			return Nodes.SingleOrDefault(n => n.DomainObject == domainObject);
		}

		public Edge EdgeForDomainObject(object domainObject)
		{
			return Edges.SingleOrDefault(e => e.DomainObject == domainObject);
		}
	}
}
