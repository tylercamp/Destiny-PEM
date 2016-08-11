using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Analysis.Filtering
{
	public interface IEventFilter
	{
		bool PassesFilter(Model.Event e, FilterContext context);
	}
}
