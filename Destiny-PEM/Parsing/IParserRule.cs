using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DestinyPEM.Parsing
{
	public interface IParserRule
	{
		/// <summary>
		/// Whether or not the IParserRule can parse the given line.
		/// </summary>
		bool CanParse(String line);

		/// <summary>
		/// Parses the given line based on internally-defined rules.
		/// </summary>
		/// <param name="line">The line to parse</param>
		/// <returns>A member of the domain model</returns>
		object ParseLine(String line);
	}
}
