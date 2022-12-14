using Mirage.Aptos.SDK.Converters;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	public class UserTransaction : TypedTransaction
	{
		[JsonProperty(PropertyName = "version")]
		public string Version;
		[JsonProperty(PropertyName = "hash")]
		public string Hash;
		[JsonProperty(PropertyName = "state_change_hash")]
		public string StateChangeHash;
		[JsonProperty(PropertyName = "event_root_hash")]
		public string EventRootHash;
		[JsonProperty(PropertyName = "state_checkpoint_hash")]
		public string StateCheckpointHash;
		[JsonProperty(PropertyName = "gas_used")]
		public string GasUsed;
		[JsonProperty(PropertyName = "success")]
		public bool Success;
		[JsonProperty(PropertyName = "vm_status")]
		public string VmStatus;
		[JsonProperty(PropertyName = "accumulator_root_hash")]
		public string AccumulatorRootHash;
		[JsonProperty(PropertyName = "changes")]
		[JsonConverter(typeof(WriteSetChangeArrayConverter))]
		public WriteSetChange[] Changes;
		[JsonProperty(PropertyName = "sender")]
		public string Sender;
		[JsonProperty(PropertyName = "sequence_number")]
		public string SequenceNumber;
		[JsonProperty(PropertyName = "max_gas_amount")]
		public string MaxGasAmount;
		[JsonProperty(PropertyName = "gas_unit_price")]
		public string GasUnitPrice;
		[JsonProperty(PropertyName = "expiration_timestamp_secs")]
		public string ExpirationTimestampSecs;
		[JsonProperty(PropertyName = "payload")]
		[JsonConverter(typeof(TransactionPayloadConverter))]
		public TransactionPayload Payload;
		[JsonProperty(PropertyName = "signature")]
		[JsonConverter(typeof(TransactionSignatureConverter))]
		public TransactionSignature Signature;
		[JsonProperty(PropertyName = "events")]
		public Event[] Events;
		[JsonProperty(PropertyName = "timestamp")]
		public string Timestamp;
	}
}