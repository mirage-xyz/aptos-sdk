using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.DTO;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK
{
	/// <summary>
	/// Provides methods for retrieving data from Aptos node.
	/// </summary>
	/// <seealso href="https://fullnode.devnet.aptoslabs.com/v1/spec"/>
	public class Client
	{
		private const int DefaultMaxGasAmount = 200000;
		private const int DefaultTxnExpSecFromNow = 20;

		private readonly ClientServices _services;

		/// <summary>
		/// Build a client configured to connect to an Aptos node at the given URL.
		/// </summary>
		/// <param name="nodeUrl">URL of the Aptos Node API endpoint.</param>
		/// <param name="config">Additional configuration options for the generated Axios client.</param>
		public Client(string nodeUrl, OpenAPIConfig config = null)
		{
			if (config == null)
			{
				config = new OpenAPIConfig();
			}

			config.Base = nodeUrl;

			_services = new ClientServices(config);
		}

		public async Task PopulateRequestParams(
			SubmitTransaction transaction,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var ledgerInfo = await _services.GeneralService.GetLedgerInfo();
			var account = await _services.AccountsService.GetAccount(transaction.Sender.Address);
			var gasUnitPrice = extraArgs?.GasUnitPrice;
			if (gasUnitPrice == null)
			{
				var gasEstimation = await _services.TransactionsService.EstimateGasPrice();
				gasUnitPrice = gasEstimation.GasEstimate;
			}

			var expireTimestamp =
				(uint)Math.Floor((double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000 +
				                          DefaultTxnExpSecFromNow));

			transaction.SequenceNumber = account.SequenceNumber;
			transaction.MaxGasAmount = DefaultMaxGasAmount;
			transaction.GasUnitPrice = (ulong)gasUnitPrice;
			transaction.ExpirationTimestampSecs = expireTimestamp;
			transaction.ChainID = ledgerInfo.ChainID;
		}

		public Task<PendingTransaction> SubmitTransaction(SubmitTransactionRequest request)
		{
			return _services.TransactionsService.SubmitTransaction(request);
		}

		public Task<TypedTransaction> GetTransactionByHash(string hash)
		{
			return _services.TransactionsService.GetTransactionByHash(hash);
		}

		public Task<MoveResource> GetAccountResource(string account, string resourceType)
		{
			return _services.AccountsService.GetAccountResource(account, resourceType);
		}
		
		public Task<TReturn> GetTableItem<TReturn>(string tableHandle, TableItemRequest requestBody)
		{
			return _services.TableService.GetTableItem<TReturn>(tableHandle, requestBody);
		}
	}
}