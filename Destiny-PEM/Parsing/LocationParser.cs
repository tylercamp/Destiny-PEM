using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Parsing.Rules;

namespace DestinyPEM.Parsing
{
	public class LocationParser : Parser
	{
		public LocationParser()
		{
			ParserRules.Add(new LocationParserRule());
		}
	}
}
