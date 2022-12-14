using Mirage.Aptos.Infrastructure;
using Mirage.Aptos.Utils;

namespace Mirage.Aptos.SDK
{
	public static class DataUtils
	{
		public static byte[] Serialize<T>(T serializable) where T : SerializableAbiPart<T>
		{
			var serializer = new Serializer();
			serializable.Serialize(serializer);
			return serializer.GetBytes();
		}
	}
}