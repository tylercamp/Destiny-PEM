using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Model;

namespace DestinyPEM.Tests
{
	static class PrebuiltGalaxyMap
	{
		static GalaxyMap _object = null;
		public static GalaxyMap Object
		{
			get
			{
				if (_object == null)
				{
					_object = GalaxyMapBuilder.Build(
						travelTimesFileText: File.ReadAllText("travel-times.txt"),
						locationsListFileText: File.ReadAllText("locations-list.txt")
						);
				}

				return _object;
			}
		}
	}
}
