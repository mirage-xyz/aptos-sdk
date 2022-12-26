using System;
using System.Threading.Tasks;
using Mirage.Aptos.SDK;
using Mirage.Aptos.SDK.DTO;
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
		private Account _from;
		private Account _to;
		
		private string _collectionName = "Mirage Aptos SDK";
		private string _tokenName = "Mirages's first token";
		private long _tokenPropertyVersion = 0;
		
		[OneTimeSetUp]
		public void SetUp()
		{
			_client = new Client(NodeUrl);
			_faucetClient = new FaucetClient(FaucetUrl, _client);
			_tokenClient = new TokenClient(_client);
			
			_from = new Account();
			_to = new Account();
		}

		[Test, Order(1)]
		public async Task FundAccounts()
		{
			await _faucetClient.FundAccount(_from, TopUpAmount);
			await _faucetClient.FundAccount(_to, TopUpAmount);
		}

		[Test, Order(2)]
		public async Task CreateCollection()
		{
			var description = "Collection for test Aptos SDK";
			var uri = "https://mirage.xyz/";

			var transaction = await _tokenClient.CreateCollection(_from, _collectionName, description, uri);
			Console.WriteLine(transaction.Hash);
			
			await RequestMinedTransaction(transaction.Hash);

			var collectionData = await _tokenClient.GetCollectionData(_from.Address, _collectionName);
			
			Console.WriteLine(JsonConvert.SerializeObject(collectionData));
		}

		[Test, Order(3)]
		public async Task CreateToken()
		{
			var tokenDescription = "Mirages's simple token";
			var transaction = await _tokenClient.CreateToken(
				_from,
				_collectionName,
				_tokenName,
				tokenDescription,
				1,
				"https://mirage.xyz/_next/static/videos/video-desktop-8511e2ee740953e08e74b95f401399f7.webm"
			);
			
			await RequestMinedTransaction(transaction.Hash);

			var tokenData = await _tokenClient.GetTokenData(_from.Address, _collectionName, _tokenName);
			
			Console.WriteLine(JsonConvert.SerializeObject(tokenData));
		}

		[Test, Order(4)]
		public async Task OfferToken()
		{
			var transaction = await _tokenClient.OfferToken(
				_from,
				_to.Address,
				_from.Address,
				_collectionName,
				_tokenName,
				1,
				_tokenPropertyVersion
			);
			
			Console.WriteLine(transaction.Hash);
			
			await RequestMinedTransaction(transaction.Hash);
			
			await ShowAccountsBalances();
		}

		[Test, Order(5)]
		public async Task ClaimToken()
		{
			var transaction = await _tokenClient.ClaimToken(
				_to,
				_from.Address,
				_from.Address,
				_collectionName,
				_tokenName,
				_tokenPropertyVersion
			);
			
			Console.WriteLine(transaction.Hash);
			await RequestMinedTransaction(transaction.Hash);

			await ShowAccountsBalances();
		}

		private async Task ShowAccountsBalances()
		{
			var fromTokenBalance = await _tokenClient.GetToken(_from.Address, _collectionName, _tokenName, _tokenPropertyVersion);
			var toTokenBalance = await _tokenClient.GetToken(_to.Address, _collectionName, _tokenName, _tokenPropertyVersion);
			Console.WriteLine("From balance: " + fromTokenBalance.Amount);
			Console.WriteLine("To balance: " + toTokenBalance.Amount);
		}
		
		private async Task RequestTransaction(string hash)
		{
			var tx = await _client.GetTransactionByHash(hash);
			// Console.WriteLine(JsonConvert.SerializeObject(tx));
		}
		
		private Task<TypedTransaction> RequestMinedTransaction(string hash)
		{
			var awaiter = new TransactionAwaiter(_client);
			return awaiter.WaitForTransactionWithResult(hash);
		}
	}
}