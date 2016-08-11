using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DestinyPEM.Parsing.Rules;
using DestinyPEM.Model;

namespace DestinyPEM.Parsing
{
	public class TraversalTimesParser : Parser
	{
		public TraversalTimesParser()
		{
			ParserRules.Add(new ConstantParserRule());
			ParserRules.Add(new TravelToPlanetConstantParserRule());
			ParserRules.Add(new LinkParserRule());
		}
	}
}
