using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Model;

namespace DestinyPEM.Analysis.Filtering
{
	public class MaximumRarityEventFilter : IEventFilter
	{
		public int MaximumRarity { get; set; }

		public bool PassesFilter(Event e, FilterContext context)
		{
			return e.Rarity <= MaximumRarity;
		}
	}
}
