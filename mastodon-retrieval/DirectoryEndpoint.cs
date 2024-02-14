using Newtonsoft.Json.Linq;

namespace mastodon_retrieval;

public class DirectoryEndpoint : Endpoint {
	public override void call(string[] domains) {
		foreach (string domain in domains) try {
			HttpClient client = new () {
				BaseAddress = new Uri($"https://{domain}")
			};

			JArray totalAccounts = new ();
			int fileNo = 0;
			for (int offset = 0;; offset += 80) { // pagination: 80 accounts at once, add them all together at the end
				Task<HttpResponseMessage> task = Task.Run(() => client.GetAsync($"/api/v1/directory?local=true&limit=80&offset={offset}"));
				task.Wait();
				HttpResponseMessage result = task.Result;
				int remainingRequests = int.Parse(result.Headers.GetValues("X-RateLimit-Remaining").First());
				string resetDate = result.Headers.GetValues("X-RateLimit-Reset").First();
				Console.WriteLine(remainingRequests);
				Console.WriteLine(resetDate);
				DateTime properDateTime = DateTime.Parse(resetDate);

				JArray accounts = JArray.Parse(new StreamReader(result.Content.ReadAsStream()).ReadToEnd());

				foreach (JToken? t in accounts)
					totalAccounts.Add(t.Value<JObject>()!);

				if (totalAccounts.Count > 10000) { // make sure that memory doesn't get full, so if the data is more than 10000 accounts, split it up into multiple files
					File.WriteAllText($"accounts_{domain}_{fileNo++}.json", totalAccounts.ToString());
					totalAccounts = new JArray();
				}

				if (remainingRequests == 0) { // don't overload the API, wait until more requests are allowed (plus 5 seconds to be safe) when out of requests
					TimeSpan waitingTime = properDateTime.Subtract(DateTime.Now).Add(new TimeSpan(0, 0, 5));
					Thread.Sleep(waitingTime);
				}

				if (accounts.Count < 80)
					break;
			}

			File.WriteAllText($"accounts_{domain}_{fileNo}.json", totalAccounts.ToString());
		} catch (Exception) { // not a great way of handling errors, but this should only be triggered when a server could not be reached or if it doesn't use HTTPS
			Console.WriteLine("couldn't retrieve data, continuing...");
		}
	}
}