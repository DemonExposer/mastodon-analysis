using Newtonsoft.Json.Linq;

namespace mastodon_retrieval; 

public class InstanceEndpoint : Endpoint {
	public override void call(string[] domains) {
		Console.WriteLine(domains.Length);
		foreach (string domain in domains) try {
			HttpClient client = new () {
				BaseAddress = new Uri($"https://{domain}")
			};
			Console.WriteLine(domain);

			Task<HttpResponseMessage> task = Task.Run(() => client.GetAsync("/api/v2/instance"));
			task.Wait();
			HttpResponseMessage result = task.Result;

			JObject data = JObject.Parse(new StreamReader(result.Content.ReadAsStream()).ReadToEnd());

			File.WriteAllText($"instance_data_{domain}.json", data.ToString());
		} catch (Exception) {
			Console.WriteLine("couldn't retrieve data, continuing...");
		}
	}
}