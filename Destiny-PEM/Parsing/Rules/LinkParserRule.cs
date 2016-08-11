using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DestinyPEM.Logging;
using DestinyPEM.Model;

namespace DestinyPEM.Parsing.Rules
{
	public class LinkParserRule : IParserRule
	{
		Regex matcher = new Regex(@"^([\w\s-'\:\/]*)\<\-\>([\w\s-'\/\:]*)\=\s*(\d+)\:(\d+)");

		public bool CanParse(string line)
		{
			return matcher.Match(line).Groups.Count == 5;
		}

		public object ParseLine(string line)
		{
			var match = matcher.Match(line);

			var linkSourceName = match.Groups[1].Value.Trim();
			var linkTargetName = match.Groups[2].Value.Trim();
			var linkDuration_MinutesText = match.Groups[3].Value.Trim();
			var linkDuration_SecondsText = match.Groups[4].Value.Trim();

			int constantValue_Minutes;
			int constantValue_Seconds;
			if (!int.TryParse(linkDuration_MinutesText, out constantValue_Minutes) ||
				!int.TryParse(linkDuration_SecondsText, out constantValue_Seconds))
			{
				Logger.LogError("Invalid format for variable at line '{0}'.", line);
				return null;
			}

			TimeSpan equivalentTimeSpan = new TimeSpan(0, 0, constantValue_Minutes, constantValue_Seconds);

			//	Locations are either of format 'Location' or 'Planet::Location', extract info as appropriate
			String sourceLocationName = ExtractLocationFromLocationName(linkSourceName);
			String sourcePlanetName = ExtractPlanetFromLocationName(linkSourceName);
			String targetLocationName = ExtractLocationFromLocationName(linkTargetName);
			String targetPlanetName = ExtractPlanetFromLocationName(linkTargetName);
			Model.TravelLinkSpecification newLink = new TravelLinkSpecification(sourceLocationName, targetLocationName, equivalentTimeSpan);
			newLink.FirstPlanet = sourcePlanetName;
			newLink.SecondPlanet = targetPlanetName;
			
			return newLink;
		}

		private String ExtractPlanetFromLocationName(String locationString)
		{
			var components = locationString.Split(new[] {"::"}, StringSplitOptions.None);

			return components.Length > 1 ? components[0] : null;
		}

		private String ExtractLocationFromLocationName(String locationString)
		{
			var components = locationString.Split(new[] { "::" }, StringSplitOptions.None);

			return components.Length > 1 ? components[1] : locationString;
		}
	}
}
