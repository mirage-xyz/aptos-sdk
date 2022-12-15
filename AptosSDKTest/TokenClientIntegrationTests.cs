using System;
using System.Threading.Tasks;
using Mirage.Aptos.SDK;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AptosSDKTest
{
	[TestFixture]
	public class TokenClientIntegrationTests
	{
		private const string NodeUrl = "https://fullnode.devnet.aptoslabs.com";
		private const string FaucetUrl = "https://faucet.devnet.aptoslabs.com";
		private const int TopUpAmount = 100000000;

		private Client _client;
		private FaucetClient _faucetClient;
		private TokenClient _tokenClient;
		
		[OneTimeSetUp]
		public void SetUp()
		{
			_client = new Client(NodeUrl);
			_faucetClient = new FaucetClient(FaucetUrl, _client);
			_tokenClient = new TokenClient(_client);
		}
		
		public async Task RequestTransaction(string hash)
		{
			var tx = await _client.GetTransactionByHash(hash);
			// Console.WriteLine(JsonConvert.SerializeObject(tx));
		}
		
		public async Task RequestMinedTransaction(string hash)
		{
			var awaiter = new TransactionAwaiter(_client);
			var transaction = await awaiter.WaitForTransactionWithResult(hash);
			Console.WriteLine(JsonConvert.SerializeObject(transaction));
		}

		[Test]
		public async Task CreateCollection()
		{
			var from = new Account();
			var to = new Account();
			
			await _faucetClient.FundAccount(from, TopUpAmount);
			await _faucetClient.FundAccount(to, TopUpAmount);
			
			Console.WriteLine("----- Create collection -----");
			var collectionName = "Mirage Aptos SDK 4";
			var description = "Collection for test Aptos SDK";
			var uri = "https://mirage.xyz/";

			var hash = await _tokenClient.CreateCollection(from, collectionName, description, uri);
			Console.WriteLine(hash.Hash);
			
			await RequestTransaction(hash.Hash);

			Console.WriteLine("----- Create token -----");
			var tokenName = "Mirages's first token";
			var tokenDescription = "Mirages's simple token";
			var hash1 = await _tokenClient.CreateToken(
				from,
				collectionName,
				tokenName,
				description,
				1,
				"https://mirage.xyz/_next/static/videos/video-desktop-8511e2ee740953e08e74b95f401399f7.webm"
			);
			
			Console.WriteLine(hash1.Hash);
			await RequestTransaction(hash1.Hash);

			Console.WriteLine("----- Offer token -----");
			var tokenPropertyVersion = 0;
			var hash3 = await _tokenClient.OfferToken(
				from,
				to.Address,
				from.Address,
				collectionName,
				tokenName,
				1,
				tokenPropertyVersion
			);
			
			Console.WriteLine(hash3.Hash);
			await RequestTransaction(hash3.Hash);

			Console.WriteLine("----- Claim token -----");
			var hash4 = await _tokenClient.ClaimToken(
				to,
				from.Address,
				from.Address,
				collectionName,
				tokenName,
				tokenPropertyVersion
			);
			
			Console.WriteLine(hash4.Hash);
			await RequestTransaction(hash4.Hash);
		}
	}
}