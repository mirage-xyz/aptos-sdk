using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	public class PendingTransaction : SubmitTransactionRequest
	{
		[JsonProperty(PropertyName = "hash")]
		public string Hash;
	}
}