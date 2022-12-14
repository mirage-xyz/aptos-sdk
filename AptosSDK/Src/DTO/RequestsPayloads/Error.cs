using Newtonsoft.Json;

namespace Mirage.Aptos.SDK.DTO
{
	public class Error
	{
		[JsonProperty(PropertyName = "message")]
		public string Message;
		[JsonProperty(PropertyName = "error_code")]
		public string ErrorCode;
		[JsonProperty(PropertyName = "vm_error_code")]
		public int? VmErrorCode;
	}
}