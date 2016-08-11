using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Discrete
{
	public class Edge
	{
		public Node A, B;

		public long Weight;

		public enum EdgeType
		{
			OneWay, TwoWay
		}

		public EdgeType Type = EdgeType.TwoWay;

		public bool Connects(Node a, Node b)
		{
			switch (Type)
			{
				case EdgeType.OneWay:
					return A == a && B == b;

				case EdgeType.TwoWay:
					return (A == a && B == b) || (A == b && B == a);

				default:
					//	Shouldn't happen
					throw new Exception();
			}
		}

		/// <summary>
		/// The domain object that this edge represents
		/// </summary>
		public object DomainObject;
	}
}
