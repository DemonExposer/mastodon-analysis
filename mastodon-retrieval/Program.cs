using System.Text.Json;
using System.Text.Json.Nodes;

namespace mastodon_retrieval;

public class Program {
	public static async Task Main(string[] args) {
		HttpClient client = new () {
			BaseAddress = new Uri("https://sigmoid.social"),
		};
		
		HttpResponseMessage result = await client.GetAsync("/api/v1/directory?local=true&limit=5&offset=0");
		int remainingRequests = int.Parse(result.Headers.GetValues("X-RateLimit-Remaining").First());
		string resetDate = result.Headers.GetValues("X-RateLimit-Reset").First();
		Console.WriteLine(remainingRequests);
		Console.WriteLine(resetDate);
		JsonArray accounts = JsonNode.Parse(await result.Content.ReadAsStringAsync())!.AsArray();
		Console.WriteLine(JsonSerializer.Serialize(accounts, new JsonSerializerOptions {WriteIndented = true}));
	}
}