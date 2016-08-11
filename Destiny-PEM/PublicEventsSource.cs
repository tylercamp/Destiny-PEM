using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DestinyPEM.Logging;
using DestinyPEM.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DestinyPEM
{
	//	http://destiny.wikia.com/wiki/Public_Event timings?

	public class PublicEventsSource
	{
		//	Format: QueryUri + "?_=(UNIX MS TIME)"
		private static String QueryUri = "http://destinypublicevents.com/ws/timerjson.php";

		public GalaxyMap Reference { get; private set; }

		public PublicEventsSource(GalaxyMap reference)
		{
			Reference = reference;
		}

		public List<Event> PollForEvents(DateTime pollTime)
		{
			return PollForEventsAsync(DateTime.UtcNow).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<List<Event>> PollForEventsAsync(DateTime pollTime)
		{
			if (pollTime.Kind != DateTimeKind.Utc)
				pollTime = pollTime.ToUniversalTime();

			//	http://stackoverflow.com/questions/5955883/datetimes-representation-in-milliseconds
			var unixEpochTime = pollTime.ToUniversalTime().Subtract(
					new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				).TotalMilliseconds;

			var queryString = String.Format("{0}?_={1}", QueryUri, Math.Round(unixEpochTime));
			HttpClient client = new HttpClient();
			var response = await client.GetAsync(new Uri(queryString)).ConfigureAwait(false);

			if (!response.IsSuccessStatusCode)
			{
				Logger.LogError("Unable to pull for latest public events from URI '{0}', error {1}", queryString, response.StatusCode);
				return null;
			}

			var textResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var jsonData = JsonConvert.DeserializeObject(textResponse) as JArray;
			if (jsonData == null)
			{
				Logger.LogError("Unable to parse public events info, invalid JSON format.");
				return null;
			}

			List<Event> result = new List<Event>();
			foreach (var jsonObject in jsonData)
			{
				if (jsonObject["planetName"] == null)
					continue;

				var eventJsonObjects = jsonObject["mapLocations"] as JArray;
				foreach (var eventJsonObject in eventJsonObjects)
				{
					Location locationForEvent = Reference.FindLocation(eventJsonObject["title"].Value<String>());
					if (locationForEvent == null)
						Debugger.Break();
					var newEvent = new Event
					{
						Location = locationForEvent,
						Rarity = eventJsonObject["frequency"].Value<int>(),
						StartTime = pollTime + TimeSpan.FromSeconds(eventJsonObject["start"].Value<int>())
					};
					newEvent.EndTime = newEvent.StartTime + TimeSpan.FromMinutes(7.5); // 5 minute event window, assume at most an additional 2.5 minutes for completion
					result.Add(newEvent);
				}
			}


			return result;
		}
	}
}
