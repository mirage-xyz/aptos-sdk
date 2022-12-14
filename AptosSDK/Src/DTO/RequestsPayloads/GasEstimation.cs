using System;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	[Serializable]
	public class GasEstimation
	{
		[JsonProperty(PropertyName = "deprioritized_gas_estimate")]
		public ulong DeprioritizedGasEstimate;
		
		[JsonProperty(PropertyName = "gas_estimate")]
		public ulong GasEstimate;
		
		[JsonProperty(PropertyName = "prioritized_gas_estimate")]
		public ulong PrioritizedGasEstimate;
	}
}