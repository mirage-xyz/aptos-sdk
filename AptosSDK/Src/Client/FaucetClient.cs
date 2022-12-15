using System.Collections.Generic;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.DTO;

namespace Mirage.Aptos.SDK
{
	public class FaucetClient
	{
		private const string FundAccountRoute = "/mint";
		
		private string _url;
		private Client _client;
		
		public FaucetClient(string faucetUrl, Client client)
		{
			_url = faucetUrl;
			_client = client;
		}

		public async Task<TypedTransaction[]> FundAccount(Account account, uint amount)
		{
			var query = new Dictionary<string, string>
			{
				{ "address", account.Address },
				{ "amount", amount.ToString() }
			};
			var txnHashes = await WebHelper.SendPostRequest<string[]>(_url + FundAccountRoute, query: query);

			var awaitedAnswers = await Task.WhenAll(CreateAwaiters(txnHashes));

			return awaitedAnswers;
		}

		private IEnumerable<Task<TypedTransaction>> CreateAwaiters(string[] hashes)
		{
			foreach (var hash in hashes)
			{
				var awaiter = new TransactionAwaiter(_client);
				yield return awaiter.WaitForTransactionWithResult(hash);
			}
		}
	}
}