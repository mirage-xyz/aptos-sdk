using System.Numerics;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.Constants;
using Mirage.Aptos.SDK.DTO;
using Mirage.Aptos.Constants;
using TransactionPayloadABI = Mirage.Aptos.Imlementation.ABI.TransactionPayload;

namespace Mirage.Aptos.SDK
{
	public class CoinClient: SpecificClient
	{
		private const string AptosCoinType = "0x1::aptos_coin::AptosCoin";

		public CoinClient(Client client) : base(client, ABIs.GetCoinABIs())
		{
		}

		public async Task<PendingTransaction> Transfer(Account from, Account to, ulong amount)
		{
			var payload = GetPayload(to, amount);
			var transaction = await PrepareTransaction(from, payload);

			var raw = transaction.GetRaw();
			var signature = _signatureBuilder.GetSignature(from, raw);
			var request = transaction.GetRequest(payload, signature);
			
			var receipt = await _client.SubmitTransaction(request);

			return receipt;
		}

		private EntryFunctionPayload GetPayload(Account to, ulong amount)
		{
			return new EntryFunctionPayload
			{
				Type = TransactionPayloadTypes.EntryFunction,
				Function = FunctionTypes.Transfer,
				TypeArguments = new string[] { AptosCoinType },
				Arguments = new string[] { to.Address, amount.ToString() }
			};
		}

		public async Task<BigInteger> GetBalance(Account account)
		{
			var typeTag = $"0x1::coin::CoinStore<{AptosCoinType}>";
			var resource = await _client.GetAccountResource(account, typeTag);

			var data = resource.Data.ToObject<CoinStoreType>();

			return BigInteger.Parse(data.Coin.Value);
		}
	}
}