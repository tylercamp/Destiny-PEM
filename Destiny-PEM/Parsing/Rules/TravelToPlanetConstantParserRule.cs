using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DestinyPEM.Logging;
using DestinyPEM.Model.PredictionConstants;

namespace DestinyPEM.Parsing.Rules
{
	public class TravelToPlanetConstantParserRule : IParserRule
	{
		Regex matcher = new Regex(@"\:TravelTo(\w+)Time\s*\=\s*(\d+)\:(\d{1,2})$");

		public bool CanParse(string line)
		{
			var match = matcher.Match(line);
			return match.Groups.Count == 4;
		}

		public object ParseLine(string line)
		{
			var match = matcher.Match(line);

			String planetName = match.Groups[1].Value;
			String constantValue_MinutesText = match.Groups[2].Value;
			String constantValue_SecondsText = match.Groups[3].Value;

			int constantValue_Minutes;
			int constantValue_Seconds;
			if (!int.TryParse(constantValue_MinutesText, out constantValue_Minutes) ||
				!int.TryParse(constantValue_SecondsText, out constantValue_Seconds))
			{
				Logger.LogError("Invalid format for variable at line {0}.", line);
				return null;
			}

			TimeSpan equivalentTimeSpan = new TimeSpan(0, 0, constantValue_Minutes, constantValue_Seconds);



			object result = new TravelToPlanetTime(equivalentTimeSpan, planetName);

			return result;
		}
	}
}
