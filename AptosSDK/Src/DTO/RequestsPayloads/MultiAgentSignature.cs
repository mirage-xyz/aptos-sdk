using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	public class MultiAgentSignature : TransactionSignature
	{
		[JsonProperty(PropertyName = "sender")]
		public TransactionSignature Sender;
		[JsonProperty(PropertyName = "secondary_signer_addresses")]
		public string[] SecondarySignerAddresses;
		[JsonProperty(PropertyName = "secondary_signers")]
		public TransactionSignature[] SecondarySigners;
	}
}