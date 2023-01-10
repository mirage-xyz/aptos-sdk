# Mirage Aptos SDK

SKD for integration Aptos features to .NET and Unity platforms

Aptos is a layer 1 blockchain bringing a paradigm shift to Web3 through better technology and user experience. Built with Move to create a home for developers building next-gen applications.

## Installation

Build package

### .NET

**NuGet** (_coming soon_)
```bash
...
```
**Paket**

You can reference the library in the ``paket.dependencies`` file:
```
git git@github.com:mirage-xyz/aptos-sdk.git
```
To read more about Paket options read the [link below](https://fsprojects.github.io/Paket/git-dependencies.html).

### Unity

**OpenUPM** (_coming soon_)
```bash
...
```

You can build package

```bash
dotnet build
```
or download it from [releases](https://github.com/mirage-xyz/aptos-sdk/releases);

## References

To get references documentation please follow the [link](https://aptos-docs.mirage.xyz/api/Mirage.Aptos.SDK.html).

## Examples

### Transfer coins
```csharp
string NodeUrl = "https://fullnode.devnet.aptoslabs.com";
string FaucetUrl = "https://faucet.devnet.aptoslabs.com";

Client client = new Client(NodeUrl);
FaucetClient faucetClient = new FaucetClient(FaucetUrl, _client);
TokenClient tokenClient = new TokenClient(_client);

var from = new Account();
var to = new Account();

await _faucetClient.FundAccount(from, TopUpAmount);

var pendingTransaction = await _coinClient.Transfer(from, to, 1000);

var awaiter = new TransactionAwaiter(_client);
var transaction = await awaiter.WaitForTransactionWithResult(pendingTransaction.Hash);
```

### Create and transfer token

#### Create collection

```csharp
var description = "Collection for test";
var uri = "https://mirage.xyz/";
var collectionName = "Mirage Aptos SDK";

var transaction = await tokenClient.CreateCollection(from, collectionName, description, uri);

var collectionData = await _tokenClient.GetCollectionData(_from.Address, _collectionName);
```

#### Create token

```csharp
var tokenName = "Mirages's first token";
var tokenDescription = "Mirages's simple token";
var transaction = await tokenClient.CreateToken(
	from,
	collectionName,
	tokenName,
	tokenDescription,
	1,
	"https://mirage.xyz/_next/static/videos/video-desktop-8511e2ee740953e08e74b95f401399f7.webm"
);

var tokenData = await tokenClient.GetTokenData(_from.Address, _collectionName, _tokenName);
```

To get more examples check [examples project](AptosSDKTest).