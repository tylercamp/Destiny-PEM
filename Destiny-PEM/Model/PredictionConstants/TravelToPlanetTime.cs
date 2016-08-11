using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model.PredictionConstants
{
	public class TravelToPlanetTime : IPredictionConstant
	{
		public TravelToPlanetTime(TimeSpan value, String planetName)
		{
			Value = value;
			PlanetName = planetName;
		}

		public TimeSpan Value { get; private set; }

		public String PlanetName { get; private set; }
	}
}
