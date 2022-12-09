using System;
using System.Threading.Tasks;
using Mirage.Aptos.SDK.Constants;
using Mirage.Aptos.SDK.DTO;
using Mirage.Aptos.Constants;

namespace Mirage.Aptos.SDK
{
	public class TokenClient : SpecificClient
	{
		public TokenClient(Client client) : base(client, ABIs.GetTokenABIs())
		{
		}

		public async Task<PendingTransaction> CreateCollection(
			Account account,
			string name,
			string description,
			string uri,
			long maxAmount = UInt32.MaxValue,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var payload = new EntryFunctionPayload
			{
				Type = TransactionPayloadTypes.EntryFunction,
				Function = FunctionTypes.CreateCollectionScript,
				TypeArguments = Array.Empty<string>(),
				Arguments = new object[]
					{ name, description, uri, maxAmount.ToString(), new bool[] { false, false, false } }
			};

			var receipt = await GenerateSignSubmitTransaction(account, payload, extraArgs);

			return receipt;
		}

		public async Task<PendingTransaction> CreateToken(
			Account account,
			string collectionName,
			string name,
			string description,
			ulong supply,
			string uri,
			ulong max = UInt64.MaxValue,
			string royaltyPayeeAddress = null,
			int royaltyPointsDenominator = 0,
			int royaltyPointsNumerator = 0,
			string[] propertyKeys = null,
			string[] propertyValues = null,
			string[] propertyTypes = null,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var payload = CreateTokenPayload(
				account,
				collectionName,
				name,
				description,
				supply,
				uri,
				max,
				royaltyPayeeAddress,
				royaltyPointsDenominator,
				royaltyPointsNumerator,
				propertyKeys,
				propertyValues,
				propertyTypes
			);

			var receipt = await GenerateSignSubmitTransaction(account, payload, extraArgs);

			return receipt;
		}

		private EntryFunctionPayload CreateTokenPayload(
			Account account,
			string collectionName,
			string name,
			string description,
			ulong supply,
			string uri,
			ulong max = UInt64.MaxValue,
			string royaltyPayeeAddress = null,
			int royaltyPointsDenominator = 0,
			int royaltyPointsNumerator = 0,
			string[] propertyKeys = null,
			string[] propertyValues = null,
			string[] propertyTypes = null
		)
		{
			if (royaltyPayeeAddress == null)
			{
				royaltyPayeeAddress = account.Address;
			}

			if (propertyKeys == null)
			{
				propertyKeys = Array.Empty<string>();
			}

			if (propertyValues == null)
			{
				propertyValues = Array.Empty<string>();
			}

			if (propertyTypes == null)
			{
				propertyTypes = Array.Empty<string>();
			}

			return new EntryFunctionPayload
			{
				Type = TransactionPayloadTypes.EntryFunction,
				Function = FunctionTypes.CreateTokenScript,
				TypeArguments = Array.Empty<string>(),
				Arguments = new object[]
				{
					collectionName,
					name,
					description,
					supply.ToString(),
					max.ToString(),
					uri,
					royaltyPayeeAddress,
					royaltyPointsDenominator.ToString(),
					royaltyPointsNumerator.ToString(),
					new bool[] { false, false, false, false, false },
					propertyKeys,
					propertyValues,
					propertyTypes
				}
			};
		}

		public async Task<PendingTransaction> OfferToken(
			Account account,
			string receiver,
			string creator,
			string collectionName,
			string name,
			long amount,
			long propertyVersion = 0,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var payload = new EntryFunctionPayload
			{
				Type = TransactionPayloadTypes.EntryFunction,
				Function = FunctionTypes.OfferScript,
				TypeArguments = Array.Empty<string>(),
				Arguments = new object[]
					{ receiver, creator, collectionName, name, propertyVersion.ToString(), amount.ToString() }
			};

			var receipt = await GenerateSignSubmitTransaction(account, payload, extraArgs);

			return receipt;
		}
		
		public async Task<PendingTransaction> ClaimToken(
			Account account,
			string sender,
			string creator,
			string collectionName,
			string name,
			long propertyVersion = 0,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var payload = new EntryFunctionPayload
			{
				Type = TransactionPayloadTypes.EntryFunction,
				Function = FunctionTypes.ClaimScript,
				TypeArguments = Array.Empty<string>(),
				Arguments = new object[]
					{ sender, creator, collectionName, name, propertyVersion.ToString() }
			};

			var receipt = await GenerateSignSubmitTransaction(account, payload, extraArgs);

			return receipt;
		}

		private async Task<PendingTransaction> GenerateSignSubmitTransaction(
			Account account,
			EntryFunctionPayload payload,
			OptionalTransactionArgs extraArgs = null
		)
		{
			var transaction = await PrepareTransaction(account, payload, extraArgs);

			var raw = transaction.GetRaw();
			var signature = _signatureBuilder.GetSignature(account, raw);
			var request = transaction.GetRequest(payload, signature);

			var receipt = await _client.SubmitTransaction(request);

			return receipt;
		}
	}
}