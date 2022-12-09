using System;
using System.Threading.Tasks;
using Mirage.Aptos.SDK;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AptosSDKTest
{
	[TestFixture]
	public class CoinClientTests
	{
		private readonly byte[] _alicePrivateKey = new byte[] {255,211,113,35,165,87,101,140,224,222,92,33,154,65,150,110,140,93,2,42,28,171,127,97,43,26,129,71,81,123,43,127,184,15,11,253,79,245,134,84,235,194,101,199,183,86,195,6,154,234,47,136,15,71,94,119,91,201,60,202,25,182,116,124};
		private readonly byte[] _bobPrivateKey = new byte[] {20,225,26,255,205,22,88,59,107,19,118,175,43,243,114,90,198,51,246,142,39,197,124,171,28,60,58,214,46,21,146,143,18,48,212,127,244,102,145,168,135,173,213,67,205,70,19,246,184,65,43,96,86,181,231,77,73,199,154,39,255,120,32,129};
		
		private Client _client;
		private CoinClient _coinClient;

		[SetUp]
		public void SrtUp()
		{
			_client = new Client("https://fullnode.devnet.aptoslabs.com");
			_coinClient = new CoinClient(_client);
		}

		[Test]
		public async Task SignTransaction()
		{
			Console.WriteLine("------ Accounts -------");
			var from = new Account(_alicePrivateKey);
			Console.WriteLine($"Alice: {from.GetAddress()}");
			var to = new Account(_bobPrivateKey);
			Console.WriteLine($"Bob: {to.GetAddress()}");
			
			Console.WriteLine("------ Balances --------");
			Console.WriteLine($"Alice's balance: {await _coinClient.GetBalance(from)}");
			Console.WriteLine($"Bob's balance: {await _coinClient.GetBalance(to)}");
			
			Console.WriteLine("------ Transaction --------");
			var hash = await _coinClient.Transfer(from, to, 1000);
			Console.WriteLine("hash = " + hash);
			Console.WriteLine("hash = " + hash);

			var awaiter = new TransactionAwaiter(_client);
			var transaction = await awaiter.WaitForTransactionWithResult(hash);
			
			Console.WriteLine(JsonConvert.SerializeObject(transaction));
		}
	}
}