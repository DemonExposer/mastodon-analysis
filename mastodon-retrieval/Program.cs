using Newtonsoft.Json.Linq;

namespace mastodon_retrieval;

public class Program {
	public static async Task Main(string[] args) {
		string[] domains = await File.ReadAllLinesAsync("domains.txt");
		
		foreach (string domain in domains) try {
			HttpClient client = new() {
				BaseAddress = new Uri($"https://{domain}")
			};

			JArray totalAccounts = new JArray();
			for (int offset = 0;; offset += 80) {
				HttpResponseMessage result =
					await client.GetAsync($"/api/v1/directory?local=true&limit=80&offset={offset}");
				int remainingRequests = int.Parse(result.Headers.GetValues("X-RateLimit-Remaining").First());
				string resetDate = result.Headers.GetValues("X-RateLimit-Reset").First();
				Console.WriteLine(remainingRequests);
				Console.WriteLine(resetDate);
				DateTime properDateTime = DateTime.Parse(resetDate);

				JArray accounts = JArray.Parse(await result.Content.ReadAsStringAsync());

				foreach (JToken? t in accounts)
					totalAccounts.Add(t.Value<JObject>()!);

				if (remainingRequests == 0) {
					TimeSpan waitingTime = properDateTime.Subtract(DateTime.Now).Add(new TimeSpan(0, 0, 5));
					Thread.Sleep(waitingTime);
				}

				if (accounts.Count < 80)
					break;
			}

			await File.WriteAllTextAsync($"accounts_{domain}.json", totalAccounts.ToString());
		} catch (Exception e) {
			Console.WriteLine("pech man");
		}
	}
}