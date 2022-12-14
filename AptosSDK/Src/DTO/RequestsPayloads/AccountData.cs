using System;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	[Serializable]
	public class AccountData
	{
		[JsonProperty(PropertyName = "sequence_number")]
		public uint SequenceNumber;
		[JsonProperty(PropertyName = "authentication_key")]
		public string AuthenticationKey;
	}
}