using System;
using System.Numerics;
using System.Threading.Tasks;
using Mirage.Aptos.SDK;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AptosSDKTest
{
	[TestFixture]
	public class CoinClientIntegrationTests
	{
		private const string NodeUrl = "https://fullnode.devnet.aptoslabs.com";
		private const string FaucetUrl = "https://faucet.devnet.aptoslabs.com";
		private const int TopUpAmount = 100000000;

		private Client _client;
		private CoinClient _coinClient;
		private FaucetClient _faucetClient;
		private Account _aliceAccount;
		private Account _bobAccount;

		[OneTimeSetUp]
		public void SetUp()
		{
			_client = new Client(NodeUrl);
			_coinClient = new CoinClient(_client);
			_faucetClient = new FaucetClient(FaucetUrl, _client);
			_aliceAccount = new Account();
			_bobAccount = new Account();
		}

		[Test]
		public async Task TopUpAccount()
		{
			var account = new Account();

			await _faucetClient.FundAccount(account, TopUpAmount);
			var balance1 = await _coinClient.GetBalance(account);

			await _faucetClient.FundAccount(account, TopUpAmount);
			var balance2 = await _coinClient.GetBalance(account);

			Assert.AreEqual(balance1 + TopUpAmount, balance2);
		}

		[Test]
		public async Task SignTransaction()
		{
			Console.WriteLine("------ Accounts -------");
			var from = new Account();
			Console.WriteLine($"Alice: {from.GetAddress()}");
			var to = new Account();
			Console.WriteLine($"Bob: {to.GetAddress()}");

			await _faucetClient.FundAccount(from, TopUpAmount);
			await _faucetClient.FundAccount(to, TopUpAmount);

			Console.WriteLine("------ Balances --------");
			Console.WriteLine($"Alice's balance: {await _coinClient.GetBalance(from)}");
			Console.WriteLine($"Bob's balance: {await _coinClient.GetBalance(to)}");

			Console.WriteLine("------ Transaction --------");
			var pendingTransaction = await _coinClient.Transfer(from, to, 1000);
			Console.WriteLine("hash = " + pendingTransaction.Hash);

			var awaiter = new TransactionAwaiter(_client);
			var transaction = await awaiter.WaitForTransactionWithResult(pendingTransaction.Hash);

			Console.WriteLine(JsonConvert.SerializeObject(transaction));
		}
	}
}