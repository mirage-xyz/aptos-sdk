using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.Constants;
using Mirage.Aptos.SDK.DTO;

namespace Mirage.Aptos.SDK
{
	public class TransactionAwaiter
	{
		private CancellationTokenSource _cancellationTokenSource;
		private Client _client;

		public TransactionAwaiter(Client client)
		{
			_cancellationTokenSource = new CancellationTokenSource();
			_client = client;
		}

		public async Task<TypedTransaction> WaitForTransactionWithResult(string hash)
		{
			var isPending = true;
			var token = _cancellationTokenSource.Token;
			TypedTransaction transaction = null;
			while (!_cancellationTokenSource.IsCancellationRequested && isPending)
			{
				try
				{
					transaction = await _client.GetTransactionByHash(hash);
					
					if (transaction.Type == TransactionTypes.PendingTransaction)
					{
						await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
					}
					else
					{
						isPending = false;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					break;
				}
			}
			
			return transaction;
		}

		public void StopWait()
		{
			_cancellationTokenSource.Cancel();
		}
	}
}