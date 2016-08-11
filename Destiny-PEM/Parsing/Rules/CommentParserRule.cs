using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DestinyPEM.Parsing.Rules
{
	public class CommentParserRule : IParserRule
	{
		Regex matcher = new Regex(@"\#.*");
		public bool CanParse(string line)
		{
			return matcher.IsMatch(line);
		}

		public object ParseLine(string line)
		{
			return null;
		}
	}
}
