using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Model
{
	public class TravelLinkSpecification
	{
		public String First, Second;
		public String FirstPlanet, SecondPlanet;

		public TimeSpan TravelTime;

		public TravelLinkSpecification(String first, String second, TimeSpan travelTime)
		{
			First = first;
			Second = second;
			TravelTime = travelTime;
		}
	}
}
