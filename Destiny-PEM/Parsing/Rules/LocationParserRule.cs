using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DestinyPEM.Model;

namespace DestinyPEM.Parsing.Rules
{
	public class LocationParserRule : IParserRule
	{
		Regex matcher = new Regex(@"^(\w+)\:\:(.+)$");

		public bool CanParse(string line)
		{
			return matcher.Match(line).Groups.Count == 3;
		}

		public object ParseLine(string line)
		{
			var match = matcher.Match(line);
			var planetName = match.Groups[1].Value.Trim();
			var locationName = match.Groups[2].Value.Trim();

			Model.LocationSpecification locationSpec = new LocationSpecification()
			{
				Name = locationName,
				Planet = planetName
			};

			return locationSpec;
		}
	}
}
