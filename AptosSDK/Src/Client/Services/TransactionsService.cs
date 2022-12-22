using System;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.DTO;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.Services
{
	public class TransactionsService : BaseService
	{
		private const string EstimateGasPriceRoute = "/estimate_gas_price";
		private const string SubmitTransactionRoute = "/transactions";
		private const string GetTransactionByHashRoute = @"/transactions/by_hash/{0}";
		private const string JsonWrapperForTransaction = @"{{""transaction"":{0}}}";
		
		public TransactionsService(OpenAPIConfig config) : base(config)
		{
		}
		
		public Task<PendingTransaction> SubmitTransaction(SubmitTransactionRequest requestBody)
		{
			return WebHelper.SendPostRequest<SubmitTransactionRequest, PendingTransaction>(URL + SubmitTransactionRoute, requestBody);
		}
		
		public async Task<TypedTransaction> GetTransactionByHash(string hash)
		{
			var url = URL + string.Format(GetTransactionByHashRoute, hash);
			var wrapper = await WebHelper.SendGetRequest<WrappedTransaction>(url, wrapper: JsonWrapperForTransaction);
			return wrapper.Transaction;
		}
		
		public Task<GasEstimation> EstimateGasPrice()
		{
			return WebHelper.SendGetRequest<GasEstimation>(URL + EstimateGasPriceRoute);
		}
	}
}