using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.DTO;
using Newtonsoft.Json;

namespace Mirage.Aptos.SDK
{
	public static class WebHelper
	{
		private const string MimeType = "application/json";
		public static Task<TResultType> SendPostRequest<TPayloadType, TResultType>(
			string url,
			TPayloadType payload,
			Dictionary<string, string> headers = null
		)
		{
			var payloadJson = JsonConvert.SerializeObject(payload);

			return SendChangeRequest<TResultType>(url, payloadJson, headers);
		}
		
		public static Task<TResultType> SendPostRequest<TResultType>(
			string url,
			Dictionary<string, string> headers = null
		)
		{
			var payloadJson = JsonConvert.SerializeObject(string.Empty);

			return SendChangeRequest<TResultType>(url, payloadJson, headers);
		}

		public static async Task<TResultType> SendChangeRequest<TResultType>(
			string url,
			string payload,
			Dictionary<string, string> headers = null
		)
		{
			var content = new StringContent(payload);
			content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
			
			using (var client = new HttpClient())
			{
				if (headers != null)
				{
					AddHeaders(client, headers);
				}
				
				client.Timeout = TimeSpan.FromSeconds(20);
				
				var answer = await client.PostAsync(url, content);

				var json = await answer.Content.ReadAsStringAsync();
				if (answer.IsSuccessStatusCode)
				{
					try
					{
						var result = JsonConvert.DeserializeObject<TResultType>(json);
						return result;
					}
					catch (Exception e)
					{
						Console.WriteLine($"Error while deserializing response: {e.Message}");
						throw e;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(json))
					{
						throw new InvalidOperationException("Some fucking bug.");
					}
					else
					{
						var error = JsonConvert.DeserializeObject<Error>(json);
						Console.WriteLine(AptosException.CreateMessage(error.Message, error.ErrorCode,
							error.VmErrorCode));
						throw new AptosException(error.Message, error.ErrorCode, error.VmErrorCode);
					}
				}
			}
		}

		public static async Task<TResultType> SendGetRequest<TResultType>(
			string urlWithQuery,
			Dictionary<string, string> headers = null,
			string wrapper = null
		)
		{
			using (var request = new HttpClient())
			{
				if (headers != null)
				{
					AddHeaders(request, headers);
				}
				
				request.Timeout = TimeSpan.FromSeconds(20);
				
				var answer = await request.GetAsync(urlWithQuery);

				var json = await answer.Content.ReadAsStringAsync();
				if (answer.IsSuccessStatusCode)
				{
					try
					{
						var jsonPayload = json;
						if (wrapper != null)
						{
							jsonPayload = string.Format(wrapper, json);
						}

						var result = JsonConvert.DeserializeObject<TResultType>(jsonPayload);
						return result;
					}
					catch (Exception e)
					{
						Console.WriteLine($"Error while deserializing response: {e.Message}");
						throw e;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(json))
					{
						throw new InvalidOperationException("Some fucking bug.");
					}
					else
					{
						var error = JsonConvert.DeserializeObject<Error>(json);
						Console.WriteLine(AptosException.CreateMessage(error.Message, error.ErrorCode,
							error.VmErrorCode));
						throw new AptosException(error.Message, error.ErrorCode, error.VmErrorCode);
					}
				}
			}
		}
		private static void AddHeaders(HttpClient request, Dictionary<string, string> headers)
		{
			foreach (var entryHeader in headers)
			{
				request.DefaultRequestHeaders.Add(entryHeader.Key, entryHeader.Value);
			}
		}
	}
}