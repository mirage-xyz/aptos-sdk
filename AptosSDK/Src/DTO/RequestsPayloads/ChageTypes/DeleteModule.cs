using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO.ChageTypes
{
	public class DeleteModule : WriteSetChange
	{
		[JsonProperty(PropertyName = "address")]
		public string Address;
		[JsonProperty(PropertyName = "state_key_hash")]
		public string StateKeyHash;
		[JsonProperty(PropertyName = "module")]
		public string Module;
	}
}