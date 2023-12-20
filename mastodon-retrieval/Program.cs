namespace mastodon_retrieval;

public class Program {
	public static void Main(string[] args) {
		string[] domains = File.ReadAllLines("domains.txt");
		Endpoint endpoint = new DirectoryEndpoint();
		endpoint.call(domains);
	}
}
