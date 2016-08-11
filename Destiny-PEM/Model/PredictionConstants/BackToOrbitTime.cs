using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model.PredictionConstants
{
	public class BackToOrbitTime : IPredictionConstant
	{
		public BackToOrbitTime(TimeSpan value)
		{
			Value = value;
		}

		public TimeSpan Value { get; private set; }
	}
}
