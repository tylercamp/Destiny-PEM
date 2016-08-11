using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Logging;
using DestinyPEM.Parsing.Rules;

namespace DestinyPEM.Parsing
{
	public class Parser
	{
		private List<object> parsedDomainObjects;

		public List<IParserRule> ParserRules { get; private set; }

		public bool IgnoreBlankLines { get; set; }

		public Parser()
		{
			IgnoreBlankLines = true;

			ParserRules = new List<IParserRule>();
			//	Default rules
			ParserRules.Add(new CommentParserRule());
		}

		public void Parse(String data)
		{
			parsedDomainObjects = new List<object>();

			var newline = new [] {'\n', '\r'};
			var splitOptions = IgnoreBlankLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
			foreach (var line in data.Split(newline, splitOptions))
			{
				//	Find the rule for the current line
				IEnumerable<IParserRule> possibleRules;
				try
				{
					possibleRules = ParserRules.Where(r => r.CanParse(line));
				}
				catch (InvalidOperationException e)
				{
					Logger.LogError("Ambiguous type of data at line '{0}'", line);
					continue;
				}

				if (!possibleRules.Any())
				{
					Logger.LogError("Unknown type of data at line '{0}'", line);
					continue;
				}


				//	Use the rules to extract a meaningful object
				object domainObject = null;
				foreach (var rule in possibleRules)
				{
					try
					{
						domainObject = rule.ParseLine(line);
					}
					catch (Exception e)
					{
						Logger.LogError("Error parsing line '{0}', message: '{1}'", e.Message);
						continue;
					}

					//	Use the first domainObject successfully created by a rule
					if (domainObject != null)
						break;
				}


				//	If a rule created a meaningful object, store it
				if (domainObject != null)
					parsedDomainObjects.Add(domainObject);
			}
		}



		public IEnumerable<TDomainObject> GetAll<TDomainObject>()
		{
			return parsedDomainObjects.Where(o => o is TDomainObject).Cast<TDomainObject>();
		}
	}
}
